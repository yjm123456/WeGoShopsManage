using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Jayrock.Json.Conversion;
using Jd.Api.Parser;

namespace Jd.Api.Util
{
    /// <summary>
    /// Jd系统工具类。
    /// </summary>
    public abstract class JdUtils
    {
        public const string Jd_AUTH_URL = "http://container.open.jd.com/container?authcode=";

        /// <summary>
        /// 给Jd请求签名。
        /// </summary>
        /// <param name="parameters">所有字符型的Jd请求参数</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        public static string SignJdRequest(IDictionary<string, string> parameters, string secret)
        {
            return SignJdRequest(parameters, secret, false);
        }

        /// <summary>
        /// 给Jd请求签名。
        /// </summary>
        /// <param name="parameters">所有字符型的Jd请求参数</param>
        /// <param name="secret">签名密钥</param>
        /// <param name="qhs">是否前后都加密钥进行签名</param>
        /// <returns>签名</returns>
        public static string SignJdRequest(IDictionary<string, string> parameters, string secret, bool qhs)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder(secret);
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    query.Append(key).Append(value);
                }
            }
            if (qhs)
            {
                query.Append(secret);
            }
            // 第三步：使用MD5加密
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));

            // 第四步：把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        /// <summary>
        /// 验证回调地址的签名是否合法。
        /// </summary>
        /// <param name="callbackUrl">回调地址</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns>验证成功返回True，否则返回False</returns>
        public static bool VerifyJdResponse(string callbackUrl, string appSecret)
        {
            Uri uri = new Uri(callbackUrl);

            string query = uri.Query;
            if (string.IsNullOrEmpty(query)) // 没有回调参数
            {
                return false;
            }

            query = query.Trim(new char[] { '?', ' ' });
            if (query.Length == 0) // 没有回调参数
            {
                return false;
            }

            IDictionary<string, string> queryDict = SplitUrlQuery(query);
            string JdParams;
            queryDict.TryGetValue("Jd_parameters", out JdParams);
            string JdSession;
            queryDict.TryGetValue("Jd_session", out JdSession);
            string JdSign;
            queryDict.TryGetValue("Jd_sign", out JdSign);
            string appKey;
            queryDict.TryGetValue("Jd_appkey", out appKey);

            JdSign = (JdSign == null ? null : Uri.UnescapeDataString(JdSign));
            return VerifyJdResponse(JdParams, JdSession, JdSign, appKey, appSecret);
        }

        /// <summary>
        /// 验证回调地址的签名是否合法。要求所有参数均为已URL反编码的。
        /// </summary>
        /// <param name="JdParams">Jd私有参数（未经BASE64解密后的）</param>
        /// <param name="JdSession">Jd私有会话码</param>
        /// <param name="JdSign">Jd回调签名（经过URL反编码的）</param>
        /// <param name="appKey">应用公钥</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns>验证成功返回True，否则返回False</returns>
        public static bool VerifyJdResponse(string JdParams, string JdSession, string JdSign, string appKey, string appSecret)
        {
            StringBuilder result = new StringBuilder();
            result.Append(appKey).Append(JdParams).Append(JdSession).Append(appSecret);
            byte[] bytes = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(result.ToString()));
            return Convert.ToBase64String(bytes) == JdSign;
        }

        /// <summary>
        /// 获取Jd容器回调上下文，主要用于客户端应用。
        /// </summary>
        /// <param name="authCode">授权码</param>
        /// <returns>Jd容器上下文</returns>
        public static JdContext GetJdContext(string authCode)
        {
            string url = Jd_AUTH_URL + authCode;
            WebUtils wu = new WebUtils();
            string rsp = wu.DoGet(url, null);
            if (string.IsNullOrEmpty(rsp))
            {
                return null;
            }

            JdContext context = new JdContext();
            IEnumerator<KeyValuePair<string, string>> paramEnum = SplitUrlQuery(rsp).GetEnumerator();
            while (paramEnum.MoveNext())
            {
                if ("Jd_parameters".Equals(paramEnum.Current.Key))
                {
                    context.AddParameters(DecodeJdParams(paramEnum.Current.Value));
                }
                else
                {
                    context.AddParameter(paramEnum.Current.Key, paramEnum.Current.Value);
                }
            }

            return context;
        }

        /// <summary>
        /// 解释Jd回调参数为键值对（采用GBK字符集编码）。
        /// </summary>
        /// <param name="JdParams">经过BASE64和URL编码的字符串</param>
        /// <returns>键值对</returns>
        public static IDictionary<string, string> DecodeJdParams(string JdParams)
        {
            return DecodeJdParams(JdParams, Encoding.GetEncoding("GBK"));
        }

        /// <summary>
        /// 解释Jd回调参数为键值对。
        /// </summary>
        /// <param name="JdParams">经过BASE64和URL编码的字符串</param>
        /// <param name="encoding">字符集编码</param>
        /// <returns>键值对</returns>
        public static IDictionary<string, string> DecodeJdParams(string JdParams, Encoding encoding)
        {
            if (string.IsNullOrEmpty(JdParams))
            {
                return null;
            }
            byte[] buffer = Convert.FromBase64String(Uri.UnescapeDataString(JdParams));
            string originJdParams = encoding.GetString(buffer);
            return SplitUrlQuery(originJdParams);
        }

        private static IDictionary<string, string> SplitUrlQuery(string query)
        {
            IDictionary<string, string> result = new Dictionary<string, string>();

            string[] pairs = query.Split(new char[] { '&' });
            if (pairs != null && pairs.Length > 0)
            {
                foreach (string pair in pairs)
                {
                    string[] oneParam = pair.Split(new char[] { '=' }, 2);
                    if (oneParam != null && oneParam.Length == 2)
                    {
                        result.Add(oneParam[0], oneParam[1]);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 清除字典中值为空的项。
        /// </summary>
        /// <param name="dict">待清除的字典</param>
        /// <returns>清除后的字典</returns>
        public static IDictionary<string, T> CleanupDictionary<T>(IDictionary<string, T> dict)
        {
            IDictionary<string, T> newDict = new Dictionary<string, T>(dict.Count);
            IEnumerator<KeyValuePair<string, T>> dem = dict.GetEnumerator();

            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                T value = dem.Current.Value;
                if (value != null)
                {
                    newDict.Add(name, value);
                }
            }

            return newDict;
        }

        /// <summary>
        /// 获取文件的真实后缀名。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>JPG, GIF, PNG or null</returns>
        public static string GetFileSuffix(byte[] fileData)
        {
            if (fileData == null || fileData.Length < 10)
            {
                return null;
            }

            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F')
            {
                return "GIF";
            }
            else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return "PNG";
            }
            else if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F')
            {
                return "JPG";
            }
            else if (fileData[0] == 'B' && fileData[1] == 'M')
            {
                return "BMP";
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件的真实媒体类型。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(byte[] fileData)
        {
            string suffix = GetFileSuffix(fileData);
            string mimeType;
            
            switch (suffix)
            {
                case "JPG": mimeType = "image/jpeg"; break;
                case "GIF": mimeType = "image/gif"; break;
                case "PNG": mimeType = "image/png"; break;
                case "BMP": mimeType = "image/bmp"; break;
                default: mimeType = "application/octet-stream"; break;
            }

            return mimeType;
        }
        /// <summary>
        /// 根据文件后缀名获取文件的媒体类型。
        /// </summary>
        /// <param name="fileName">带后缀的文件名或文件全名</param>
        /// <returns>媒体类型</returns>
        public static string GetMimeType(string fileName)
        {
            string mimeType;
            fileName = fileName.ToLower();

            if (fileName.EndsWith(".bmp", StringComparison.CurrentCulture))
            {
                mimeType = "image/bmp";
            }
            else if (fileName.EndsWith(".gif", StringComparison.CurrentCulture))
            {
                mimeType = "image/gif";
            }
            else if (fileName.EndsWith(".jpg", StringComparison.CurrentCulture) || fileName.EndsWith(".jpeg", StringComparison.CurrentCulture))
            {
                mimeType = "image/jpeg";
            }
            else if (fileName.EndsWith(".png", StringComparison.CurrentCulture))
            {
                mimeType = "image/png";
            }
            else
            {            
                mimeType = "application/octet-stream";
            }

            return mimeType;
        }

        /// <summary>
        /// 根据API名称获取响应根节点名称。
        /// </summary>
        /// <param name="api">API名称</param>
        /// <returns></returns>
        public static string GetRootElement(string api)
        {
            int pos = api.IndexOf(".");
            if (pos != -1 && api.Length > pos)
            {
                api = api.Substring(pos + 1).Replace('.', '_');
            }
            return api + "_response";
        }

        /// <summary>
        /// 把JSON解释为字典结构。
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>字典</returns>
        public static IDictionary ParseJson(string json)
        {
            return JsonConvert.Import(json) as IDictionary;
        }

        /// <summary>
        /// 把JSON解释为API响应对象。
        /// </summary>
        /// <typeparam name="T">API响应类型</typeparam>
        /// <param name="json">JSON字符串</param>
        /// <returns>API响应对象</returns>
        public static T ParseResponse<T>(string json) where T : JdResponse
        {
            JdJsonParser parser = new JdJsonParser();
            return parser.Parse<T>(json);
        }

        /// <summary>
        /// 获取从1970年1月1日到现在的毫秒总数。
        /// </summary>
        /// <returns>毫秒数</returns>
        public static long GetCurrentTimeMillis()
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
