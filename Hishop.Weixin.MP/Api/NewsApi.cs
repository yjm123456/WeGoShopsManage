using System;
using System.Collections.Generic;
using System.Linq;
using Hishop.Weixin.MP.Util;
using System.Text;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;


namespace Hishop.Weixin.MP.Api
{
    public class NewsApi
    {
        /// <summary> 
        /// 客服接口发送消息 
        /// </summary> 
        public static string KFSend(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", access_token), postData);
        }

        /// <summary> 
        /// 根据OpenID列表群发 
        /// </summary> 
        public static string Send(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}", access_token), postData);
        }

        /// <summary> 
        /// 给所有人发送 
        /// </summary> 
        public static string SendAll(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}", access_token), postData);
        }
        /// <summary> 
        /// 获取关注者OpenID集合 
        /// </summary> 
        public static List<string> GetOpenIDs(string access_token)
        {
            List<string> result = new List<string>();

            List<string> openidList = GetOpenIDs(access_token, null);
            result.AddRange(openidList);

            while (openidList.Count > 0)
            {
                openidList = GetOpenIDs(access_token, openidList[openidList.Count - 1]);
                result.AddRange(openidList);
            }

            return result;
        }
        /// <summary> 
        /// 图文消息json 
        /// </summary> 
        public static string CreateImageNewsJson(string media_id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"filter\":{\"is_to_all\":true},");
            sb.Append("\"msgtype\":\"mpnews\",");
            sb.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary> 
        /// 客服发送文本消息json 
        /// </summary> 
        public static string CreateKFTxtNewsJson(string openid, string content)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"touser\":\"" + openid + "\",\"msgtype\":\"text\",\"text\": {\"content\":\"" + content + "\"}}");
            return sb.ToString();
        }
        /// <summary> 
        /// 客服发送图文消息json 
        /// </summary> 
        public static string CreateKFImageNewsJson(string openid, string media_id)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"touser\":\"" + openid + "\",\"msgtype\":\"image\",\"image\": {\"media_id\":\"" + media_id + "\"}}");
            return sb.ToString();
        }
        /// <summary> 
        /// 图文消息json 
        /// </summary> 
        public static string CreateImageNewsJson(string media_id, List<string> openidList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"touser\":[");
            sb.Append(string.Join(",", openidList.ConvertAll<string>(a => "\"" + a + "\"").ToArray()));
            sb.Append("],");
            sb.Append("\"msgtype\":\"mpnews\",");
            sb.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary> 
        /// 文本消息json 
        /// </summary> 
        public static string CreateTxtNewsJson(string media_id)//, List<string> openidList
        {
            string temp = "内容测试";
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"filter\":{\"is_to_all\":true},");
            //sb.Append("{\"touser\":[");
            //sb.Append(string.Join(",", openidList.ConvertAll<string>(a => "\"" + a + "\"").ToArray()));
            //sb.Append("],");
            sb.Append("\"text\":{\"content\":\"" + temp + "\"},");
            sb.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
            sb.Append("}");
            return sb.ToString();
        }
        /// <summary> 
        /// 获取关注者OpenID集合 
        /// </summary> 
        public static List<string> GetOpenIDs(string access_token, string next_openid)
        {
            // 设置参数 
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", access_token, string.IsNullOrEmpty(next_openid) ? "" : next_openid);
            string returnStr = new WebUtils().DoPost(url, "");
            int count = int.Parse(GetJsonValue(returnStr, "count"));
            if (count > 0)
            {
                string startFlg = "\"openid\":[";
                int start = returnStr.IndexOf(startFlg) + startFlg.Length;
                int end = returnStr.IndexOf("]", start);
                string openids = returnStr.Substring(start, end - start).Replace("\"", "");
                return openids.Split(',').ToList<string>();
            }
            else
            {
                return new List<string>();
            }
        }
        /// <summary> 
        /// Http上传文件 
        /// </summary> 
        public static string HttpUploadFile(string url, string path)
        {
            // 设置参数 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线 
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息  
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(GetFilePath(path), FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据 
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求 
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码 
            string content = sr.ReadToEnd();
            return content;

        }
        /// <summary>
        /// 避免在异步处理中，不支持MapPath的路径方式的方法
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFilePath(string path)
        {
            var rsPath = HttpRuntime.AppDomainAppPath.ToString();
            if (path.ToLower().StartsWith("http"))
            {
                return path;
            }
            else
            {
                if (path.ToLower().StartsWith("/") || path.ToLower().StartsWith("\\"))
                {
                    rsPath += path;
                }
                else
                {
                    rsPath += "\\" + path;
                }
            }

            return rsPath;
        }
        /// <summary> 
        /// 上传图文消息素材返回media_id 
        /// </summary> 
        public static string UploadNews(string access_token, string postData)
        {
            return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}", access_token), postData);
        }

        /// <summary> 
        /// 上传媒体返回媒体ID 
        /// </summary> 
        public static string UploadMedia(string access_token, string type, string path)
        {
            // 设置参数 
            string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, type);
            return HttpUploadFile(url, path);
        }
        public static string GetJsonValue(string msg, string Field)
        {
            string result = "";

            try
            {
                JObject jo = JObject.Parse(msg);
                if (jo[Field] != null)
                {
                    result = jo[Field].ToString();
                }
            }
            catch (Exception ex)
            {
                Debuglog(msg, "_debuglogtext.txt");
            }
            return result;
        }
        private static object LockLog = new object(); //日志排他锁
        /// <summary>
        /// 调式日志，用于调式日志输出
        /// </summary>
        /// <param name="log"></param>
        public static void Debuglog(string log, string logname = "_Debuglog.txt")
        {
            lock (LockLog) //防止并发异常
            {
                try
                {
                    string LogName = DateTime.Now.ToString("yyyyMMdd") + logname; //按天日志
                    string logfile = HttpRuntime.AppDomainAppPath.ToString() + "App_Data/" + LogName;
                    System.IO.StreamWriter sw = System.IO.File.AppendText(logfile);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
                    sw.WriteLine("---------------");
                    sw.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }
        /// <summary>
        /// 根据图片路径，获取微信media_id
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetMedia_IDByPath(string access_token, string path)
        {
            string result = string.Empty;
            string tempPath = path;
            if (tempPath.StartsWith("http"))
            {
               tempPath= System.Text.RegularExpressions.Regex.Replace(tempPath, "(http|https)://([^/]*)", "");
            }
            if (tempPath.StartsWith("/"))
            {
                try
                {
                    result = UploadMedia(access_token, "image", tempPath); // 上图片返回媒体ID 
                }
                catch (Exception ex)
                {
                    result = ex.ToString();
                }
            }
            else
            {
                result = "图片路径不对";
            }
            return result;
        }

        /// <summary> 
        /// 拼接图文消息素材Json字符串 
        /// </summary> 6
        public static string GetArticlesJsonStr(string access_token, DataTable dt)
        {
            StringBuilder sbArticlesJson = new StringBuilder();

            sbArticlesJson.Append("{\"articles\":[");
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string path = dr["ImgUrl"].ToString();
                if (!File.Exists(path))
                {
                    return "{\"code\":0,\"msg\":\"要发送的图片不存在\"}";
                }
                string msg = UploadMedia(access_token, "image", path); // 上图片返回媒体ID 
                string media_id = GetJsonValue(msg, "media_id");//先是获取本地的图片地址，然后上传到微信之后取和media_id
                if (!String.IsNullOrEmpty(media_id))
                {

                    sbArticlesJson.Append("{");
                    sbArticlesJson.Append("\"thumb_media_id\":\"" + media_id + "\",");
                    sbArticlesJson.Append("\"author\":\"" + dr["Author"].ToString() + "\",");
                    sbArticlesJson.Append("\"title\":\"" + dr["Title"].ToString() + "\",");
                    sbArticlesJson.Append("\"content_source_url\":\"" + dr["TextUrl"].ToString() + "\",");
                    sbArticlesJson.Append("\"content\":\"" + dr["Content"].ToString() + "\",");
                    sbArticlesJson.Append("\"digest\":\"" + dr["Content"].ToString() + "\",");
                    if (i == dt.Rows.Count - 1)
                    {
                        sbArticlesJson.Append("\"show_cover_pic\":\"1\"}");
                    }
                    else
                    {
                        sbArticlesJson.Append("\"show_cover_pic\":\"1\"},");
                    }
                }
                i++;
            }
            sbArticlesJson.Append("]}");

            return sbArticlesJson.ToString();
        }

        public static string GetErrorCodeMsg(string errcode)
        {
            string result = string.Empty;
            switch (errcode)
            {
                case "45028": result = "发送条数已经用完"; break;
                case "-1": result = "系统繁忙，此时请开发者稍候再试"; break;
                case "0": result = "请求成功"; break;
                case "40001": result = "获取access_token时AppSecret错误，或者access_token无效。请开发者认真比对AppSecret的正确性，或查看是否正在为恰当的公众号调用接口"; break;
                case "40002": result = "不合法的凭证类型"; break;
                case "40003": result = "不合法的OpenID，请开发者确认OpenID（该用户）是否已关注公众号，或是否是其他公众号的OpenID"; break;
                case "40004": result = "不合法的媒体文件类型"; break;
                case "40005": result = "不合法的文件类型"; break;
                case "40006": result = "不合法的文件大小"; break;
                case "40007": result = "不合法的媒体文件id"; break;
                case "40008": result = "不合法的消息类型"; break;
                case "40009": result = "不合法的图片文件大小"; break;
                case "40010": result = "不合法的语音文件大小"; break;
                case "40011": result = "不合法的视频文件大小"; break;
                case "40012": result = "不合法的缩略图文件大小"; break;
                case "40013": result = "不合法的AppID，请开发者检查AppID的正确性，避免异常字符，注意大小写"; break;
                case "40014": result = "不合法的access_token，请开发者认真比对access_token的有效性（如是否过期），或查看是否正在为恰当的公众号调用接口"; break;
                case "40015": result = "不合法的菜单类型"; break;
                case "40016": result = "不合法的按钮个数"; break;
                case "40017": result = "不合法的按钮个数"; break;
                case "40018": result = "不合法的按钮名字长度"; break;
                case "40019": result = "不合法的按钮KEY长度"; break;
                case "40020": result = "不合法的按钮URL长度"; break;
                case "40021": result = "不合法的菜单版本号"; break;
                case "40022": result = "不合法的子菜单级数"; break;
                case "40023": result = "不合法的子菜单按钮个数"; break;
                case "40024": result = "不合法的子菜单按钮类型"; break;
                case "40025": result = "不合法的子菜单按钮名字长度"; break;
                case "40026": result = "不合法的子菜单按钮KEY长度"; break;
                case "40027": result = "不合法的子菜单按钮URL长度"; break;
                case "40028": result = "不合法的自定义菜单使用用户"; break;
                case "40029": result = "不合法的oauth_code"; break;
                case "40030": result = "不合法的refresh_token"; break;
                case "40031": result = "不合法的openid列表"; break;
                case "40032": result = "不合法的openid列表长度"; break;
                case "40033": result = "不合法的请求字符，不能包含\\uxxxx格式的字符"; break;
                case "40035": result = "不合法的参数"; break;
                case "40038": result = "不合法的请求格式"; break;
                case "40039": result = "不合法的URL长度"; break;
                case "40050": result = "不合法的分组id"; break;
                case "40051": result = "分组名字不合法"; break;
                case "40117": result = "分组名字不合法"; break;
                case "40118": result = "media_id大小不合法"; break;
                case "40119": result = "button类型错误"; break;
                case "40120": result = "button类型错误"; break;
                case "40121": result = "不合法的media_id类型"; break;
                case "40132": result = "微信号不合法"; break;
                case "40137": result = "不支持的图片格式"; break;
                case "41001": result = "缺少access_token参数"; break;
                case "41002": result = "缺少appid参数"; break;
                case "41003": result = "缺少refresh_token参数"; break;
                case "41004": result = "缺少secret参数"; break;
                case "41005": result = "缺少多媒体文件数据"; break;
                case "41006": result = "缺少media_id参数"; break;
                case "41007": result = "缺少子菜单数据"; break;
                case "41008": result = "缺少oauth code"; break;
                case "41009": result = "缺少openid"; break;
                case "42001": result = "access_token超时，请检查access_token的有效期，请参考基础支持-获取access_token中，对access_token的详细机制说明"; break;
                case "42002": result = "refresh_token超时"; break;
                case "42003": result = "oauth_code超时"; break;
                case "43001": result = "需要GET请求"; break;
                case "43002": result = "需要POST请求"; break;
                case "43003": result = "需要HTTPS请求"; break;
                case "43004": result = "需要接收者关注"; break;
                case "43005": result = "需要好友关系"; break;
                case "44001": result = "多媒体文件为空"; break;
                case "44002": result = "POST的数据包为空"; break;
                case "44003": result = "图文消息内容为空"; break;
                case "44004": result = "文本消息内容为空"; break;
                case "45001": result = "多媒体文件大小超过限制"; break;
                case "45002": result = "消息内容超过限制"; break;
                case "45003": result = "标题字段超过限制"; break;
                case "45004": result = "描述字段超过限制"; break;
                case "45005": result = "链接字段超过限制"; break;
                case "45006": result = "图片链接字段超过限制"; break;
                case "45007": result = "语音播放时间超过限制"; break;
                case "45008": result = "图文消息超过限制"; break;
                case "45009": result = "接口调用超过限制"; break;
                case "45010": result = "创建菜单个数超过限制"; break;
                case "45015": result = "回复时间超过限制"; break;
                case "45016": result = "系统分组，不允许修改"; break;
                case "45017": result = "分组名字过长"; break;
                case "45018": result = "分组数量超过上限"; break;
                case "46001": result = "不存在媒体数据"; break;
                case "46002": result = "不存在的菜单版本"; break;
                case "46003": result = "不存在的菜单数据"; break;
                case "46004": result = "不存在的用户"; break;
                case "47001": result = "解析JSON/XML内容错误"; break;
                case "48001": result = "api功能未授权，请确认公众号已获得该接口，可以在公众平台官网-开发者中心页中查看接口权限"; break;
                case "50001": result = "用户未授权该api"; break;
                case "50002": result = "用户受限，可能是违规后接口被封禁"; break;
                case "61451": result = "参数错误(invalid parameter)"; break;
                case "61452": result = "无效客服账号(invalid kf_account)"; break;
                case "61453": result = "客服帐号已存在(kf_account exsited)"; break;
                case "61454": result = "客服帐号名长度超过限制(仅允许10个英文字符，不包括@及@后的公众号的微信号)(invalid kf_acount length)"; break;
                case "61455": result = "客服帐号名包含非法字符(仅允许英文+数字)(illegal character in kf_account)"; break;
                case "61456": result = "客服帐号个数超过限制(10个客服账号)(kf_account count exceeded)"; break;
                case "61457": result = "无效头像文件类型(invalid file type)"; break;
                case "61450": result = "系统错误(system error)"; break;
                case "61500": result = "日期格式错误"; break;
                case "61501": result = "日期范围错误"; break;
                case "9001001": result = "POST数据参数不合法"; break;
                case "9001002": result = "远端服务不可用"; break;
                case "9001003": result = "Ticket不合法"; break;
                case "9001004": result = "获取摇周边用户信息失败"; break;
                case "9001005": result = "获取商户信息失败"; break;
                case "9001006": result = "获取OpenID失败"; break;
                case "9001007": result = "上传文件缺失"; break;
                case "9001008": result = "上传素材的文件类型不合法"; break;
                case "9001009": result = "上传素材的文件尺寸不合法"; break;
                case "9001010": result = "上传失败"; break;
                case "9001020": result = "帐号不合法"; break;
                case "9001021": result = "已有设备激活率低于50%，不能新增设备"; break;
                case "9001022": result = "设备申请数不合法，必须为大于0的数字"; break;
                case "9001023": result = "已存在审核中的设备ID申请"; break;
                case "9001024": result = "一次查询设备ID数量不能超过50"; break;
                case "9001025": result = "设备ID不合法"; break;
                case "9001026": result = "页面ID不合法"; break;
                case "9001027": result = "页面参数不合法"; break;
                case "9001028": result = "一次删除页面ID数量不能超过10"; break;
                case "9001029": result = "页面已应用在设备中，请先解除应用关系再删除"; break;
                case "9001030": result = "一次查询页面ID数量不能超过50"; break;
                case "9001031": result = "时间区间不合法"; break;
                case "9001032": result = "保存设备与页面的绑定关系参数错误"; break;
                case "9001033": result = "门店ID不合法"; break;
                case "9001034": result = "设备备注信息过长"; break;
                case "9001035": result = "设备申请参数不合法"; break;
                case "9001036": result = "查询起始值begin不合法 "; break;
                default:

                    //System.IO.StreamWriter sw3 = System.IO.File.AppendText(HttpContext.Current.Server.MapPath("error.txt"));

                    //sw3.WriteLine("||++错误编码+" + errcode + "+++||||");
                    //sw3.WriteLine(DateTime.Now);
                    //sw3.Flush();
                    //sw3.Close();
                    result = "未知错误";
                    break;
            }
            return result;
        }
    }
}
