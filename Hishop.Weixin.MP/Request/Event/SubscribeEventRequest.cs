using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Request.Event
{
    public class SubscribeEventRequest : EventRequest
    {
        /// <summary>
        /// tany,2015-09-29
        /// </summary>
        public string EventKey { get; set; }
        public string Ticket { get; set; }

        public override RequestEventType Event
        {
            get {
                return RequestEventType.Subscribe;
            }
            set { }
        }
    }
}
