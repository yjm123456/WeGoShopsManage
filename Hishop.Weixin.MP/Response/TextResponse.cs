using System;
using System.Collections.Generic;

namespace Hishop.Weixin.MP.Response
{
    /// <summary>
    /// ��Ӧ�ı���Ϣ
    /// </summary>
    public class TextResponse : AbstractResponse
    {
        public string Content { get; set; }

        public override ResponseMsgType MsgType
        {
            get { return ResponseMsgType.Text; }
        }
    }
}
