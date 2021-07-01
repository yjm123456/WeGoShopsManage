using Aop.Api.Response;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hishop.AlipayFuwu.Api.Model;
using Hishop.AlipayFuwu.Api.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class VshopTemplatedWebControl : TemplatedWebControl
	{
		protected int referralId;

		private string skinName;

		protected virtual string SkinPath
		{
			get
			{
				return Globals.ApplicationPath + "/Templates/common/" + this.skinName;
			}
		}

		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				value = value.ToLower(CultureInfo.InvariantCulture);
				if (!value.EndsWith(".html"))
				{
					return;
				}
				this.skinName = value;
			}
		}

		private bool SkinFileExists
		{
			get
			{
				return !string.IsNullOrEmpty(this.SkinName);
			}
		}

		protected VshopTemplatedWebControl()
		{
			if (HttpContext.Current.Request.Form.Keys.Count == 0)
			{
				HiAffiliation.LoadPage();
			}
			this.WeiXinLoginTry();
		}

		protected virtual void WeiXinLoginTry()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string userAgent = this.Page.Request.UserAgent;
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			string text = HttpContext.Current.Request.Url.ToString().ToLower();
			if (!text.Contains("userlogin.aspx") && !text.Contains("userlogining.aspx") && !text.Contains("register.aspx") && (currentMember == null || this.Page.Session["userid"] == null || this.Page.Session["userid"].ToString() != currentMember.UserId.ToString()) && userAgent.ToLower().Contains("micromessenger") && masterSettings.IsValidationService)
			{
				this.WeixinLoginAction(masterSettings, masterSettings.IsAutoToLogin ? 2 : 0);
			}
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
			base.RenderEndTag(writer);
		}

		public string GetOpenID(SiteSettings site, int isMustLogin)
		{
			string text = this.Page.Request.QueryString["code"];
			int num = Globals.RequestQueryNum("ReferralId");
			try
			{
				string getCurrentWXOpenId = Globals.GetCurrentWXOpenId;
				if (string.IsNullOrEmpty(getCurrentWXOpenId) && site != null)
				{
					string result;
					if (!string.IsNullOrEmpty(text))
					{
						string responseResult = this.GetResponseResult(string.Concat(new string[]
						{
							"https://api.weixin.qq.com/sns/oauth2/access_token?appid=",
							site.WeixinAppId,
							"&secret=",
							site.WeixinAppSecret,
							"&code=",
							text,
							"&grant_type=authorization_code"
						}));
						if (string.IsNullOrEmpty(responseResult) || !responseResult.Contains("access_token"))
						{
							goto IL_569;
						}
						JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
						string text2 = jObject["openid"].ToString();
						Globals.GetCurrentWXOpenId = text2;
						try
						{
							if (!this.HasReg(text2, "wx"))
							{
								string generateId = Globals.GetGenerateId();
								MemberInfo memberInfo = new MemberInfo();
								memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
								memberInfo.UserName = Globals.UrlDecode("新用户");
								memberInfo.OpenId = text2;
								memberInfo.CreateDate = DateTime.Now;
								memberInfo.SessionId = generateId;
								memberInfo.SessionEndTime = DateTime.Now.AddYears(10);
								memberInfo.UserHead = Globals.GetWebUrlStart() + "/templates/common/images/user.png";
								memberInfo.ReferralUserId = Globals.GetCurrentDistributorId();
								memberInfo.Password = HiCryptographer.Md5Encrypt("888888");
								string responseResult2 = this.GetResponseResult(string.Concat(new string[]
								{
									"https://api.weixin.qq.com/sns/userinfo?access_token=",
									jObject["access_token"].ToString(),
									"&openid=",
									jObject["openid"].ToString(),
									"&lang=zh_CN"
								}));
								JObject jObject2 = JsonConvert.DeserializeObject(responseResult2) as JObject;
								if (responseResult2.Contains("nickname"))
								{
									memberInfo.UserName = Globals.UrlDecode(jObject2["nickname"].ToString());
									memberInfo.UserHead = jObject2["headimgurl"].ToString();
									MemberProcessor.CreateMember(memberInfo);
									MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(text2, "wx");
									if (openIdMember == null)
									{
										Globals.ClearUserCookie();
										result = "/Default.aspx?ReferralId=" + num;
										return result;
									}
									MemberHelper.SetUserHeadAndUserName(text2, "", "", 2);
									this.setLogin(openIdMember.UserId);
								}
								else
								{
									if (isMustLogin != 0)
									{
										string text3 = HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "");
										text3 = Regex.Replace(text3, "&code=(.*)&state=STATE", "");
										string text4 = "snsapi_userinfo";
										string text5 = string.Concat(new string[]
										{
											"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
											site.WeixinAppId,
											"&redirect_uri=",
											Globals.UrlEncode(text3),
											"&response_type=code&scope=",
											text4,
											"&state=STATE#wechat_redirect"
										});
										result = text5;
										return result;
									}
									memberInfo.Status = Convert.ToInt32(UserStatus.Visitor);
									MemberProcessor.CreateMember(memberInfo);
								}
							}
							else
							{
								string responseResult3 = this.GetResponseResult(string.Concat(new string[]
								{
									"https://api.weixin.qq.com/sns/userinfo?access_token=",
									jObject["access_token"].ToString(),
									"&openid=",
									jObject["openid"].ToString(),
									"&lang=zh_CN"
								}));
								JObject jObject3 = JsonConvert.DeserializeObject(responseResult3) as JObject;
								if (responseResult3.Contains("nickname"))
								{
									MemberInfo openIdMember2 = MemberProcessor.GetOpenIdMember(text2, "wx");
									if (openIdMember2 == null)
									{
										Globals.ClearUserCookie();
										result = "/Default.aspx?ReferralId=" + num;
										return result;
									}
									if (openIdMember2.UserName == "新用户" || string.IsNullOrEmpty(openIdMember2.UserHead) || openIdMember2.UserHead.ToLower().Contains("/templates/common/images/user.png"))
									{
										MemberHelper.SetUserHeadAndUserName(text2, jObject3["headimgurl"].ToString(), Globals.UrlDecode(jObject3["nickname"].ToString()), 1);
									}
									else if (openIdMember2.IsAuthorizeWeiXin == 0)
									{
										MemberHelper.SetUserHeadAndUserName(text2, "", "", 2);
									}
									this.setLogin(openIdMember2.UserId);
								}
							}
							goto IL_569;
						}
						catch (ThreadAbortException ex)
						{
							Globals.Debuglog(ex.ToString(), "_DebuglogWX_021.txt");
							goto IL_569;
						}
						catch (Exception ex2)
						{
							Globals.Debuglog(ex2.ToString() + "||" + responseResult, "_DebuglogWX_01.txt");
							goto IL_569;
						}
					}
					string text6 = string.Concat(new string[]
					{
						"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
						site.WeixinAppId,
						"&redirect_uri=",
						Globals.UrlEncode(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "")),
						"&response_type=code&scope=snsapi_base&state=STATE#wechat_redirect"
					});
					result = text6;
					return result;
				}
				IL_569:;
			}
			catch (ThreadAbortException ex3)
			{
				Globals.Debuglog(ex3.ToString(), "_DebuglogWX_02.txt");
			}
			catch (Exception ex4)
			{
				Globals.Debuglog(ex4.ToString(), "_DebuglogWX_03.txt");
			}
			return "";
		}

		public void WeixinLoginAction(SiteSettings site, int isMustLogin)
		{
			try
			{
				string openID = this.GetOpenID(site, isMustLogin);
				if (!string.IsNullOrEmpty(openID))
				{
					this.Page.Response.Redirect(openID);
				}
				else
				{
					string getCurrentWXOpenId = Globals.GetCurrentWXOpenId;
					string text = Globals.RequestQueryStr("code");
					if (!string.IsNullOrEmpty(text) && text.Contains(","))
					{
						Globals.Debuglog("code问题获取值为：" + text, "_DebugCodeErr.txt");
						int num = Globals.RequestQueryNum("ReferralId");
						this.Page.Response.Redirect("/default.aspx?ReferralId=" + num);
					}
					else if (isMustLogin == 1 || isMustLogin == 2)
					{
						if (string.IsNullOrEmpty(getCurrentWXOpenId))
						{
							Globals.Debuglog("WXOpenId问题获取值空：", "_DebugCodeErr.txt");
							string text2 = HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "");
							text2 = Regex.Replace(text2, "&code=(.*)&state=STATE", "");
							string text3 = "snsapi_base";
							string url = string.Concat(new string[]
							{
								"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
								site.WeixinAppId,
								"&redirect_uri=",
								Globals.UrlEncode(text2),
								"&response_type=code&scope=",
								text3,
								"&state=STATE#wechat_redirect"
							});
							this.Page.Response.Redirect(url);
						}
						else if (string.IsNullOrEmpty(text))
						{
							Globals.Debuglog("进入1，必须登录的页面，并且无code请求" + this.Page.Request.Url.ToString(), "_debugtest.txt");
							MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(getCurrentWXOpenId, "wx");
							if (openIdMember != null && openIdMember.Status == Convert.ToInt32(UserStatus.DEL))
							{
								this.Page.Response.Redirect(Globals.ApplicationPath + "/logout.aspx");
							}
							else if (openIdMember == null)
							{
								string text4 = HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "");
								text4 = Regex.Replace(text4, "&code=(.*)&state=STATE", "");
								string text5 = "snsapi_userinfo";
								string url2 = string.Concat(new string[]
								{
									"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
									site.WeixinAppId,
									"&redirect_uri=",
									Globals.UrlEncode(text4),
									"&response_type=code&scope=",
									text5,
									"&state=STATE#wechat_redirect"
								});
								this.Page.Response.Redirect(url2);
							}
							else if (openIdMember.IsAuthorizeWeiXin == 0 || (!string.IsNullOrEmpty(openIdMember.UserHead) && openIdMember.UserHead.Contains("/templates/common/images/user.png")))
							{
								string text6 = HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "");
								text6 = Regex.Replace(text6, "&code=(.*)&state=STATE", "");
								Globals.Debuglog("进入2，检测为匿名用户，并请求授权登录，去获得code参数" + this.Page.Request.Url.ToString(), "_debugtest.txt");
								string text7 = "snsapi_userinfo";
								string url3 = string.Concat(new string[]
								{
									"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
									site.WeixinAppId,
									"&redirect_uri=",
									Globals.UrlEncode(text6),
									"&response_type=code&scope=",
									text7,
									"&state=STATE#wechat_redirect"
								});
								this.Page.Response.Redirect(url3);
							}
							else
							{
								this.setLogin(openIdMember.UserId);
							}
						}
						else if (Globals.GetCurrentMemberUserId(false) == 0 || Globals.ToNum(this.Page.Session["userid"]) == 0)
						{
							Globals.Debuglog("进入4，code请求地址，获取用户基本信息" + this.Page.Request.Url.ToString(), "_debugtest.txt");
							string responseResult = this.GetResponseResult(string.Concat(new string[]
							{
								"https://api.weixin.qq.com/sns/oauth2/access_token?appid=",
								site.WeixinAppId,
								"&secret=",
								site.WeixinAppSecret,
								"&code=",
								text,
								"&grant_type=authorization_code"
							}));
							if (responseResult.Contains("access_token"))
							{
								MemberInfo openIdMember2 = MemberProcessor.GetOpenIdMember(getCurrentWXOpenId, "wx");
								if (openIdMember2 == null)
								{
									Globals.Debuglog("进入16，检测用户信息为空，清空Cookie并跳转到首页" + this.Page.Request.Url.ToString(), "_debugtest.txt");
									Globals.ClearUserCookie();
									this.Page.Response.Redirect(Globals.ApplicationPath + "/Default.aspx");
								}
								else
								{
									Globals.Debuglog("进入5，准备获取到基本信息" + this.Page.Request.Url.ToString(), "_debugtest.txt");
									JObject jObject = JsonConvert.DeserializeObject(responseResult) as JObject;
									string responseResult2 = this.GetResponseResult(string.Concat(new string[]
									{
										"https://api.weixin.qq.com/sns/userinfo?access_token=",
										jObject["access_token"].ToString(),
										"&openid=",
										jObject["openid"].ToString(),
										"&lang=zh_CN"
									}));
									JObject jObject2 = JsonConvert.DeserializeObject(responseResult2) as JObject;
									if (responseResult2.Contains("nickname"))
									{
										Globals.Debuglog("进入16，获取到基本信息" + this.Page.Request.Url.ToString(), "_debugtest.txt");
										if (string.IsNullOrEmpty(openIdMember2.UserHead) || openIdMember2.UserHead.ToString().Contains("/templates/common/images/user.png"))
										{
											MemberHelper.SetUserHeadAndUserName(getCurrentWXOpenId, jObject2["headimgurl"].ToString(), Globals.UrlDecode(jObject2["nickname"].ToString()), 1);
										}
										else if (openIdMember2.IsAuthorizeWeiXin != 1)
										{
											MemberHelper.SetUserHeadAndUserName(getCurrentWXOpenId, "", "", 2);
										}
										this.setLogin(openIdMember2.UserId);
									}
									else if (!string.IsNullOrEmpty(openIdMember2.UserHead) && !openIdMember2.UserHead.ToString().Contains("/templates/common/images/user.png"))
									{
										MemberHelper.SetUserHeadAndUserName(getCurrentWXOpenId, "", "", 2);
										this.setLogin(openIdMember2.UserId);
									}
								}
							}
							else
							{
								string text8 = HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, "");
								text8 = Regex.Replace(text8, "&code=(.*)&state=STATE", "");
								Globals.Debuglog("获取不到access_token,说明需要授权登录，去获得code参数" + this.Page.Request.Url.ToString(), "_debugtest.txt");
								string text9 = "snsapi_userinfo";
								string url4 = string.Concat(new string[]
								{
									"https://open.weixin.qq.com/connect/oauth2/authorize?appid=",
									site.WeixinAppId,
									"&redirect_uri=",
									Globals.UrlEncode(text8),
									"&response_type=code&scope=",
									text9,
									"&state=STATE#wechat_redirect"
								});
								this.Page.Response.Redirect(url4);
							}
						}
					}
				}
			}
			catch (ThreadAbortException)
			{
			}
			catch (Exception ex)
			{
				Globals.Debuglog(ex.ToString() + this.Page.Request.Url.ToString() + "openid:" + Globals.GetCurrentWXOpenId, "_DebugWXLogin.txt");
			}
		}

		public void AlipayLoginAction(SiteSettings site)
		{
			if (string.IsNullOrEmpty(AlipayFuwuConfig.appId) && !AlipayFuwuConfig.CommSetConfig(site.AlipayAppid, this.Page.Server.MapPath("~/"), "GBK"))
			{
				this.WriteFuwuError(this.Page.Request.QueryString.ToString(), "服务窗口参数配置不准确！");
				this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx");
			}
			string text = this.Page.Request.QueryString["auth_code"];
			string value = this.Page.Request.QueryString["scope"];
			if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(text))
			{
				this.WriteFuwuError(this.Page.Request.QueryString.ToString(), "已授权");
				AlipaySystemOauthTokenResponse oauthTokenResponse = AliOHHelper.GetOauthTokenResponse(text);
				this.WriteFuwuError(AliOHHelper.SerializeObject(oauthTokenResponse, true), "获取AccessToken");
				if (oauthTokenResponse == null || oauthTokenResponse.IsError || oauthTokenResponse.AccessToken == null)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx?returnUrl=" + Globals.UrlEncode(HttpContext.Current.Request.Url.AbsoluteUri.ToString()));
					return;
				}
				string text2 = oauthTokenResponse.AlipayUserId;
				string text3 = "";
				JObject jObject = JsonConvert.DeserializeObject(oauthTokenResponse.Body) as JObject;
				if (jObject["alipay_system_oauth_token_response"]["user_id"] != null)
				{
					text3 = jObject["alipay_system_oauth_token_response"]["user_id"].ToString();
				}
				if (this.HasReg(text3, "fuwu") || this.HasReg(text2, "fuwu"))
				{
					MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(text3, "fuwu");
					if (openIdMember == null || openIdMember.Status == Convert.ToInt32(UserStatus.DEL))
					{
						this.Page.Response.Redirect(Globals.ApplicationPath + "/logout.aspx");
					}
					string alipayOpenid = openIdMember.AlipayOpenid;
					if ((text2 != "" && text2 != alipayOpenid) || string.IsNullOrEmpty(alipayOpenid))
					{
						openIdMember.AlipayOpenid = text2;
						MemberProcessor.SetAlipayInfos(openIdMember);
					}
					this.setLogin(openIdMember.UserId);
					this.WriteFuwuError("已存在用户登入！", text3);
					return;
				}
				string accessToken = oauthTokenResponse.AccessToken;
				AlipayUserUserinfoShareResponse alipayUserUserinfo = AliOHHelper.GetAlipayUserUserinfo(accessToken);
				this.WriteFuwuError(AliOHHelper.SerializeObject(alipayUserUserinfo, true), "获取用户信息");
				string alipayUsername = "";
				string text4 = "";
				if (alipayUserUserinfo != null && !alipayUserUserinfo.IsError)
				{
					text4 = alipayUserUserinfo.Avatar;
					if (alipayUserUserinfo.RealName != null)
					{
						alipayUsername = alipayUserUserinfo.RealName;
					}
					if (string.IsNullOrEmpty(text2))
					{
						text2 = alipayUserUserinfo.UserId;
					}
					if (string.IsNullOrEmpty(text3))
					{
						JObject jObject2 = JsonConvert.DeserializeObject(alipayUserUserinfo.Body) as JObject;
						if (jObject2["alipay_user_id"] != null)
						{
							text3 = jObject2["alipay_user_id"].ToString();
						}
					}
				}
				string text5 = "FW*" + text3.Substring(10);
				string generateId = Globals.GetGenerateId();
				MemberInfo memberInfo = new MemberInfo();
				memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
				memberInfo.UserName = text5;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.SessionId = generateId;
				memberInfo.SessionEndTime = DateTime.Now.AddYears(10);
				memberInfo.UserHead = text4;
				memberInfo.AlipayAvatar = text4;
				memberInfo.AlipayLoginId = text5;
				memberInfo.AlipayOpenid = text2;
				memberInfo.AlipayUserId = text3;
				memberInfo.AlipayUsername = alipayUsername;
				HttpCookie httpCookie = HttpContext.Current.Request.Cookies["Vshop-ReferralId"];
				if (httpCookie != null)
				{
					memberInfo.ReferralUserId = Convert.ToInt32(httpCookie.Value);
				}
				else
				{
					memberInfo.ReferralUserId = 0;
				}
				memberInfo.Password = HiCryptographer.Md5Encrypt("888888");
				MemberProcessor.CreateMember(memberInfo);
				MemberInfo member = MemberProcessor.GetMember(generateId);
				this.setLogin(member.UserId);
				return;
			}
			else
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.WriteFuwuError(this.Page.Request.QueryString.ToString(), "拒绝授权");
					this.Page.Response.Redirect(Globals.ApplicationPath + "/UserLogin.aspx");
					return;
				}
				string text6 = AliOHHelper.AlipayAuthUrl(HttpContext.Current.Request.Url.ToString().Replace(":" + HttpContext.Current.Request.Url.Port, ""), site.AlipayAppid, "auth_userinfo");
				this.WriteFuwuError(text6, "用户登入授权的路径");
				this.Page.Response.Redirect(text6);
				return;
			}
		}

		public void WriteError(string msg, string OpenId)
		{
		}

		private string GetResponseResult(string url)
		{
			string result = "";
			WebRequest webRequest = WebRequest.Create(url);
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse())
			{
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8))
					{
						result = streamReader.ReadToEnd();
					}
				}
			}
			return result;
		}

		public void setLogin(int UserId)
		{
			HttpCookie httpCookie = new HttpCookie("Vshop-Member");
			httpCookie.Value = UserId.ToString();
			httpCookie.Expires = DateTime.Now.AddYears(1);
			HttpContext.Current.Response.Cookies.Add(httpCookie);
			HttpCookie httpCookie2 = new HttpCookie("Vshop-Member-Verify");
			httpCookie2.Value = Globals.EncryptStr(UserId.ToString());
			httpCookie2.Expires = DateTime.Now.AddYears(1);
			HttpContext.Current.Response.Cookies.Add(httpCookie2);
			this.Page.Session["userid"] = UserId.ToString();
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(UserId);
			if (userIdDistributors != null && userIdDistributors.UserId > 0)
			{
				Globals.SetDistributorCookie(userIdDistributors.UserId);
			}
		}

		public void WriteFuwuError(string msg, string OpenId)
		{
			Globals.Debuglog(msg + "||OpenId:" + OpenId, "_Debuglogfuwulogin.txt");
		}

		public bool HasReg(string OpenId, string fromWay = "wx")
		{
			MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(OpenId, fromWay);
			if (openIdMember != null)
			{
				this.setLogin(openIdMember.UserId);
				return true;
			}
			return false;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.LoadHtmlThemedControl())
			{
				this.AttachChildControls();
				return;
			}
			throw new SkinNotFoundException(this.SkinPath);
		}

		protected bool LoadHtmlThemedControl()
		{
			string text = this.ControlText();
			if (!string.IsNullOrEmpty(text))
			{
				Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}

		private string ControlText()
		{
			if (!this.SkinFileExists)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
			if (stringBuilder.Length == 0)
			{
				return null;
			}
			stringBuilder.Replace("<%", "").Replace("%>", "");
			string vshopSkinPath = Globals.GetVshopSkinPath(null);
			stringBuilder.Replace("/images/", vshopSkinPath + "/images/");
			stringBuilder.Replace("/script/", vshopSkinPath + "/script/");
			stringBuilder.Replace("/style/", vshopSkinPath + "/style/");
			stringBuilder.Replace("/utility/", Globals.ApplicationPath + "/utility/");
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
			MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			for (int i = matchCollection.Count - 1; i >= 0; i--)
			{
				int num = matchCollection[i].Groups["UrlName"].Index - 4;
				int num2 = matchCollection[i].Groups["UrlName"].Length + 4;
				if (matchCollection[i].Groups["Param"].Length > 0)
				{
					num2 += matchCollection[i].Groups["Param"].Length + 2;
				}
				stringBuilder.Remove(num, num2);
				stringBuilder.Insert(num, Globals.GetSiteUrls().UrlData.FormatUrl(matchCollection[i].Groups["UrlName"].Value.Trim(), new object[]
				{
					matchCollection[i].Groups["Param"].Value
				}));
			}
			return stringBuilder.ToString();
		}

		public void ReloadPage(NameValueCollection queryStrings)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings));
		}

		public void ReloadPage(NameValueCollection queryStrings, bool endResponse)
		{
			this.Page.Response.Redirect(this.GenericReloadUrl(queryStrings), endResponse);
		}

		private string GenericReloadUrl(NameValueCollection queryStrings)
		{
			if (queryStrings == null || queryStrings.Count == 0)
			{
				return this.Page.Request.Url.AbsolutePath;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.Page.Request.Url.AbsolutePath).Append("?");
			foreach (string text in queryStrings.Keys)
			{
				if (queryStrings[text] != null)
				{
					string text2 = queryStrings[text].Trim();
					if (!string.IsNullOrEmpty(text2) && text2.Length > 0)
					{
						stringBuilder.Append(text).Append("=").Append(this.Page.Server.UrlEncode(text2)).Append("&");
					}
				}
			}
			queryStrings.Clear();
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		protected void GotoResourceNotFound(string errorMsg = "")
		{
			this.GotoResourceNotFound(ErrorType.前台其它错误, errorMsg);
		}

		protected void GotoResourceNotFound(ErrorType type, string errorMsg = "")
		{
			this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/ResourceNotFound.aspx?type={0}&errorMsg={1}", (int)type, errorMsg));
		}
	}
}
