using Hishop.Weixin.MP.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hishop.Weixin.MP.Api
{

    /// <summary>
    /// 处理二维码 tany  2015-9-25
    /// </summary>
    public class BarCodeApi
    {
        /// <summary>  
        /// 创建二维码ticket  
        /// </summary>  
        /// <returns></returns>  
        public static string CreateTicket(string TOKEN, string scene_id = "12399", string QRType = "QR_LIMIT_SCENE", string exSecond = "2592000")
        {

            string result = "";
            //string strJson = @"{""expire_seconds"":1800, ""action_name"": ""QR_SCENE"", ""action_info"": {""scene"": {""scene_id"":100000023}}}";  临时
            //string strJson = @"{""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"":100000024}}}"; 永久
            string strJson = @"{""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"":" + scene_id + "}}}";

            if(QRType=="QR_SCENE") //临时二维码获取,最长30天
                strJson = @"{""expire_seconds"":"+ exSecond + @", ""action_name"": ""QR_SCENE"", ""action_info"": {""scene"": {""scene_id"":" + scene_id + "}}}";


            string wxurl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + TOKEN;
            string RetJson= new WebUtils().DoPost(wxurl, strJson);


            //正常情况下返回结果：
            //{"ticket":"gQHF8DoAAAAAAAAAASxodHRwOi8vd2VpeGluLnFxLmNvbS9xL1FFTzd5X25tMDlxcUxHTS0wR19OAAIEFgwFVgMEAAAAAA==","url":"http:\/\/weixin.qq.com\/q\/QEO7y_nm09qqLGM-0G_N"}
          
            //匿名对象解析
            var tempEntity = new { ticket = "", url = "" };
            string json5 = Newtonsoft.Json.JsonConvert.SerializeObject(tempEntity);
            //json5 : {"ID":0,"Name":""}
            //tempEntity = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType("{\"ID\":\"112\",\"Name\":\"石子儿\"}", tempEntity);
            tempEntity = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(RetJson , tempEntity);
            
            return tempEntity.ticket;

            //return RetJson;

           
        }


        /// <summary>
        /// 生成带WIFI信息的Ticket
        /// </summary>
        /// <param name="token"></param>
        /// <param name="wifiInfo"></param>
        /// <returns></returns>
        public static string CreateTicketWifi(string token, string wifiInfo)
        {
            string result = "";
            string strJson = @"{""action_name"": ""QR_LIMIT_SCENE"", ""action_info"": {""scene"": {""scene_id"":" + wifiInfo + "}}}";

            string wxurl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token=" + token;
            string RetJson = new WebUtils().DoPost(wxurl, strJson);

            var tempEntity = new { ticket = "", url = "" };
            string json5 = Newtonsoft.Json.JsonConvert.SerializeObject(tempEntity);
            tempEntity = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(RetJson, tempEntity);

            return tempEntity.ticket;
        }


        public static string GetQRImageUrlByTicket(string TICKET)
        {
            string content = string.Empty;
            string strpath = string.Empty;
            string savepath = string.Empty;

            string stUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + System.Web.HttpUtility.UrlEncode(TICKET, Encoding.UTF8);

            return stUrl;
 
        }

        /// <summary>
        /// 获取粉丝的头像和昵称信息
        /// </summary>
        /// <param name="TOKEN"></param>
        /// <param name="OpenID"></param>
        /// <param name="RetInfo"></param>
        /// <param name="NickName"></param>
        /// <param name="HeadImageUrl"></param>
        /// <returns></returns>
        public static bool GetHeadImageUrlByOpenID(string TOKEN, string OpenID, out string RetInfo, out string NickName, out string HeadImageUrl)
        {
            NickName = "";
            HeadImageUrl = "";
            RetInfo = "";
            if (string.IsNullOrEmpty(OpenID))
            {
                RetInfo = "{\"errcode\":40013,\"errmsg\":\"openId为空\"}";

                return false;
            }
           

            string result = "";
            string wxurl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + TOKEN + "&openid=" + OpenID + "&lang=zh_CN";
                            //https://api.weixin.qq.com/cgi-bin/user/info?access_token=ACCESS_TOKEN&openid=OPENID&lang=zh_CN

            
            //RetInfo = RetInfo +  "***wxurl=" + wxurl + "***";


            string RetJson = (new WebUtils()).DoGet(wxurl, null);
            if (RetJson.Contains("errcode"))
            {
                return false;
            }
             
            //匿名对象解析
            var tempEntity = new { subscribe = "", nickname = "", headimgurl="" };
            string json5 = Newtonsoft.Json.JsonConvert.SerializeObject(tempEntity);
            tempEntity = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(RetJson, tempEntity);

            NickName = tempEntity.nickname;
            HeadImageUrl = tempEntity.headimgurl;

            //RetInfo = RetInfo +  "OK.Json原文：" + RetJson + "***";
            if (tempEntity.subscribe.Trim() != "1")
            {
                RetInfo = "此用户未关注当前公众号，无法拉取信息。";
            }
            return tempEntity.subscribe.Trim() == "1";
 

 
        }


        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        /// <param name="TOKEN"></param>
        /// <param name="OpenID"></param>
        /// <returns></returns>
        public static string GetUserInfosByOpenID(string TOKEN, string OpenID)
        {

            string rs = "";

            if (string.IsNullOrEmpty(OpenID))
            {
                rs = "OpenID为空。";
            }
            else
            {

                //{
                //    "subscribe": 1, 
                //    "openid": "o6_bmjrPTlm6_2sgVt7hMZOPfL2M", 
                //    "nickname": "Band", 
                //    "sex": 1, 
                //    "language": "zh_CN", 
                //    "city": "广州", 
                //    "province": "广东", 
                //    "country": "中国", 
                //    "headimgurl":    "http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/0", 
                //   "subscribe_time": 1382694957,
                //   "unionid": " o6_bmasdasdsad6_2sgVt7hMZOPfL"
                //   "remark": "",
                //   "groupid": 0
                //}

                string wxurl = "https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + TOKEN + "&openid=" + OpenID + "&lang=zh_CN";
                rs = new WebUtils().DoGet(wxurl, null);
            }

            return rs;
        }  







    }
}
