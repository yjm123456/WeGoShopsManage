using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Request
{
    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class VoiceRequest : AbstractRequest
    {
        /// <summary>
        /// ������Ϣý��id
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// ������ʽ
        /// </summary>
        public string Format { get; set; }
    }
}
