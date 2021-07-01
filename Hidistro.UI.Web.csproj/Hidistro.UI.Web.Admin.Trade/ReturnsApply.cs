using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	public class ReturnsApply : AdminPage
	{
		protected string Reurl = string.Empty;

		protected System.Web.UI.WebControls.TextBox txtOrderId;

		protected System.Web.UI.WebControls.Label lblStatus;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txtShopTo;

		protected RegionSelector dropRegion;

		protected System.Web.UI.WebControls.TextBox txtProductName;

		protected System.Web.UI.WebControls.DropDownList OrderFromList;

		protected System.Web.UI.WebControls.TextBox txtShopName;

		protected System.Web.UI.WebControls.TextBox txtUserName;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected System.Web.UI.WebControls.Button btnDeleteCheck;

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

		protected ReturnsApply() : base("m03", "ddp12")
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
			this.Reurl = base.Request.Url.ToString();
			if (!this.Reurl.Contains("?"))
			{
				this.Reurl += "?pageindex=1";
			}
			this.Reurl = System.Text.RegularExpressions.Regex.Replace(this.Reurl, "&t=(\\d+)", "");
			this.Reurl = System.Text.RegularExpressions.Regex.Replace(this.Reurl, "(\\?)t=(\\d+)", "?");
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			if (!this.Page.IsPostBack)
			{
				this.bindOrderType();
				this.BindOrders();
			}
		}

		private void bindOrderType()
		{
			int selectedIndex = 0;
			int.TryParse(base.Request.QueryString["orderType"], out selectedIndex);
			this.OrderFromList.SelectedIndex = selectedIndex;
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
				int num = (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId") != System.DBNull.Value) ? ((int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId")) : 0;
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnModifyPrice");
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton2 = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnSendGoods");
				System.Web.UI.WebControls.Button button = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnPayOrder");
				System.Web.UI.WebControls.Button button2 = (System.Web.UI.WebControls.Button)e.Item.FindControl("btnConfirmOrder");
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton3 = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnCloseOrderClient");
				System.Web.UI.HtmlControls.HtmlAnchor arg_198_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				System.Web.UI.HtmlControls.HtmlAnchor arg_1AE_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturn");
				System.Web.UI.HtmlControls.HtmlAnchor arg_1C4_0 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
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
					htmlInputButton3.Attributes.Add("onclick", "CloseOrderFun('" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID") + "')");
					htmlInputButton3.Visible = true;
					if (a != "hishop.plugins.payment.podrequest")
					{
						button.Visible = true;
					}
				}
				if (num > 0)
				{
					GroupBuyStatus groupBuyStatus = (GroupBuyStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
					htmlInputButton2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid && groupBuyStatus == GroupBuyStatus.Success);
				}
				else
				{
					htmlInputButton2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && a == "hishop.plugins.payment.podrequest"));
				}
				htmlInputButton2.Attributes.Add("onclick", "DialogFrame('../trade/SendOrderGoods.aspx?OrderId=" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID") + "&reurl='+ encodeURIComponent(goUrl),'订单发货',750,220)");
				button2.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
			}
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
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
				else if (e.CommandName == "FINISH_TRADE")
				{
					orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE);
				}
			}
		}

		protected void btnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的订单", false);
				return;
			}
			text = "'" + text.Replace(",", "','") + "'";
			int num = OrderHelper.DeleteOrders(text);
			this.BindOrders();
			this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
		}

		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult orders = OrderHelper.GetOrders(orderQuery);
			this.rptList.DataSource = orders.Data;
			this.rptList.DataBind();
			this.pager.TotalRecords = orders.TotalRecords;
			this.txtUserName.Text = orderQuery.UserName;
			this.txtOrderId.Text = orderQuery.OrderId;
			this.txtProductName.Text = orderQuery.ProductName;
			this.txtShopTo.Text = orderQuery.ShipTo;
			this.calendarStartDate.SelectedDate = orderQuery.StartDate;
			this.calendarEndDate.SelectedDate = orderQuery.EndDate;
			this.lblStatus.Text = ((int)orderQuery.Status).ToString();
			if (orderQuery.RegionId.HasValue)
			{
				this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
			}
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
			orderQuery.OrderItemsStatus = new OrderStatus?(OrderStatus.ApplyForRefund);
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
			orderQuery.PageIndex = this.pager.PageIndex;
			orderQuery.PageSize = this.pager.PageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}

		private void ReloadOrders(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("UserName", this.txtUserName.Text);
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("ProductName", this.txtProductName.Text);
			nameValueCollection.Add("ShipTo", this.txtShopTo.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("OrderType", this.OrderFromList.SelectedValue);
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
			if (this.dropRegion.GetSelectedRegionId().HasValue)
			{
				nameValueCollection.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
			}
			nameValueCollection.Add("StoreName", this.txtShopName.Text.Trim());
			base.ReloadPage(nameValueCollection);
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
				orderCombineStatus.DetailUrl = "OrderDetails.aspx?OrderId=" + System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderID").ToString() + "#returnInfo";
			}
		}
	}
}
