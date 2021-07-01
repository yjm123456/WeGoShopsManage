﻿using System;
using System.Collections.Generic;

namespace Jd.Api.Util
{
    /// <summary>
    /// Jd容器上下文。
    /// </summary>
    public class JdContext
    {
        private IDictionary<string, string> parameters = new Dictionary<string, string>();

        /// <summary>
        /// 应用编号
        /// </summary>
        public string AppKey
        {
            get { return this["Jd_appkey"]; }
        }

        /// <summary>
        /// 授权码
        /// </summary>
        public string SessionKey
        {
            get { return this["Jd_session"]; }
        }

        /// <summary>
        /// 回调签名
        /// </summary>
        public string Signature
        {
            get { return this["Jd_sign"]; }
        }

        /// <summary>
        /// 京东用户编号
        /// </summary>
        public long UserId
        {
            get
            {
                long userId = 0L;
                string userIdStr = this["visitor_id"];
                if (!string.IsNullOrEmpty(userIdStr))
                {
                    long.TryParse(userIdStr, out userId);
                }
                return userId;
            }
        }

        /// <summary>
        /// 京东用户昵称
        /// </summary>
        public string UserNick
        {
            get { return this["visitor_nick"]; }
        }

        /// <summary>
        /// 获取指定参数的值
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public string this[string name]
        {
            get
            {
                string value;
                parameters.TryGetValue(name, out value);
                return value;
            }
        }

        internal void AddParameter(string name, string value)
        {
            this.parameters.Add(name, value);
        }

        internal void AddParameters(IDictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                IEnumerator<KeyValuePair<string, string>> paramEnum = parameters.GetEnumerator();
                while (paramEnum.MoveNext())
                {
                    AddParameter(paramEnum.Current.Key, paramEnum.Current.Value);
                }
            }
        }
    }
}
