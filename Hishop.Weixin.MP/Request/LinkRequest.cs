using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Request
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class LinkRequest : AbstractRequest
    {
        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// ��Ϣ����
        /// </summary>
        public string Url { get; set; }
    }
}
