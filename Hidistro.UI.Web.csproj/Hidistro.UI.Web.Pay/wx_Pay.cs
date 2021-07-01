using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Domain;
using Hishop.Weixin.Pay.Notify;
using jos_sdk_net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wx_Pay : System.Web.UI.Page
	{
		protected System.Collections.Generic.List<OrderInfo> orderlist;
        
		protected string OrderId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                Globals.Debuglog("开始执行微信支付回调", "_Debuglog.txt");
                SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
                NotifyClient notifyClient;
                if (masterSettings.EnableSP)
                {
                    notifyClient = new NotifyClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
                }
                else
                {
                    notifyClient = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
                }
                PayNotify payNotify = notifyClient.GetPayNotify(base.Request.InputStream);
                if (payNotify == null)
                {
                    return;
                }
                this.OrderId = payNotify.PayInfo.OutTradeNo;
                #region 订单上送京东
                List<OrderInfo> selectResult = new List<OrderInfo>();
                Globals.Debuglog("打印回调订单OrderMarking：" + this.OrderId, "_JDDebuglog.txt");
                string sql = @" SELECT * FROM [dbo].[Hishop_Orders] WHERE OrderMarking='"+this.OrderId+"'";
                //string sql = @" SELECT * FROM [dbo].[Hishop_Orders] WHERE PayDate is not null and  PayDate > '2019-05-10 16:34:00' AND OrderStatus = 2  ORDER BY OrderDate DESC";
                List<OrderInfo> order = new List<OrderInfo>();
                order = connection.GetOrders(sql);
                Globals.Debuglog("获取完成订单号条数："+ order.Count.ToString(), "_JDDebuglog.txt");
                List<OrderInfo> Orders = new List<OrderInfo>();
                List<OrderInfo> UsingOrder = new List<OrderInfo>();
                string sql2 = @"SELECT ProductId,SKU,Quantity,ItemDescription FROM [dbo].[Hishop_OrderItems] WHERE OrderId =@OrderID";
                Orders = connection.GetProductID(sql2,order[0].OrderId.ToString());
                if (Orders.Count > 0 && order.Count > 0)
                {
                    for (int j = 0; j < Orders.Count; j++)
                    {
                        OrderInfo o = new OrderInfo();
                        o.OrderId = order[0].OrderId.ToString();
                        o.Username = order[0].ShipTo.ToString();
                        o.CellPhone = order[0].CellPhone.ToString();
                        o.ShippingRegion = order[0].ShippingRegion.ToString();
                        o.Remark = order[0].Remark.ToString();
                        o.Address = order[0].Address.ToString();
                        o.SKU = Orders[j].SKU.ToString();
                        o.ProductId = Orders[j].ProductId;
                        o.Quantity = Orders[j].Quantity.ToString();
                        o.OrderDate = order[0].OrderDate;
                        //获取产品信息（productCode）
                        string sql3 = @"SELECT ProductCode FROM [dbo].[Hishop_Products] WHERE ProductId=@ProductID";
                        o.ProductCode = connection.GetProductDetails(sql3, Orders[j].ProductId.ToString());
                        //获取产品tag
                        string sql4 = @"SELECT * FROM [dbo].[Hishop_ProductTag] WHERE ProductId =@ProductID";
                        o.Tag = connection.GetProductTag(sql4, Orders[j].ProductId.ToString());
                        if (o.Tag == 3)
                        {
                            UsingOrder.Add(o);
                        }
                    }
                }
                else {
                    Globals.Debuglog("微信回调未查询到该笔订单：" + order[0].OrderId.ToString(), "_JDDebuglog.txt");
                    return;
                }
                string sqlCheck = @" select * FROM [dbo].[Hishop_OrderAlreadySend] where OrderID=@OrderID";
                selectResult = connection.GetProductCheckResult(sqlCheck, order[0].OrderId.ToString());
                //若查询结果为空则继续
                if (selectResult.Count > 0)
                {
                    //记录日志
                    Globals.Debuglog("该订单号已提交过京东，请勿重复提交！订单号：" + order[0].OrderId.ToString(), "_JDDebuglog.txt");
                    return ;
                }
                if (UsingOrder.Count > 0)
                {
                    bool res = SendOrder.GetOrders(UsingOrder);
                    if (res != true)
                    {
                        Globals.Debuglog("京东订单提交失败，订单号：" + order[0].OrderId.ToString(), "_JDDebuglog.txt");
                        return;
                    }
                }
                #endregion
                if (this.OrderId.StartsWith("B"))
                {
                    this.DoOneTao(this.OrderId, payNotify.PayInfo);
                    base.Response.End();
                }
                this.orderlist = ShoppingProcessor.GetOrderMarkingOrderInfo(this.OrderId, true);
                if (this.orderlist.Count == 0)
                {
                    base.Response.Write("success");
                    return;
                }
                foreach (OrderInfo current in this.orderlist)
                {
                    current.GatewayOrderId = payNotify.PayInfo.TransactionId;
                }
                this.UserPayOrder();
            }
            catch (Exception err)
            {
                string errs = err.Message.ToString();
                Globals.Debuglog("微信回调PageLoad报错："+errs, "_Debuglog.txt");

            }
		}

		private void DoOneTao(string Pid, PayInfo PayInfo)
		{
            try
            {
                OneyuanTaoParticipantInfo addParticipant = OneyuanTaoHelp.GetAddParticipant(0, Pid, "");
                if (addParticipant == null)
                {
                    base.Response.Write("success");
                    return;
                }
                addParticipant.PayTime = new System.DateTime?(System.DateTime.Now);
                addParticipant.PayWay = "weixin";
                addParticipant.PayNum = Pid;
                addParticipant.Remark = "订单已支付：支付金额为￥" + PayInfo.TotalFee.ToString();
                if (!addParticipant.IsPay && OneyuanTaoHelp.SetPayinfo(addParticipant))
                {
                    OneyuanTaoHelp.SetOneyuanTaoFinishedNum(addParticipant.ActivityId, 0);
                }
                else
                {
                    Globals.Debuglog(JsonConvert.SerializeObject(PayInfo), "_Debuglog.txt");
                }
                base.Response.Write("success");
            }
            catch (Exception err)
            {
                Globals.Debuglog("微信回调DoOneTao报错：" + err.Message.ToString(), "_Debuglog.txt");
            }
		}

		private void UserPayOrder()
		{
            try
            {
                foreach (OrderInfo current in this.orderlist)
                {
                    if (current.OrderStatus == OrderStatus.BuyerAlreadyPaid)
                    {
                        base.Response.Write("success");
                        return;
                    }
                }
                foreach (OrderInfo current2 in this.orderlist)
                {
                    if (current2.CheckAction(OrderActions.BUYER_PAY) && MemberProcessor.UserPayOrder(current2))
                    {
                        current2.OnPayment();
                        base.Response.Write("success");
                    }
                }
            }
            catch (Exception errsa)
            {
                Globals.Debuglog("微信回调UserPayOrder报错：" + errsa.Message.ToString(), "_Debuglog.txt");
            }
		}
	}
}
