using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Hishop.Weixin.MP.Util
{
	public sealed class WebUtils
	{
		public string DoPost(string url, IDictionary<string, string> parameters)
		{
			HttpWebRequest webRequest = this.GetWebRequest(url, "POST");
			webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
			byte[] bytes = Encoding.UTF8.GetBytes(WebUtils.BuildQuery(parameters));
			Stream requestStream = webRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
			return this.GetResponseAsString(rsp, Encoding.UTF8);
		}

		public string DoPost(string url, string value)
		{
			return this.HttpSend(url, value);
		}

		public string HttpSend(string url, string value)
		{
			string result;
			try
			{
				string method = "GET";
				if (!string.IsNullOrEmpty(value))
				{
					method = "POST";
				}
				HttpWebRequest webRequest = this.GetWebRequest(url, method);
				webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
				if (!string.IsNullOrEmpty(value))
				{
					byte[] bytes = Encoding.UTF8.GetBytes(value);
					Stream requestStream = webRequest.GetRequestStream();
					requestStream.Write(bytes, 0, bytes.Length);
					requestStream.Close();
				}
				HttpWebResponse rsp = (HttpWebResponse)webRequest.GetResponse();
				result = this.GetResponseAsString(rsp, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			return result;
		}

		public string DoGet(string url, IDictionary<string, string> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				if (url.Contains("?"))
				{
					url = url + "&" + WebUtils.BuildQuery(parameters);
				}
				else
				{
					url = url + "?" + WebUtils.BuildQuery(parameters);
				}
			}
			return this.HttpSend(url, null);
		}

		public HttpWebRequest GetWebRequest(string url, string method)
		{
			HttpWebRequest httpWebRequest;
			if (url.Contains("https"))
			{
				ServicePointManager.ServerCertificateValidationCallback = ((object s, X509Certificate ce, X509Chain ch, SslPolicyErrors er) => true);
				httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
			}
			else
			{
				httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			}
			httpWebRequest.ServicePoint.Expect100Continue = false;
			httpWebRequest.Method = method;
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Timeout = 20000;
			httpWebRequest.UserAgent = "Hishop";
			return httpWebRequest;
		}

		public string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
		{
			Stream stream = null;
			StreamReader streamReader = null;
			string result;
			try
			{
				stream = rsp.GetResponseStream();
				streamReader = new StreamReader(stream, encoding);
				result = streamReader.ReadToEnd();
			}
			finally
			{
				if (streamReader != null)
				{
					streamReader.Close();
				}
				if (stream != null)
				{
					stream.Close();
				}
				if (rsp != null)
				{
					rsp.Close();
				}
			}
			return result;
		}

		public string BuildGetUrl(string url, IDictionary<string, string> parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				if (url.Contains("?"))
				{
					url = url + "&" + WebUtils.BuildQuery(parameters);
				}
				else
				{
					url = url + "?" + WebUtils.BuildQuery(parameters);
				}
			}
			return url;
		}

		public static string BuildQuery(IDictionary<string, string> parameters)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			IEnumerator<KeyValuePair<string, string>> enumerator = parameters.GetEnumerator();
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, string> current = enumerator.Current;
				string key = current.Key;
				current = enumerator.Current;
				string value = current.Value;
				if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
				{
					if (flag)
					{
						stringBuilder.Append("&");
					}
					stringBuilder.Append(key);
					stringBuilder.Append("=");
					stringBuilder.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
					flag = true;
				}
			}
			return stringBuilder.ToString();
		}
	}
}
