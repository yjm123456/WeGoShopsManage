using Hidistro.ControlPanel.OutPay.App;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.OutPay
{
	public class AliPaynotifyAmount_url : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Collections.Generic.SortedDictionary<string, string> requestPost = this.GetRequestPost();
			if (requestPost.Count > 0)
			{
				Notify notify = new Notify();
				bool flag = notify.Verify(requestPost, base.Request.Form["notify_id"], base.Request.Form["sign"]);
				if (flag)
				{
					string text = base.Request.Form["success_details"];
					try
					{
						if (!string.IsNullOrEmpty(text))
						{
							string[] array = text.Split(new char[]
							{
								'|'
							});
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string text2 = array2[i];
								string[] array3 = text2.Split(new char[]
								{
									'^'
								});
								if (array3.Length >= 8)
								{
									MemberAmountRequestInfo amountRequestDetail = MemberAmountProcessor.GetAmountRequestDetail(int.Parse(array3[0]));
									if (amountRequestDetail != null && amountRequestDetail.State != RequesState.已发放)
									{
										int[] serialids = new int[]
										{
											int.Parse(array3[0])
										};
										MemberAmountProcessor.SetAmountRequestStatus(serialids, 2, "支付宝付款：流水号" + array3[6] + ",支付时间：" + array3[7], "", ManagerHelper.GetCurrentManager().UserName);
										string url = Globals.FullPath("/Vshop/MemberAmountRequestDetail.aspx?Id=" + amountRequestDetail.Id);
										try
										{
											Messenger.SendWeiXinMsg_MemberAmountDrawCashRelease(amountRequestDetail, url);
										}
										catch
										{
										}
									}
								}
							}
						}
						string text3 = base.Request.Form["fail_details"];
						if (!string.IsNullOrEmpty(text3))
						{
							string[] array4 = text3.Split(new char[]
							{
								'|'
							});
							string[] array5 = array4;
							for (int j = 0; j < array5.Length; j++)
							{
								string text4 = array5[j];
								string[] array6 = text4.Split(new char[]
								{
									'^'
								});
								if (array6.Length >= 8)
								{
									MemberAmountRequestInfo amountRequestDetail2 = MemberAmountProcessor.GetAmountRequestDetail(int.Parse(array6[0]));
									if (amountRequestDetail2 != null && amountRequestDetail2.State != RequesState.已发放 && amountRequestDetail2.State != RequesState.驳回)
									{
										int[] serialids2 = new int[]
										{
											int.Parse(array6[0])
										};
										MemberAmountProcessor.SetAmountRequestStatus(serialids2, 3, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss  ") + array6[5] + array6[6], array6[3], ManagerHelper.GetCurrentManager().UserName);
									}
								}
							}
						}
					}
					catch (System.Exception ex)
					{
						try
						{
							Globals.Debuglog(string.Concat(new string[]
							{
								System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"),
								"验证成功，写入数据库失败->",
								base.Request.Form.ToString(),
								"||",
								ex.ToString()
							}), "_DebugLogAlipaynotify_url.txt");
						}
						catch (System.Exception)
						{
						}
					}
					base.Response.Write("success");
					return;
				}
				base.Response.Write("fail");
				try
				{
					Globals.Debuglog(System.DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]") + "验证失败1，写入数据库失败->" + base.Request.Form.ToString(), "_DebugLogAlipaynotify_url.txt");
					return;
				}
				catch (System.Exception)
				{
					return;
				}
			}
			base.Response.Write("无通知参数");
		}

		public System.Collections.Generic.SortedDictionary<string, string> GetRequestPost()
		{
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			string[] allKeys = form.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				sortedDictionary.Add(allKeys[i], base.Request.Form[allKeys[i]]);
			}
			return sortedDictionary;
		}
	}
}
