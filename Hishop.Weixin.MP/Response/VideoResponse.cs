using System;
using System.Collections.Generic;
using Hishop.Weixin.MP.Domain;

namespace Hishop.Weixin.MP.Response
{
    /// <summary>
    /// ��Ӧ��Ƶ��Ϣ
    /// </summary>
    public class VideoResponse : AbstractResponse
    {
        public Video Video { get; set; }

        public override ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Video; }
        }
    }
}
