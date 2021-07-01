using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hishop.Weixin.MP.Api
{
    public class TemplateApi
    {
        public static void SendMessage(string accessTocken,TemplateMessage templateMessage)
        {
            StringBuilder json = new StringBuilder("{");
            json.AppendFormat("\"touser\":\"{0}\",", templateMessage.Touser);
            json.AppendFormat("\"template_id\":\"{0}\",", templateMessage.TemplateId);
            if (!string.IsNullOrEmpty(templateMessage.Url))
            {
                json.AppendFormat("\"url\":\"{0}\",", templateMessage.Url);
            }  //tany 20151019
            json.AppendFormat("\"topcolor\":\"{0}\",", templateMessage.Topcolor);
            json.Append("\"data\":{");
            foreach (var part in templateMessage.Data)
                json.AppendFormat("\"{0}\":{{\"value\":\"{1}\",\"color\":\"{2}\"}},", part.Name, part.Value, part.Color);
            json.Remove(json.Length - 1, 1);
            json.Append("}}");
            

            WebUtils webUtils = new WebUtils();
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token="+accessTocken;
            string response = webUtils.DoPost(url, json.ToString());   //消息体中不能有 \n  等字符，否则提示数据格式错误  tany  20151020
            //"{\"errcode\":40001,\"errmsg\":\"invalid credential, access_token is invalid or not latest hint: [AnYtgA0963age5]\"}"
        }

        public static string   SendMessageDebug (string accessTocken, TemplateMessage templateMessage)
        {
            StringBuilder json = new StringBuilder("{");
            json.AppendFormat("\"touser\":\"{0}\",", templateMessage.Touser);
            json.AppendFormat("\"template_id\":\"{0}\",", templateMessage.TemplateId);
            json.AppendFormat("\"url\":\"{0}\",", templateMessage.Url);
            json.AppendFormat("\"topcolor\":\"{0}\",", templateMessage.Topcolor);
            json.Append("\"data\":{");
            foreach (var part in templateMessage.Data)
                json.AppendFormat("\"{0}\":{{\"value\":\"{1}\",\"color\":\"{2}\"}},", part.Name, part.Value, part.Color);
            json.Remove(json.Length - 1, 1);
            json.Append("}}");


            WebUtils webUtils = new WebUtils();
            string url = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + accessTocken;
            string response = webUtils.DoPost(url, json.ToString());


            return response + "(accessTocken=" + accessTocken + ")";
        }
    }
}
