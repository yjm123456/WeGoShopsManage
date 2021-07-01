using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web.Script.Serialization;
using Hishop.Weixin.MP.Domain;
using System.Web;
namespace Hishop.Weixin.MP.Api
{
    public class TokenApi
    {
        public string AppId
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppId");
            }
        }

        public string AppSecret
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("AppSecret");
            }
        }
        /// <summary>
        /// 获取token，如果出错，则获取的是token的json格式
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GetToken_Message(string appid, string secret)
        {
            
            string response = GetToken(appid, secret);// new Util.WebUtils().DoGet(url, null);
            if (response.Contains("access_token"))
                response = new JavaScriptSerializer().Deserialize<Token>(response).access_token;
            return response;

        }

        static object lockobj = new object();

        /// <summary>
        /// 获取token的json格式数据，使用一个小时的缓存
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string GetToken(string appid, string secret)
        {
            const string regionCacheKey = "weixinToken";
            string token = string.Empty;
           
            int seconds = 600;//10分钟缓存

            token = System.Web.HttpRuntime.Cache.Get(regionCacheKey) as string;
            if (string.IsNullOrEmpty(token))
            {
                lock (lockobj)
                {
                    if (string.IsNullOrEmpty(token))
                    {
                        string url = String.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret);
                        token = new Util.WebUtils().DoGet(url, null);
                        HttpRuntime.Cache.Insert(regionCacheKey, token, null, DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration);
                    }
                }
            }
            return token;

            //string tokenAndTime = HiCache.Get(regionCacheKey) as string;
            //if (string.IsNullOrEmpty(tokenAndTime))
            //{
               
            //    tokenAndExpireTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + token;
            //    HiCache.Insert(regionCacheKey, tokenAndExpireTime, seconds);

            //}
            //else
            //{
            //    tokenAndExpireTime = tokenAndTime;
            //}
            ////解析时间是否超过1小时

            //DateTime dtExpire = DateTime.Parse(tokenAndExpireTime.Substring(0, 19));
            //if (dtExpire.AddSeconds(seconds) < DateTime.Now)
            //{
            //    token = Hishop.Weixin.MP.Api.TokenApi.GetToken(appid, secret);
            //    tokenAndExpireTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "|" + token;
            //    HiCache.Remove(regionCacheKey);
            //    HiCache.Insert(regionCacheKey, tokenAndExpireTime, seconds);
            //}
            //else
            //{
            //    token = tokenAndExpireTime.Substring(20, tokenAndExpireTime.Length - 20);
            //}

            //Dictionary<string, string> dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(token);
            //if (dict != null && dict.ContainsKey("access_token"))
            //    return dict["access_token"];
            //if (dict != null && dict.ContainsKey("errcode") && dict.ContainsKey("errmsg"))
            //    _GetTokenError = dict["errcode"] + "|" + dict["errmsg"];
            //else
            //    _GetTokenError = "";

            //return String.Empty;


        }


        /// <summary>
        /// 检查TOKEN是否正确返回
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
       public static bool CheckIsRightToken(string Token)
        {  
           bool rs=true;
           if (Token.Contains("errcode") || Token.Contains("errmsg"))
               rs = false;
           return rs;
        }
    }
}
