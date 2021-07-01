using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using jos_sdk_net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class RecycleStation : AdminPage
	{
		protected string Reurl = string.Empty;

		private StatisticNotifier myNotifier = new StatisticNotifier();

		private UpdateStatistics myEvent = new UpdateStatistics();

		protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;

		protected System.Web.UI.WebControls.TextBox txtOrderId;

		protected System.Web.UI.WebControls.Label lblStatus;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txtShopTo;

		protected System.Web.UI.WebControls.TextBox txtShopName;

		protected System.Web.UI.WebControls.DropDownList ddrOrderStatus;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Button btnRestoreCheck;

		protected System.Web.UI.HtmlControls.HtmlInputButton Button8;

		protected PageSize hrefPageSize;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected CloseTranReasonDropDownList ddlCloseReason;

		protected System.Web.UI.WebControls.Button btnCloseOrder;

		protected FormatedMoneyLabel lblOrderTotalForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected System.Web.UI.WebControls.TextBox txtRemark;

		protected System.Web.UI.HtmlControls.HtmlInputText txtcategoryId;

		protected System.Web.UI.WebControls.Button btnRemark;

		protected System.Web.UI.WebControls.Label lblOrderId;

		protected System.Web.UI.WebControls.Label lblOrderTotal;

		protected System.Web.UI.WebControls.Label lblRefundType;

		protected System.Web.UI.WebControls.Label lblRefundRemark;

		protected System.Web.UI.WebControls.Label lblContacts;

		protected System.Web.UI.WebControls.Label lblEmail;

		protected System.Web.UI.WebControls.Label lblTelephone;

		protected System.Web.UI.WebControls.Label lblAddress;

		protected System.Web.UI.WebControls.TextBox txtAdminRemark;

		protected System.Web.UI.WebControls.Label return_lblOrderId;

		protected System.Web.UI.WebControls.Label return_lblOrderTotal;

		protected System.Web.UI.WebControls.Label return_lblRefundType;

		protected System.Web.UI.WebControls.Label return_lblReturnRemark;

		protected System.Web.UI.WebControls.Label return_lblContacts;

		protected System.Web.UI.WebControls.Label return_lblEmail;

		protected System.Web.UI.WebControls.Label return_lblTelephone;

		protected System.Web.UI.WebControls.Label return_lblAddress;

		protected System.Web.UI.WebControls.TextBox return_txtRefundMoney;

		protected System.Web.UI.WebControls.TextBox return_txtAdminRemark;

		protected System.Web.UI.WebControls.Label replace_lblOrderId;

		protected System.Web.UI.WebControls.Label replace_lblOrderTotal;

		protected System.Web.UI.WebControls.Label replace_lblComments;

		protected System.Web.UI.WebControls.Label replace_lblContacts;

		protected System.Web.UI.WebControls.Label replace_lblEmail;

		protected System.Web.UI.WebControls.Label replace_lblTelephone;

		protected System.Web.UI.WebControls.Label replace_lblAddress;

		protected System.Web.UI.WebControls.Label replace_lblPostCode;

		protected System.Web.UI.WebControls.TextBox replace_txtAdminRemark;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hiddOrderId;

		protected System.Web.UI.WebControls.Button btnDeleteAndUpdateData;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;

		protected System.Web.UI.WebControls.Button btnAcceptRefund;

		protected System.Web.UI.WebControls.Button btnRefuseRefund;

		protected System.Web.UI.WebControls.Button btnAcceptReturn;

		protected System.Web.UI.WebControls.Button btnRefuseReturn;

		protected System.Web.UI.WebControls.Button btnAcceptReplace;

		protected System.Web.UI.WebControls.Button btnRefuseReplace;

		protected System.Web.UI.WebControls.Button btnOrderGoods;

		protected System.Web.UI.WebControls.Button btnProductGoods;

		protected RecycleStation() : base("m03", "ddp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("isCallback");
			if (a == "true")
			{
				if (string.IsNullOrEmpty(base.Request["ReturnsId"]))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				int num;
				string text;
				if (base.Request["type"] == "refund")
				{
					RefundHelper.GetRefundType(base.Request["orderId"], out num, out text);
				}
				else
				{
					num = 0;
					text = "";
				}
				string arg;
				if (num == 1)
				{
					arg = "退到预存款";
				}
				else
				{
					arg = "银行转帐";
				}
				stringBuilder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
				if (!(base.Request["type"] == "replace"))
				{
					stringBuilder.AppendFormat(",\"RefundType\":\"{0}\"", num);
					stringBuilder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", arg);
				}
				stringBuilder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.ShipTo);
				stringBuilder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress);
				stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", (orderInfo.TelPhone + " " + orderInfo.CellPhone).Trim());
				stringBuilder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address);
				stringBuilder.AppendFormat(",\"Remark\":\"{0}\"", text.Replace("\r\n", ""));
				stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode);
				stringBuilder.AppendFormat(",\"GroupBuyId\":\"{0}\"", (orderInfo.GroupBuyId > 0) ? orderInfo.GroupBuyId : 0);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
				base.Response.End();
			}
			else if (a == "GetOrdersStates")
			{
				base.Response.ContentType = "application/json";
				System.Data.DataTable allOrderID = OrderHelper.GetAllOrderID();
				int num2 = allOrderID.Select("OrderStatus=" + 12).Length;
				int num3 = allOrderID.Select("OrderStatus=" + 1).Length;
				int num4 = allOrderID.Select(string.Concat(new object[]
				{
					"OrderStatus=",
					2,
					" or (OrderStatus=",
					1,
					" AND Gateway = 'hishop.plugins.payment.podrequest')"
				})).Length;
				int num5 = allOrderID.Select("OrderStatus=" + 3).Length;
				int num6 = allOrderID.Select("OrderStatus=" + 5).Length;
				int num7 = OrderHelper.GetCountOrderIDByStatus(new OrderStatus?(OrderStatus.BuyerAlreadyPaid), new OrderStatus?(OrderStatus.ApplyForRefund)) + OrderHelper.GetCountOrderIDByStatus(new OrderStatus?(OrderStatus.SellerAlreadySent), new OrderStatus?(OrderStatus.ApplyForReturns));
				int num8 = allOrderID.Select("OrderStatus=" + 4).Length;
				string s = string.Concat(new object[]
				{
					"{\"type\":\"1\",\"allcount\":",
					num2,
					",\"waibuyerpaycount\":",
					num3,
					",\"buyalreadypaidcount\":",
					num4,
					",\"sellalreadysentcount\":",
					num5,
					",\"finishedcount\":",
					num6,
					",\"applyforreturnscount\":",
					num7,
					",\"closedcount\":",
					num8,
					"}"
				});
				base.Response.Write(s);
				base.Response.End();
			}
			this.Reurl = base.Request.Url.ToString();
			if (!this.Reurl.Contains("?"))
			{
				this.Reurl += "?pageindex=1";
			}
			this.Reurl = System.Text.RegularExpressions.Regex.Replace(this.Reurl, "&t=(\\d+)", "");
			this.Reurl = System.Text.RegularExpressions.Regex.Replace(this.Reurl, "(\\?)t=(\\d+)", "?");
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			this.btnDeleteAndUpdateData.Click += new System.EventHandler(this.btnDeleteAndUpdateData_Click);
			this.btnProductGoods.Click += new System.EventHandler(this.btnProductGoods_Click);
			this.btnOrderGoods.Click += new System.EventHandler(this.btnOrderGoods_Click);
			this.btnRestoreCheck.Click += new System.EventHandler(this.btnRestoreCheck_Click);
			if (!this.Page.IsPostBack)
			{
				this.SetOrderStatusLink();
				this.bindOrderType();
				this.BindOrders();
			}
		}

		private void bindOrderType()
		{
			int num = 0;
			int.TryParse(base.Request.QueryString["orderType"], out num);
		}

		private void btnProductGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的订单", false);
				return;
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				list.Add("'" + str + "'");
			}
			System.Data.DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>商品名称</td>");
			stringBuilder.AppendLine("<td>货号</td>");
			stringBuilder.AppendLine("<td>规格</td>");
			stringBuilder.AppendLine("<td>拣货数量</td>");
			stringBuilder.AppendLine("<td>现库存数</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in productGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			stringBuilder.AppendLine("</body></html>");
			base.Response.Clear();
			base.Response.Buffer = false;
			base.Response.Charset = "GB2312";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "application/ms-excel";
			this.EnableViewState = false;
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}

		private void btnOrderGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的订单", false);
				return;
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				list.Add("'" + str + "'");
			}
			System.Data.DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>订单单号</td>");
			stringBuilder.AppendLine("<td>商品名称</td>");
			stringBuilder.AppendLine("<td>货号</td>");
			stringBuilder.AppendLine("<td>规格</td>");
			stringBuilder.AppendLine("<td>拣货数量</td>");
			stringBuilder.AppendLine("<td>现库存数</td>");
			stringBuilder.AppendLine("<td>备注</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in orderGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Remark"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			stringBuilder.AppendLine("</body></html>");
			base.Response.Clear();
			base.Response.Buffer = false;
			base.Response.Charset = "GB2312";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "application/ms-excel";
			this.EnableViewState = false;
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}

		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadOrders(true);
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rptSubList");
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID").ToString());
				if (orderInfo != null && orderInfo.LineItems.Count > 0)
				{
					repeater.DataSource = orderInfo.LineItems.Values;
					repeater.DataBind();
				}
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				string a = "";
				if (!(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") is System.DBNull))
				{
					a = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
				}
				if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId") != System.DBNull.Value)
				{
					int arg_10B_0 = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId");
				}
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnModifyPrice");
				System.Web.UI.HtmlControls.HtmlInputButton arg_138_0 = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnSendGoods");
				System.Web.UI.WebControls.Button button = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnPayOrder");
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton2 = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnCloseOrderClient");
				System.Web.UI.HtmlControls.HtmlAnchor arg_17C_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				System.Web.UI.HtmlControls.HtmlAnchor arg_192_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturn");
				System.Web.UI.HtmlControls.HtmlAnchor arg_1A8_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("WeiXinNickName");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litOtherInfo");
				int totalPointNumber = orderInfo.GetTotalPointNumber();
				MemberInfo member = MemberProcessor.GetMember(orderInfo.UserId, true);
				if (member != null)
				{
					literal.Text = "买家：" + member.UserName;
				}
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				decimal total = orderInfo.GetTotal();
				if (total > 0m)
				{
					stringBuilder.Append("<strong>￥" + total.ToString("F2") + "</strong>");
					stringBuilder.Append("<small>(含运费￥" + orderInfo.AdjustedFreight.ToString("F2") + ")</small>");
				}
				if (totalPointNumber > 0)
				{
					stringBuilder.Append("<small>" + totalPointNumber.ToString() + "积分</small>");
				}
				if (orderInfo.PaymentType == "货到付款")
				{
					stringBuilder.Append("<span class=\"setColor bl\"><strong>货到付款</strong></span>");
				}
				if (string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append("<strong>￥" + total.ToString("F2") + "</strong>");
				}
				literal2.Text = stringBuilder.ToString();
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					htmlInputButton.Visible = true;
					htmlInputButton.Attributes.Add("onclick", "DialogFrame('../trade/EditOrder.aspx?OrderId=" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID") + "&reurl='+ encodeURIComponent(goUrl),'修改订单价格',900,450)");
					htmlInputButton2.Attributes.Add("onclick", "CloseOrderFun('" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID") + "')");
					htmlInputButton2.Visible = true;
					if (a != "hishop.plugins.payment.podrequest")
					{
						button.Visible = true;
					}
				}
			}
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			bool flag = false;
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
			if (orderInfo != null)
			{
				if (e.CommandName == "CONFIRM_PAY" && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
				{
					if (orderInfo.GroupBuyId > 0)
					{
						return;
					}
				}
				else if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
				{
					System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
					LineItemInfo lineItemInfo = new LineItemInfo();
					foreach (System.Collections.Generic.KeyValuePair<string, LineItemInfo> current in lineItems)
					{
						lineItemInfo = current.Value;
						if (lineItemInfo.OrderItemsStatus == OrderStatus.ApplyForRefund || lineItemInfo.OrderItemsStatus == OrderStatus.ApplyForReturns)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						return;
					}
					this.ShowMsg("订单中商品有退货(款)不允许完成!", false);
					return;
				}
				else
				{
					if (e.CommandName == "delete")
					{
						string text = e.CommandArgument.ToString();
						text = "'" + text.Replace(",", "','") + "'";
						int num = OrderHelper.RealDeleteOrders(text);
						this.BindOrders();
						this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
						return;
					}
					if (e.CommandName == "restore")
					{
						string text2 = e.CommandArgument.ToString();
						text2 = "'" + text2.Replace(",", "','") + "'";
						int num2 = OrderHelper.RestoreOrders(text2);
						this.BindOrders();
						this.ShowMsg(string.Format("成功还原了订单", num2), true);
					}
				}
			}
		}

		protected void btnDelete_Click(object sender, System.EventArgs e)
		{
			string text = this.hiddOrderId.Value;
			text = "'" + text.Replace(",", "','") + "'";
			int num = OrderHelper.RealDeleteOrders(text);
			this.BindOrders();
			this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
		}

		protected void btnDeleteAndUpdateData_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (string.IsNullOrEmpty(text))
			{
				text = this.hiddOrderId.Value;
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的订单", false);
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(array[i]);
				if (orderInfo != null)
				{
					System.DateTime? dateTime = new System.DateTime?(orderInfo.OrderDate);
					System.DateTime? dateTime2 = orderInfo.PayDate;
					if (orderInfo.Gateway == "hishop.plugins.payment.podrequest")
					{
						dateTime2 = dateTime;
					}
					if (dateTime2.HasValue && dateTime2.Value.ToString("yyyy-MM-dd") != System.DateTime.Now.ToString("yyyy-MM-dd"))
					{
						OrderHelper.RealDeleteOrders(array[i], new System.DateTime?(dateTime2.Value));
					}
					else
					{
						OrderHelper.RealDeleteOrders(array[i]);
					}
                    //删除京东绑定订单表
                    int DeleteAlreadyResult = connection.DeleteOrderAlreadySend(array[i]);

                }
			}
			this.BindOrders();
			this.ShowMsg("成功删除订单", true);
		}

		protected void btnRestoreCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要还原的订单", false);
				return;
			}
			text = "'" + text.Replace(",", "','") + "'";
			int num = OrderHelper.RestoreOrders(text);
			this.BindOrders();
			this.ShowMsg(string.Format("成功还原了订单", num), true);
		}

		protected void rptSubList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				OrderCombineStatus orderCombineStatus = (OrderCombineStatus)e.Item.FindControl("lbOrderCombineStatus");
				orderCombineStatus.OrderItemID = Globals.ToNum(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "ID").ToString());
				orderCombineStatus.OrderID = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID").ToString();
				orderCombineStatus.SkuID = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "SkuID").ToString();
				orderCombineStatus.Type = Globals.ToNum(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Type").ToString());
				orderCombineStatus.DetailUrl = "RecycleStationDetail.aspx?OrderId=" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID").ToString() + "#returnInfo";
			}
		}

		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult deleteOrders = OrderHelper.GetDeleteOrders(orderQuery);
			this.rptList.DataSource = deleteOrders.Data;
			this.rptList.DataBind();
			this.pager.TotalRecords = deleteOrders.TotalRecords;
			this.txtOrderId.Text = orderQuery.OrderId;
			this.txtShopTo.Text = orderQuery.ShipTo;
			this.calendarStartDate.SelectedDate = orderQuery.StartDate;
			this.calendarEndDate.SelectedDate = orderQuery.EndDate;
			this.lblStatus.Text = ((int)orderQuery.Status).ToString();
			this.ddrOrderStatus.SelectedValue = orderQuery.DeleteBeforeState.ToString();
			this.txtShopName.Text = orderQuery.StoreName;
		}

		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				orderQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				orderQuery.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
			{
				orderQuery.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
			{
				orderQuery.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserID"]))
			{
				orderQuery.UserId = new int?(System.Convert.ToInt32(this.Page.Request.QueryString["UserID"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				orderQuery.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				orderQuery.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				orderQuery.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]).AddMilliseconds(86399.0));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
			{
				int status = 0;
				if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out status))
				{
					orderQuery.Status = (OrderStatus)status;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out value))
				{
					orderQuery.IsPrinted = new int?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
			{
				int value2 = 0;
				if (int.TryParse(this.Page.Request.QueryString["ModeId"], out value2))
				{
					orderQuery.ShippingModeId = new int?(value2);
				}
			}
			int value3;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out value3))
			{
				orderQuery.RegionId = new int?(value3);
			}
			int value4;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && int.TryParse(this.Page.Request.QueryString["UserId"], out value4))
			{
				orderQuery.UserId = new int?(value4);
			}
			int num = 0;
			if (int.TryParse(base.Request.QueryString["orderType"], out num) && num > 0)
			{
				orderQuery.Type = new OrderQuery.OrderType?((OrderQuery.OrderType)num);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StoreName"]))
			{
				orderQuery.StoreName = Globals.UrlDecode(this.Page.Request.QueryString["StoreName"]);
			}
			int deleteBeforeState;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["DeleteBeforeState"]) && int.TryParse(this.Page.Request.QueryString["DeleteBeforeState"], out deleteBeforeState))
			{
				orderQuery.DeleteBeforeState = deleteBeforeState;
			}
			orderQuery.PageIndex = this.pager.PageIndex;
			orderQuery.PageSize = this.pager.PageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			orderQuery.Status = OrderStatus.Deleted;
			return orderQuery;
		}

		private void ReloadOrders(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("ShipTo", this.txtShopTo.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("OrderStatus", this.lblStatus.Text);
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			nameValueCollection.Add("StoreName", this.txtShopName.Text.Trim());
			nameValueCollection.Add("DeleteBeforeState", this.ddrOrderStatus.SelectedValue);
			base.ReloadPage(nameValueCollection);
		}

		private void SetOrderStatusLink()
		{
			string format = "RecycleStation.aspx?orderStatus={0}";
			this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
		}

		private void btnRemark_Click(object sender, System.EventArgs e)
		{
			if (this.hidOrderId.Value == "0")
			{
				string text = Globals.RequestFormStr("CheckBoxGroup");
				if (text.Length <= 0)
				{
					this.ShowMsg("请先选择要批量备注的订单", false);
					return;
				}
				if (this.txtRemark.Text.Length > 300)
				{
					this.ShowMsg("备注长度限制在300个字符以内", false);
					return;
				}
				string[] array = text.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (!string.IsNullOrEmpty(text2))
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(text2);
						orderInfo.OrderId = text2;
						if (this.orderRemarkImageForRemark.SelectedItem != null)
						{
							orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
						}
						orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
						OrderHelper.SaveRemark(orderInfo);
					}
				}
				this.ShowMsg("批量保存备注成功", true);
				this.BindOrders();
				return;
			}
			else
			{
				if (this.txtRemark.Text.Length > 300)
				{
					this.ShowMsg("备注长度限制在300个字符以内", false);
					return;
				}
				OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
				orderInfo2.OrderId = this.hidOrderId.Value;
				if (this.orderRemarkImageForRemark.SelectedItem != null)
				{
					orderInfo2.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
				}
				orderInfo2.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
				if (OrderHelper.SaveRemark(orderInfo2))
				{
					this.BindOrders();
					this.ShowMsg("保存备注成功", true);
					return;
				}
				this.ShowMsg("保存失败", false);
				return;
			}
		}

		private void btnCloseOrder_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
			orderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
			if ("请选择关闭的理由" == orderInfo.CloseReason)
			{
				this.ShowMsg("请选择关闭的理由", false);
				return;
			}
			if (OrderHelper.CloseTransaction(orderInfo))
			{
				orderInfo.OnClosed();
				this.BindOrders();
				this.ShowMsg("关闭订单成功", true);
				return;
			}
			this.ShowMsg("关闭订单失败", false);
		}

		protected string GetSpitLink(object oSplitState, object oOrderStatus, object oOrderID)
		{
			string result = string.Empty;
			int num = Globals.ToNum(oSplitState);
			if (num < 1)
			{
				string text = oOrderID.ToString();
				OrderStatus orderStatus = (OrderStatus)oOrderStatus;
				if ((orderStatus == OrderStatus.BuyerAlreadyPaid || orderStatus == OrderStatus.WaitBuyerPay) && OrderHelper.GetItemNumByOrderID(text) > 1)
				{
					result = string.Concat(new string[]
					{
						"<a href='OrderSplit.aspx?OrderId=",
						text,
						"&reurl=",
						base.Server.UrlEncode(base.Request.Url.ToString()),
						"' target='_blank' class='btn btn-default resetSize inputw100 bl mb5'>订单拆分</a>"
					});
				}
			}
			return result;
		}
	}
}
