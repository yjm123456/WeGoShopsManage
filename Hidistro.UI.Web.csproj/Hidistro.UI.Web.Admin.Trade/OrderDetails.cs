using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.Entities.StatisticsReport;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.Vshop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Trade
{
	[PrivilegeCheck(Privilege.Orders)]
	public class OrderDetails : AdminPage
	{
		public bool showOrderExpress;

		private StatisticNotifier myNotifier = new StatisticNotifier();

		private UpdateStatistics myEvent = new UpdateStatistics();

		protected decimal otherDiscountPrice;

		protected decimal BalancePayFreightMoneyTotal;

		protected decimal BalancePayMoneyTotal;

		protected string orderId = Globals.RequestQueryStr("OrderId");

		protected string comCode = "";

		private string reurl = string.Empty;

		private OrderInfo order;

		protected string ProcessClass2 = string.Empty;

		protected string ProcessClass3 = string.Empty;

		protected string ProcessClass4 = string.Empty;

		protected System.Web.UI.WebControls.HiddenField hdfOrderID;

		protected System.Web.UI.WebControls.HiddenField hdProductID;

		protected System.Web.UI.WebControls.HiddenField hdSkuID;

		protected System.Web.UI.WebControls.HiddenField hdReturnsId;

		protected System.Web.UI.WebControls.HiddenField hdOrderItemId;

		protected System.Web.UI.WebControls.HiddenField hdHasNewKey;

		protected System.Web.UI.WebControls.HiddenField hdExpressUrl;

		protected System.Web.UI.WebControls.HiddenField hdCompanyCode;

		protected System.Web.UI.WebControls.HiddenField hdBalance;

		protected System.Web.UI.HtmlControls.HtmlGenericControl divOrderProcess;

		protected System.Web.UI.WebControls.Literal litOrderDate;

		protected System.Web.UI.WebControls.Literal litPayDate;

		protected System.Web.UI.WebControls.Literal litShippingDate;

		protected System.Web.UI.WebControls.Literal litFinishDate;

		protected OrderStatusLabel lblOrderStatus;

		protected System.Web.UI.WebControls.Label lbCloseReason;

		protected System.Web.UI.WebControls.Label lbReason;

		protected System.Web.UI.WebControls.Literal litOrderId;

		protected System.Web.UI.HtmlControls.HtmlGenericControl divRemarkShow;

		protected OrderRemarkImage OrderRemarkImageLink;

		protected System.Web.UI.WebControls.Literal litManagerRemark;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnModifyAddr;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnModifyPrice;

		protected System.Web.UI.WebControls.Button btnConfirmPay;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnSendGoods;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnClocsOrder;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnConfirmOrder;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnViewLogistic;

		protected System.Web.UI.WebControls.Literal litRealName;

		protected System.Web.UI.WebControls.Literal litShippingRegion;

		protected System.Web.UI.WebControls.Literal litPayType;

		protected System.Web.UI.WebControls.Literal litUserTel;

		protected System.Web.UI.WebControls.Literal litShipToDate;

		protected System.Web.UI.WebControls.Literal litRemark;

		protected System.Web.UI.WebControls.Literal litUserName;

		protected System.Web.UI.WebControls.Literal litWeiXinNickName;

		protected System.Web.UI.WebControls.Repeater rptItemList;

		protected System.Web.UI.WebControls.Literal litSiteName;

		protected System.Web.UI.WebControls.Literal litActivityShow;

		protected FormatedMoneyLabel lblorderTotalForRemark;

		protected System.Web.UI.WebControls.Literal litFreight;

		protected System.Web.UI.WebControls.Literal litCommissionInfo;

		protected System.Web.UI.WebControls.Repeater rptRefundList;

		protected System.Web.UI.WebControls.Literal lblOriAddress;

		protected System.Web.UI.HtmlControls.HtmlGenericControl pNewAddress;

		protected System.Web.UI.WebControls.Literal litAddress;

		protected System.Web.UI.WebControls.Literal litModeName;

		protected System.Web.UI.WebControls.Literal litCompanyName;

		protected System.Web.UI.HtmlControls.HtmlInputButton btnUpdateExpress;

		protected System.Web.UI.WebControls.Literal litShipOrderNumber;

		protected System.Web.UI.HtmlControls.HtmlGenericControl pLoginsticInfo;

		protected System.Web.UI.WebControls.TextBox txtMoney;

		protected System.Web.UI.WebControls.TextBox txtMemo;

		protected System.Web.UI.WebControls.Button btnAgreeConfirm;

		protected System.Web.UI.WebControls.TextBox txtAdminMemo;

		protected System.Web.UI.WebControls.Button btnRefuseConfirm;

		protected System.Web.UI.WebControls.Literal spanOrderId;

		protected FormatedTimeLabel lblorderDateForRemark;

		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;

		protected System.Web.UI.WebControls.TextBox txtRemark;

		protected System.Web.UI.HtmlControls.HtmlInputText txtcategoryId;

		protected System.Web.UI.WebControls.Button btnRemark;

		protected System.Web.UI.WebControls.Literal litName;

		protected System.Web.UI.WebControls.Literal litExOrderId;

		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;

		protected OrderStatusLabel OrderStatusLabelHtml;

		protected CloseTranReasonDropDownList ddlCloseReason;

		protected System.Web.UI.WebControls.Button btnCloseOrder;

		protected System.Web.UI.WebControls.Button btnMondifyShip;

		protected PaymentDropDownList ddlpayment;

		protected System.Web.UI.WebControls.Button btnMondifyPay;

		protected System.Web.UI.HtmlControls.HtmlAnchor power;

		protected OrderDetails() : base("m03", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			ExpressSet expressSet = ExpressHelper.GetExpressSet();
			this.hdHasNewKey.Value = "0";
			this.hdExpressUrl.Value = "";
			if (expressSet != null)
			{
				if (!string.IsNullOrEmpty(expressSet.NewKey))
				{
					this.hdHasNewKey.Value = "1";
				}
				if (!string.IsNullOrEmpty(expressSet.Url.Trim()))
				{
					this.hdExpressUrl.Value = expressSet.Url.Trim();
				}
			}
			string a = Globals.RequestFormStr("posttype");
			if (a == "modifyRefundMondy")
			{
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
				decimal num = 0m;
				decimal.TryParse(Globals.RequestFormStr("price"), out num);
				int num2 = Globals.RequestFormNum("pid");
				string text = Globals.RequestFormStr("oid");
				if (num > 0m && num2 > 0 && !string.IsNullOrEmpty(text))
				{
					if (RefundHelper.UpdateRefundMoney(text, num2, num))
					{
						s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
					}
				}
				else if (num <= 0m)
				{
					s = "{\"type\":\"0\",\"tips\":\"退款金额需大于0！\"}";
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			this.reurl = "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + System.DateTime.Now.ToString("HHmmss");
			this.btnMondifyPay.Click += new System.EventHandler(this.btnMondifyPay_Click);
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			if (this.order != null)
			{
				if (!base.IsPostBack)
				{
					this.btnUpdateExpress.Visible = (this.order.OrderStatus == OrderStatus.SellerAlreadySent);
					if (string.IsNullOrEmpty(this.orderId))
					{
						base.GotoResourceNotFound();
						return;
					}
					this.hdfOrderID.Value = this.orderId;
					this.litOrderDate.Text = this.order.OrderDate.ToString("yyyy-MM-dd HH:mm:ss");
					if (this.order.PayDate.HasValue)
					{
						this.litPayDate.Text = System.DateTime.Parse(this.order.PayDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
					}
					if (this.order.ShippingDate.HasValue)
					{
						this.litShippingDate.Text = System.DateTime.Parse(this.order.ShippingDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
					}
					if (this.order.FinishDate.HasValue)
					{
						this.litFinishDate.Text = System.DateTime.Parse(this.order.FinishDate.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
					}
					this.lblOrderStatus.OrderStatusCode = this.order.OrderStatus;
					switch (this.order.OrderStatus)
					{
					case OrderStatus.WaitBuyerPay:
						this.ProcessClass2 = "active";
						if (this.order.Gateway != "hishop.plugins.payment.podrequest")
						{
							this.btnConfirmPay.Visible = true;
						}
						this.btnModifyAddr.Attributes.Add("onclick", "DialogFrame('../trade/ShipAddress.aspx?action=update&OrderId=" + this.orderId + "','修改收货地址',620,410)");
						this.btnModifyAddr.Visible = true;
						break;
					case OrderStatus.BuyerAlreadyPaid:
						this.ProcessClass2 = "ok";
						this.ProcessClass3 = "active";
						this.btnModifyAddr.Attributes.Add("onclick", "DialogFrame('../trade/ShipAddress.aspx?action=update&OrderId=" + this.orderId + "','修改收货地址',620,410)");
						this.btnModifyAddr.Visible = true;
						break;
					case OrderStatus.SellerAlreadySent:
						this.ProcessClass2 = "ok";
						this.ProcessClass3 = "ok";
						this.ProcessClass4 = "active";
						break;
					case OrderStatus.Finished:
						this.ProcessClass2 = "ok";
						this.ProcessClass3 = "ok";
						this.ProcessClass4 = "ok";
						break;
					}
					if (this.order.ManagerMark.HasValue)
					{
						this.OrderRemarkImageLink.ManagerMarkValue = (int)this.order.ManagerMark.Value;
						this.litManagerRemark.Text = this.order.ManagerRemark;
					}
					else
					{
						this.divRemarkShow.Visible = false;
					}
					this.comCode = this.order.ExpressCompanyAbb;
					this.litRemark.Text = this.order.Remark;
					string text2 = this.order.OrderId;
					string orderMarking = this.order.OrderMarking;
					if (!string.IsNullOrEmpty(orderMarking))
					{
						text2 = string.Concat(new string[]
						{
							text2,
							" (",
							this.order.PaymentType,
							"流水号：",
							orderMarking,
							")"
						});
					}
					this.litOrderId.Text = text2;
					this.litUserName.Text = this.order.Username;
					if (this.order.BalancePayMoneyTotal > 0m)
					{
						this.litPayType.Text = "余额支付 ￥" + this.order.BalancePayMoneyTotal.ToString("F2");
					}
					decimal d = this.order.GetTotal() - this.order.BalancePayMoneyTotal;
					if (d > 0m)
					{
						if (!string.IsNullOrEmpty(this.litPayType.Text.Trim()))
						{
							this.litPayType.Text = this.litPayType.Text;
						}
						if (d - this.order.CouponFreightMoneyTotal > 0m)
						{
							if (!string.IsNullOrEmpty(this.litPayType.Text))
							{
								System.Web.UI.WebControls.Literal expr_640 = this.litPayType;
								expr_640.Text += "<br>";
							}
							System.Web.UI.WebControls.Literal expr_65B = this.litPayType;
							expr_65B.Text = expr_65B.Text + ((!this.order.PayDate.HasValue) ? "待支付" : this.order.PaymentType) + " ￥" + d.ToString("F2");
						}
					}
					else if (this.order.PaymentTypeId == 77)
					{
						this.litPayType.Text = this.order.PaymentType + " ￥" + this.order.PointToCash.ToString("F2");
					}
					else if (this.order.PaymentTypeId == 55)
					{
						this.litPayType.Text = this.order.PaymentType + " ￥" + this.order.RedPagerAmount.ToString("F2");
					}
					this.litShipToDate.Text = this.order.ShipToDate;
					this.litRealName.Text = this.order.ShipTo;
					this.litName.Text = this.order.ShipTo;
					this.litExOrderId.Text = this.order.OrderId;
					this.txtShipOrderNumber.Text = this.order.ShipOrderNumber;
					this.OrderStatusLabelHtml.OrderStatusCode = this.order.OrderStatus;
					this.litUserTel.Text = (string.IsNullOrEmpty(this.order.CellPhone) ? this.order.TelPhone : this.order.CellPhone);
					this.litShippingRegion.Text = this.order.ShippingRegion;
					this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
					this.BalancePayFreightMoneyTotal = this.order.BalancePayFreightMoneyTotal;
					this.BalancePayMoneyTotal = this.order.BalancePayMoneyTotal;
					if (this.order.ReferralUserId == 0)
					{
						this.litSiteName.Text = "主站";
					}
					else
					{
						DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(this.order.ReferralUserId);
						if (distributorInfo != null)
						{
							this.litSiteName.Text = distributorInfo.StoreName;
						}
					}
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					if (!string.IsNullOrEmpty(this.order.ActivitiesName))
					{
						this.otherDiscountPrice += this.order.DiscountAmount;
						stringBuilder.Append(string.Concat(new string[]
						{
							"<p>",
							this.order.ActivitiesName,
							":￥",
							this.order.DiscountAmount.ToString("F2"),
							"</p>"
						}));
					}
					if (!string.IsNullOrEmpty(this.order.ReducedPromotionName))
					{
						this.otherDiscountPrice += this.order.ReducedPromotionAmount;
						stringBuilder.Append(string.Concat(new string[]
						{
							"<p>",
							this.order.ReducedPromotionName,
							":￥",
							this.order.ReducedPromotionAmount.ToString("F2"),
							"</p>"
						}));
					}
					if (!string.IsNullOrEmpty(this.order.CouponName))
					{
						this.otherDiscountPrice += this.order.CouponAmount;
						stringBuilder.Append(string.Concat(new string[]
						{
							"<p>",
							this.order.CouponName,
							":￥",
							this.order.CouponAmount.ToString("F2"),
							"</p>"
						}));
					}
					if (!string.IsNullOrEmpty(this.order.RedPagerActivityName))
					{
						this.otherDiscountPrice += this.order.RedPagerAmount;
						stringBuilder.Append(string.Concat(new string[]
						{
							"<p>",
							this.order.RedPagerActivityName,
							":￥",
							this.order.RedPagerAmount.ToString("F2"),
							"</p>"
						}));
					}
					if (this.order.PointToCash > 0m)
					{
						this.otherDiscountPrice += this.order.PointToCash;
						stringBuilder.Append("<p>积分抵现:￥" + this.order.PointToCash.ToString("F2") + "</p>");
					}
					this.order.GetAdjustCommssion();
					decimal d2 = 0m;
					decimal num3 = 0m;
					foreach (LineItemInfo current in this.order.LineItems.Values)
					{
						if (current.IsAdminModify)
						{
							d2 += current.ItemAdjustedCommssion;
						}
						else
						{
							num3 += current.ItemAdjustedCommssion;
						}
					}
					if (d2 != 0m)
					{
						if (d2 > 0m)
						{
							stringBuilder.Append("<p>管理员调价减:￥" + d2.ToString("F2") + "</p>");
						}
						else
						{
							stringBuilder.Append("<p>管理员调价加:￥" + (d2 * -1m).ToString("F2") + "</p>");
						}
					}
					if (num3 != 0m)
					{
						if (num3 > 0m)
						{
							stringBuilder.Append("<p>分销商调价减:￥" + num3.ToString("F2") + "</p>");
						}
						else
						{
							stringBuilder.Append("<p>分销商调价加:￥" + (num3 * -1m).ToString("F2") + "</p>");
						}
					}
					this.litActivityShow.Text = stringBuilder.ToString();
					if ((int)this.lblOrderStatus.OrderStatusCode != 4)
					{
						this.lbCloseReason.Visible = false;
					}
					else
					{
						this.divOrderProcess.Visible = false;
						this.lbReason.Text = this.order.CloseReason;
					}
					if (this.order.OrderStatus == OrderStatus.BuyerAlreadyPaid || (this.order.OrderStatus == OrderStatus.WaitBuyerPay && this.order.Gateway == "hishop.plugins.payment.podrequest"))
					{
						this.btnSendGoods.Visible = true;
					}
					else
					{
						this.btnSendGoods.Visible = false;
					}
					if ((this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(this.order.ExpressCompanyAbb))
					{
						this.pLoginsticInfo.Visible = true;
						this.btnViewLogistic.Visible = true;
						if (Express.GetExpressType() == "kuaidi100" && this.power != null)
						{
							this.power.Visible = true;
						}
					}
					if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
					{
						this.btnClocsOrder.Visible = true;
						this.btnModifyPrice.Visible = true;
					}
					else
					{
						this.btnClocsOrder.Visible = false;
						this.btnModifyPrice.Visible = false;
					}
					this.btnModifyPrice.Attributes.Add("onclick", string.Concat(new string[]
					{
						"DialogFrame('../trade/EditOrder.aspx?OrderId=",
						this.orderId,
						"&reurl=",
						base.Server.UrlEncode(this.reurl),
						"','修改订单价格',900,450)"
					}));
					this.BindRemark(this.order);
					this.ddlpayment.DataBind();
					this.ddlpayment.SelectedValue = new int?(this.order.PaymentTypeId);
					this.rptItemList.DataSource = this.order.LineItems.Values;
					this.rptItemList.DataBind();
					string oldAddress = this.order.OldAddress;
					string text3 = string.Empty;
					if (!string.IsNullOrEmpty(this.order.ShippingRegion))
					{
						text3 = this.order.ShippingRegion.Replace('，', ' ');
					}
					if (!string.IsNullOrEmpty(this.order.Address))
					{
						text3 += this.order.Address;
					}
					if (!string.IsNullOrEmpty(this.order.ShipTo))
					{
						text3 = text3 + "，" + this.order.ShipTo;
					}
					if (!string.IsNullOrEmpty(this.order.TelPhone))
					{
						text3 = text3 + "，" + this.order.TelPhone;
					}
					if (!string.IsNullOrEmpty(this.order.CellPhone))
					{
						text3 = text3 + "，" + this.order.CellPhone;
					}
					if (string.IsNullOrEmpty(oldAddress))
					{
						this.lblOriAddress.Text = text3;
						this.pNewAddress.Visible = false;
					}
					else
					{
						this.lblOriAddress.Text = oldAddress;
						this.litAddress.Text = text3;
					}
					if (this.order.OrderStatus == OrderStatus.Finished || this.order.OrderStatus == OrderStatus.SellerAlreadySent)
					{
						string text4 = this.order.RealModeName;
						if (string.IsNullOrEmpty(text4))
						{
							text4 = this.order.ModeName;
						}
						this.litModeName.Text = text4;
						this.litShipOrderNumber.Text = this.order.ShipOrderNumber;
					}
					else
					{
						this.litModeName.Text = this.order.ModeName;
					}
					if (!string.IsNullOrEmpty(this.order.ExpressCompanyName))
					{
						this.litCompanyName.Text = this.order.ExpressCompanyName;
						this.hdCompanyCode.Value = this.order.ExpressCompanyAbb;
					}
					MemberInfo member = MemberProcessor.GetMember(this.order.UserId, true);
					if (member != null)
					{
						if (!string.IsNullOrEmpty(member.OpenId))
						{
							this.litWeiXinNickName.Text = member.UserName;
						}
						if (!string.IsNullOrEmpty(member.UserBindName))
						{
							this.litUserName.Text = member.UserBindName;
						}
					}
					if (this.order.ReferralUserId > 0)
					{
						stringBuilder = new System.Text.StringBuilder();
						stringBuilder.Append("<div class=\"commissionInfo mb20\"><h3>佣金信息</h3><div class=\"commissionInfoInner\">");
						decimal d3 = 0m;
						decimal num4 = 0m;
						decimal d4 = 0m;
						decimal d5 = 0m;
						if (this.order.OrderStatus != OrderStatus.Closed)
						{
							num4 = this.order.GetTotalCommssion();
							d4 = this.order.GetSecondTotalCommssion();
							d5 = this.order.GetThirdTotalCommssion();
						}
						d3 += num4;
						string text5 = string.Empty;
						DistributorsInfo distributorInfo2 = DistributorsBrower.GetDistributorInfo(this.order.ReferralUserId);
						if (distributorInfo2 != null)
						{
							text5 = distributorInfo2.StoreName;
							if (this.order.ReferralPath != null && this.order.ReferralPath.Length > 0)
							{
								string[] array = this.order.ReferralPath.Trim().Split(new char[]
								{
									'|'
								});
								if (array.Length > 1)
								{
									int num5 = Globals.ToNum(array[0]);
									if (num5 > 0)
									{
										distributorInfo2 = DistributorsBrower.GetDistributorInfo(num5);
										if (distributorInfo2 != null)
										{
											d3 += d5;
											stringBuilder.Append(string.Concat(new string[]
											{
												"<p class=\"mb5\"><span>上二级分销商：</span> ",
												distributorInfo2.StoreName,
												"<i> ￥",
												d5.ToString("F2"),
												"</i></p>"
											}));
										}
									}
									num5 = Globals.ToNum(array[1]);
									if (num5 > 0)
									{
										distributorInfo2 = DistributorsBrower.GetDistributorInfo(num5);
										if (distributorInfo2 != null)
										{
											d3 += d4;
											stringBuilder.Append(string.Concat(new string[]
											{
												"<p class=\"mb5\"><span>上一级分销商：</span> ",
												distributorInfo2.StoreName,
												"<i> ￥",
												d4.ToString("F2"),
												"</i></p>"
											}));
										}
									}
								}
								else if (array.Length == 1)
								{
									int num5 = Globals.ToNum(array[0]);
									if (num5 > 0)
									{
										distributorInfo2 = DistributorsBrower.GetDistributorInfo(num5);
										if (distributorInfo2 != null)
										{
											stringBuilder.Append("<p class=\"mb5\"><span>上二级分销商：</span>-</p>");
											d3 += d4;
											stringBuilder.Append(string.Concat(new string[]
											{
												"<p class=\"mb5\"><span>上一级分销商：</span>",
												distributorInfo2.StoreName,
												" <i> ￥",
												d4.ToString("F2"),
												"</i></p>"
											}));
										}
									}
								}
							}
							else
							{
								stringBuilder.Append("<p class=\"mb5\"><span>上二级分销商：</span>-</p>");
								stringBuilder.Append("<p class=\"mb5\"><span>上一级分销商：</span>-</p>");
							}
						}
						stringBuilder.Append("<div class=\"clearfix\">");
						if (num3 > 0m)
						{
							string text6 = " (改价让利￥" + num3.ToString("F2") + ")";
							stringBuilder.Append(string.Concat(new string[]
							{
								"<p><span>成交店铺：</span> ",
								text5,
								" <i>￥",
								(num4 - num3).ToString("F2"),
								"</i>",
								text6,
								"</p>"
							}));
							stringBuilder.Append("<p><span>佣金总额：</span><i>￥" + (d3 - num3).ToString("F2") + "</i></p>");
						}
						else
						{
							stringBuilder.Append(string.Concat(new string[]
							{
								"<p><span>成交店铺：</span> ",
								text5,
								" <i>￥",
								num4.ToString("F2"),
								"</i></p>"
							}));
							stringBuilder.Append("<p><span>佣金总额：</span><i>￥" + d3.ToString("F2") + "</i></p>");
						}
						stringBuilder.Append("</div></div></div>");
						this.litCommissionInfo.Text = stringBuilder.ToString();
					}
					System.Data.DataTable orderItemsReFundByOrderID = RefundHelper.GetOrderItemsReFundByOrderID(this.orderId);
					if (orderItemsReFundByOrderID.Rows.Count > 0)
					{
						this.rptRefundList.DataSource = orderItemsReFundByOrderID;
						this.rptRefundList.DataBind();
						return;
					}
				}
			}
			else
			{
				base.Response.Write("原订单已删除！");
				base.Response.End();
			}
		}

		private void LoadUserControl(OrderInfo order)
		{
		}

		private void btnMondifyPay_Click(object sender, System.EventArgs e)
		{
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			if (this.ddlpayment.SelectedValue.HasValue && this.ddlpayment.SelectedValue == -1)
			{
				this.order.PaymentTypeId = 0;
				this.order.PaymentType = "货到付款";
				this.order.Gateway = "hishop.plugins.payment.podrequest";
			}
			else if (this.ddlpayment.SelectedValue.HasValue && this.ddlpayment.SelectedValue == 99)
			{
				this.order.PaymentTypeId = 99;
				this.order.PaymentType = "线下付款";
				this.order.Gateway = "hishop.plugins.payment.offlinerequest";
			}
			else if (this.ddlpayment.SelectedValue.HasValue && this.ddlpayment.SelectedValue == 66)
			{
				this.order.PaymentTypeId = 66;
				this.order.PaymentType = "余额支付";
				this.order.Gateway = "hishop.plugins.payment.balancepayrequest";
			}
			else if (this.ddlpayment.SelectedValue.HasValue && this.ddlpayment.SelectedValue == 77)
			{
				this.order.PaymentTypeId = 77;
				this.order.PaymentType = "积分抵现";
				this.order.Gateway = "hishop.plugins.payment.pointtocash";
			}
			else if (this.ddlpayment.SelectedValue.HasValue && this.ddlpayment.SelectedValue == 88)
			{
				this.order.PaymentTypeId = 88;
				this.order.PaymentType = "微信支付";
				this.order.Gateway = "hishop.plugins.payment.weixinrequest";
			}
			else
			{
				PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(this.ddlpayment.SelectedValue.Value);
				this.order.PaymentTypeId = paymentMode.ModeId;
				this.order.PaymentType = paymentMode.Name;
				this.order.Gateway = paymentMode.Gateway;
			}
			if (OrderHelper.UpdateOrderPaymentType(this.order))
			{
				this.ShowMsgAndReUrl("修改支付方式成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + System.DateTime.Now.ToString("HHmmss"));
				return;
			}
			this.ShowMsg("修改支付方式失败", false);
		}

		private void btnCloseOrder_Click(object sender, System.EventArgs e)
		{
			this.order.CloseReason = this.ddlCloseReason.SelectedValue;
			if ("请选择关闭的理由" == this.order.CloseReason)
			{
				this.ShowMsg("请选择关闭的理由", false);
				return;
			}
			if (OrderHelper.CloseTransaction(this.order))
			{
				this.order.OnClosed();
				this.ShowMsgAndReUrl("关闭订单成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + System.DateTime.Now.ToString("HHmmss"));
				return;
			}
			this.ShowMsg("关闭订单失败", false);
		}

		private void btnRemark_Click(object sender, System.EventArgs e)
		{
			if (this.txtRemark.Text.Length > 300)
			{
				this.ShowMsg("备注长度限制在300个字符以内", false);
				return;
			}
			this.order.OrderId = this.orderId;
			if (this.orderRemarkImageForRemark.SelectedItem != null)
			{
				this.order.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
			}
			this.order.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
			if (OrderHelper.SaveRemark(this.order))
			{
				this.BindRemark(this.order);
				this.ShowMsgAndReUrl("保存备注成功", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + System.DateTime.Now.ToString("HHmmss"));
				return;
			}
			this.ShowMsg("保存失败", false);
		}

		private void BindRemark(OrderInfo order)
		{
			this.spanOrderId.Text = order.OrderId;
			this.lblorderDateForRemark.Time = order.OrderDate;
			this.lblorderTotalForRemark.Money = order.GetTotal();
			this.txtRemark.Text = Globals.HtmlDecode(order.ManagerRemark);
			this.orderRemarkImageForRemark.SelectedValue = order.ManagerMark;
		}

		protected void btnConfirmPay_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (orderInfo != null && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
			{
				if (OrderHelper.ConfirmPay(orderInfo))
				{
					DebitNoteInfo debitNoteInfo = new DebitNoteInfo();
					debitNoteInfo.NoteId = Globals.GetGenerateId();
					debitNoteInfo.OrderId = this.orderId;
					debitNoteInfo.Operator = ManagerHelper.GetCurrentManager().UserName;
					debitNoteInfo.Remark = "后台" + debitNoteInfo.Operator + "收款成功";
					OrderHelper.SaveDebitNote(debitNoteInfo);
					orderInfo.OnPayment();
					this.ShowMsgAndReUrl("成功的确认了订单收款", true, "OrderDetails.aspx?OrderId=" + this.orderId + "&t=" + System.DateTime.Now.ToString("HHmmss"));
					return;
				}
				this.ShowMsg("确认订单收款失败", false);
			}
		}

		private void CloseOrder(string orderid)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderid);
			orderInfo.CloseReason = "客户要求退货(款)！";
			if (RefundHelper.CloseTransaction(orderInfo))
			{
				orderInfo.OnClosed();
				MemberHelper.GetMember(orderInfo.UserId);
			}
		}

		protected void rptRefundList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnAgree");
				System.Web.UI.HtmlControls.HtmlInputButton htmlInputButton2 = (System.Web.UI.HtmlControls.HtmlInputButton)e.Item.FindControl("btnRefuce");
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lblIsAgree");
				RefundInfo.Handlestatus handlestatus = (RefundInfo.Handlestatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "HandleStatus");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("linkModify");
				switch (handlestatus)
				{
				case RefundInfo.Handlestatus.Applied:
					htmlInputButton.Visible = false;
					htmlInputButton2.Visible = false;
					label.Visible = true;
					label.Text = "已申请";
					htmlAnchor.Visible = false;
					return;
				case RefundInfo.Handlestatus.Refunded:
					htmlInputButton.Visible = false;
					htmlInputButton2.Visible = false;
					label.Visible = true;
					label.Text = "已退款";
					htmlAnchor.Visible = false;
					return;
				case RefundInfo.Handlestatus.Refused:
					htmlInputButton.Visible = false;
					htmlInputButton2.Visible = false;
					label.Visible = true;
					label.Text = "拒绝申请";
					htmlAnchor.Visible = false;
					return;
				case RefundInfo.Handlestatus.NoneAudit:
				case RefundInfo.Handlestatus.HasTheAudit:
				case RefundInfo.Handlestatus.NoRefund:
					break;
				case RefundInfo.Handlestatus.AuditNotThrough:
					htmlInputButton.Visible = false;
					htmlInputButton2.Visible = false;
					label.Visible = true;
					label.Text = "审核不通过";
					htmlAnchor.Visible = false;
					break;
				case RefundInfo.Handlestatus.RefuseRefunded:
					htmlInputButton.Visible = false;
					htmlInputButton2.Visible = false;
					label.Visible = true;
					label.Text = "拒绝退款";
					htmlAnchor.Visible = false;
					return;
				default:
					return;
				}
			}
		}

		protected void btnAgreeConfirm_Click(object sender, System.EventArgs e)
		{
			decimal num = 0m;
			decimal.TryParse(this.txtMoney.Text.Trim(), out num);
			decimal num2 = 0m;
			int num3 = 0;
			int num4 = Globals.ToNum(this.hdProductID.Value);
			int returnsid = Globals.ToNum(this.hdReturnsId.Value);
			this.hdSkuID.Value.Trim();
			if (num4 > 0)
			{
				decimal num5 = 0m;
				decimal.TryParse(Globals.RequestFormStr("ctl00$ContentPlaceHolder1$hdBalance"), out num5);
				RefundInfo orderReturnsByReturnsID = RefundHelper.GetOrderReturnsByReturnsID(returnsid);
				if (orderReturnsByReturnsID != null)
				{
					orderReturnsByReturnsID.AdminRemark = this.txtMemo.Text.Trim();
					orderReturnsByReturnsID.HandleTime = System.DateTime.Now;
					orderReturnsByReturnsID.RefundTime = System.DateTime.Now.ToString();
					orderReturnsByReturnsID.HandleStatus = RefundInfo.Handlestatus.Refunded;
					orderReturnsByReturnsID.Operator = Globals.GetCurrentManagerUserId().ToString();
					if (num >= 0m)
					{
						orderReturnsByReturnsID.RefundMoney = num;
						orderReturnsByReturnsID.RefundId = orderReturnsByReturnsID.ReturnsId;
						orderReturnsByReturnsID.BalanceReturnMoney = num5;
						if (RefundHelper.UpdateByReturnsId(orderReturnsByReturnsID))
						{
							OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderReturnsByReturnsID.OrderId);
							num2 = orderInfo.BalancePayMoneyTotal;
							if (num > orderInfo.BalancePayMoneyTotal)
							{
								orderInfo.BalancePayMoneyTotal = 0m;
								orderInfo.BalancePayFreightMoneyTotal = 0m;
								using (System.Collections.Generic.Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator = orderInfo.LineItems.Values.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										LineItemInfo current = enumerator.Current;
										current.BalancePayMoney = 0m;
									}
									goto IL_3FA;
								}
							}
							if (num >= num5 && num <= orderInfo.BalancePayMoneyTotal)
							{
								orderInfo.BalancePayMoneyTotal -= num;
								orderInfo.BalancePayFreightMoneyTotal = ((orderInfo.BalancePayFreightMoneyTotal - (num - num5) <= 0m) ? 0m : (orderInfo.BalancePayFreightMoneyTotal - (num - num5)));
								decimal num6 = (num - num5 - orderInfo.BalancePayFreightMoneyTotal <= 0m) ? 0m : (num - num5 - orderInfo.BalancePayFreightMoneyTotal);
								using (System.Collections.Generic.Dictionary<string, LineItemInfo>.ValueCollection.Enumerator enumerator2 = orderInfo.LineItems.Values.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										LineItemInfo current2 = enumerator2.Current;
										if ((!string.IsNullOrEmpty(orderReturnsByReturnsID.SkuId) && current2.SkuId == orderReturnsByReturnsID.SkuId) || (current2.ProductId == orderReturnsByReturnsID.ProductId && string.IsNullOrEmpty(current2.SkuId)))
										{
											current2.BalancePayMoney = 0m;
										}
										else if (num6 > 0m && current2.BalancePayMoney > 0m)
										{
											current2.BalancePayMoney = ((current2.BalancePayMoney <= num6) ? 0m : (current2.BalancePayMoney - num6));
											num6 -= current2.BalancePayMoney;
										}
									}
									goto IL_3FA;
								}
							}
							if (num < num5)
							{
								orderInfo.BalancePayMoneyTotal -= num;
								foreach (LineItemInfo current3 in orderInfo.LineItems.Values)
								{
									if ((!string.IsNullOrEmpty(orderReturnsByReturnsID.SkuId) && current3.SkuId == orderReturnsByReturnsID.SkuId) || (current3.ProductId == orderReturnsByReturnsID.ProductId && string.IsNullOrEmpty(current3.SkuId)))
									{
										current3.BalancePayMoney -= num;
									}
								}
							}
							IL_3FA:
							string text = null;
							string stock = null;
							foreach (LineItemInfo current4 in orderInfo.LineItems.Values)
							{
								if ((!string.IsNullOrEmpty(orderReturnsByReturnsID.SkuId) && current4.SkuId == orderReturnsByReturnsID.SkuId) || (current4.ProductId == orderReturnsByReturnsID.ProductId && string.IsNullOrEmpty(current4.SkuId)))
								{
									text = current4.SkuId;
									stock = current4.Quantity.ToString();
									current4.OrderItemsStatus = OrderStatus.Refunded;
									break;
								}
							}
							if (!RefundHelper.UpdateOrderGoodStatu(this.hdfOrderID.Value, text, 9, orderReturnsByReturnsID.OrderItemID))
							{
								return;
							}
							RefundHelper.UpdateRefundOrderStock(stock, text);
							foreach (LineItemInfo current5 in orderInfo.LineItems.Values)
							{
								if (current5.OrderItemsStatus.ToString() == OrderStatus.Refunded.ToString() || current5.OrderItemsStatus.ToString() == OrderStatus.Returned.ToString())
								{
									num3++;
								}
							}
							OrderHelper.UpdateOrderAmount(orderInfo);
							if (orderInfo.LineItems.Values.Count == num3)
							{
								this.CloseOrder(this.hdfOrderID.Value);
								orderInfo.OrderStatus = OrderStatus.Closed;
							}
							OrderHelper.UpdateCalculadtionCommission(this.hdfOrderID.Value);
							int orderItemID = orderReturnsByReturnsID.OrderItemID;
							decimal num7 = 0m;
							if (orderItemID > 0)
							{
								LineItemInfo lineItemInfo = OrderSplitHelper.GetLineItemInfo(orderItemID, "");
								if (lineItemInfo != null)
								{
									num7 = ((num > num2) ? num2 : num);
								}
								if (num7 > 0m)
								{
									orderReturnsByReturnsID = RefundHelper.GetOrderReturnsByReturnsID(returnsid);
									RefundHelper.UpdateByReturnsId(orderReturnsByReturnsID);
									OrderHelper.MemberAmountAddByRefund(MemberHelper.GetMember(orderInfo.UserId), decimal.Round(num7, 2), this.hdfOrderID.Value);
									OrderHelper.UpdateOrder(orderInfo);
									OrderHelper.UpdateOrderItems(orderInfo);
								}
							}
							try
							{
								Messenger.SendWeiXinMsg_RefundSuccess(orderReturnsByReturnsID);
							}
							catch (System.Exception ex)
							{
								Globals.Debuglog("订单退款成功提醒消息异常" + ex.ToString(), "_DebuglogSendMsgRefund.txt");
							}
							this.ShowMsgAndReUrl("同意退款成功!", true, "OrderDetails.aspx?OrderId=" + this.hdfOrderID.Value + "&t=" + System.DateTime.Now.ToString("HHmmss"));
							try
							{
								this.myNotifier.updateAction = UpdateAction.OrderUpdate;
								this.myNotifier.actionDesc = "同意退款成功";
								this.myNotifier.RecDateUpdate = (orderInfo.PayDate.HasValue ? orderInfo.PayDate.Value : System.DateTime.Today);
								this.myNotifier.DataUpdated += new StatisticNotifier.DataUpdatedEventHandler(this.myEvent.Update);
								this.myNotifier.UpdateDB();
								return;
							}
							catch (System.Exception ex2)
							{
								Globals.Debuglog(ex2.Message, "_Debuglog.txt");
								return;
							}
						}
						this.ShowMsg("退款失败，请重试。", false);
						return;
					}
					this.ShowMsg("输入的金额格式不正确", false);
					return;
				}
			}
			else
			{
				this.ShowMsg("服务器错误，请刷新页面重试！", false);
			}
		}

		protected void btnRefuseConfirm_Click(object sender, System.EventArgs e)
		{
			int num = Globals.ToNum(this.hdProductID.Value);
			this.hdSkuID.Value.Trim();
			Globals.ToNum(this.hdOrderItemId.Value);
			int returnsid = Globals.ToNum(this.hdReturnsId.Value);
			if (num > 0)
			{
				RefundInfo orderReturnsByReturnsID = RefundHelper.GetOrderReturnsByReturnsID(returnsid);
				orderReturnsByReturnsID.RefundId = orderReturnsByReturnsID.ReturnsId;
				orderReturnsByReturnsID.AdminRemark = this.txtAdminMemo.Text.Trim();
				orderReturnsByReturnsID.HandleTime = System.DateTime.Now;
				orderReturnsByReturnsID.HandleStatus = RefundInfo.Handlestatus.RefuseRefunded;
				orderReturnsByReturnsID.Operator = Globals.GetCurrentManagerUserId().ToString();
				orderReturnsByReturnsID.BalanceReturnMoney = 0m;
				if (!RefundHelper.UpdateByReturnsId(orderReturnsByReturnsID))
				{
					this.ShowMsg("操作失败，请重试。", false);
					return;
				}
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hdfOrderID.Value);
				string skuid = null;
				foreach (LineItemInfo current in orderInfo.LineItems.Values)
				{
					if (current.ProductId == num)
					{
						skuid = current.SkuId;
						OrderStatus arg_FC_0 = current.OrderItemsStatus;
						break;
					}
				}
				if (RefundHelper.UpdateOrderGoodStatu(this.hdfOrderID.Value, skuid, 3, orderReturnsByReturnsID.OrderItemID))
				{
					this.ShowMsgAndReUrl("拒绝退款成功!", true, "OrderDetails.aspx?OrderId=" + this.hdfOrderID.Value + "&t=" + System.DateTime.Now.ToString("HHmmss"));
					return;
				}
			}
			else
			{
				this.ShowMsg("服务器错误，请刷新页面重试！", false);
			}
		}
	}
}
