using System;
using System.Collections.Generic;
using Hishop.Weixin.MP.Domain;

namespace Hishop.Weixin.MP.Response
{
    /// <summary>
    /// ��Ӧͼ����Ϣ
    /// </summary>
    public class NewsResponse : AbstractResponse
    {
        public int ArticleCount
        {
            get {
                return Articles == null ? 0 : Articles.Count;
            }
        }

        public IList<Article> Articles { get; set; }

        public override ResponseMsgType MsgType
        {
            get { return ResponseMsgType.News; }
        }
    }
}
