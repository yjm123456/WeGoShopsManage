﻿using System;
using System.Collections.Generic;
using System.Text;
using Jd.Link.Channel.WebSocket;

namespace Jd.Link.Channel
{
    /// <summary>a channel pool that same uri sharing same channel
    /// </summary>
    public class ClientChannelSharedSelector : IClientChannelSelector
    {
        private static readonly int CONNECTTIMEOUT = 5000;
        private ILoggerFactory _loggerFactory;
        private Object _lockObject;
        private IDictionary<string, IClientChannel> _channels;

        /// <summary>get or set heartbeat interval time in milliseconds
        /// </summary>
        public int HeartbeatPeriod { get; set; }

        public ClientChannelSharedSelector() : this(DefaultLoggerFactory.Default) { }
        public ClientChannelSharedSelector(ILoggerFactory loggerFactory)
        {
            this._loggerFactory = loggerFactory;
            this._lockObject = new object();
            this._channels = new Dictionary<string, IClientChannel>();
        }

        public IClientChannel GetChannel(Uri uri)
        {
            if (!uri.Scheme.Equals("ws", StringComparison.InvariantCultureIgnoreCase))
                return null;

            var url = uri.ToString();

            if (!this.HaveChannel(url))
                lock (this._lockObject)
                    if (!this.HaveChannel(url))
                        this.AddChannel(url, this.WrapChannel(this.Connect(uri, CONNECTTIMEOUT)));

            return _channels[url];
        }
        public void ReturnChannel(IClientChannel channel) { }

        protected virtual IClientChannel Connect(Uri uri, int timeout)
        {
            return WebSocketClient.Connect(this._loggerFactory, uri, timeout);
        }
        private IClientChannel WrapChannel(IClientChannel channel)
        {
            if (this.HeartbeatPeriod > 0)
                channel.HeartbeatTimer = new ResetableTimer(this.HeartbeatPeriod);
            return channel;
        }
        private bool HaveChannel(string url)
        {
            return this._channels.ContainsKey(url) && this._channels[url].IsConnected;
        }
        private void AddChannel(string url, IClientChannel channel)
        {
            if (this._channels.ContainsKey(url))
                this._channels[url] = channel;
            else
                this._channels.Add(url, channel);
        }
    }
}