using Hidistro.Core;
using Hidistro.Entities.Orders;
using Jd.Api;
using Jd.Api.Jos;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;

namespace jos_sdk_net
{

       
    /// <summary>
    /// 发送京东订单
    /// </summary>
    public class SendOrder
    {
        private static DefaultJdClient JdClient;
        public static string url = ConfigurationManager.AppSettings["ServerUrl"].ToString();

        public static string appKey = ConfigurationManager.AppSettings["appKey"].ToString();

        public static string appSecret = ConfigurationManager.AppSettings["appSecret"].ToString();

        public static string accessToken = ConfigurationManager.AppSettings["accessToken"].ToString();

        /// <summary>
        /// 获取需要提交的订单
        /// </summary>
        /// <param name="SelectResult"></param>
        /// <returns></returns>
        public static bool GetOrders(List<OrderInfo> SelectResult)
        {
            bool result = false;

            try
            {
                #region 获取订单详情
                //记录日志
                Globals.Debuglog("购物车提交订单数量：" + SelectResult.Count.ToString(), "_JDDebuglog.txt");
                if (SelectResult.Count != 0)
                {
                    string Addres = "0";
                    for (int y = 0; y < SelectResult.Count; y++)
                    {
                        //识别已提交订单并且退出循环
                        if (Addres == SelectResult[y].OrderId)
                            continue;
                        //找到该提交的订单
                        //if (CheckOrder[x].OrderID == SelectResult[y].OrderId)
                        //{
                        Addres = SelectResult[y].OrderId;
                        string quantity = "";
                        string productCode = "";
                        string SKU = "";
                        #region 加入参数

                        OrderInfo ord = new OrderInfo();
                        ord.Tag = SelectResult[y].Tag;
                        ord.OrderId = SelectResult[y].OrderId.ToString();
                        ord.Username = SelectResult[y].Username.ToString();
                        ord.CellPhone = SelectResult[y].CellPhone.ToString();
                        ord.ShippingRegion = SelectResult[y].ShippingRegion.ToString();
                        ord.Remark = SelectResult[y].Remark.ToString();
                        ord.Address = SelectResult[y].Address.ToString();
                        ord.ProductId = SelectResult[y].ProductId;
                        #endregion
                        //判断找到的订单是否需要合单
                        //if (CombineOrder.Exists(v1 => v1.OrderID == SelectResult[y].OrderId))
                        if (SelectResult.Count > 1)
                        {
                            quantity = "";
                            productCode = "";
                            SKU = "";
                            for (int i = 0; i < SelectResult.Count; i++)
                            {
                                if (SelectResult[i].OrderId == SelectResult[y].OrderId)
                                {
                                    if (!string.IsNullOrEmpty(SelectResult[i].Quantity.ToString()))
                                    { quantity += SelectResult[i].Quantity.ToString() + ","; }
                                    if (!string.IsNullOrEmpty(SelectResult[i].ProductCode.ToString()))
                                    { productCode += SelectResult[i].ProductCode.ToString() + ","; }
                                    if (!string.IsNullOrEmpty(SelectResult[i].SKU.ToString()))
                                    { SKU += SelectResult[i].SKU.ToString() + ","; }
                                }
                            }
                            if (!string.IsNullOrEmpty(quantity))
                            {
                                quantity = quantity.Remove(quantity.LastIndexOf(","), 1);
                            }
                            if (!string.IsNullOrEmpty(productCode))
                                productCode = productCode.Remove(productCode.LastIndexOf(","), 1);
                            if (!string.IsNullOrEmpty(SKU))
                                SKU = SKU.Remove(SKU.LastIndexOf(","), 1);
                            if (!string.IsNullOrEmpty(SKU) && !string.IsNullOrEmpty(productCode))
                            {
                                productCode = productCode + "," + SKU;
                                SKU = "";
                            }
                        }
                        else
                        {
                            quantity = SelectResult[y].Quantity; ;
                            productCode = SelectResult[y].ProductCode; ;
                            SKU = SelectResult[y].SKU;
                        }
                        ord.SKU = SKU;
                        ord.ProductCode = productCode;
                        ord.Quantity = quantity;
                        ord.OrderDate = SelectResult[y].OrderDate;
                        //打印发送详情
                        Globals.Debuglog("打印发送详情;SKU:" + ord.SKU + "；ProductCode：" + ord.ProductCode + "；Quantity：" + ord.Quantity, "_JDDebuglog.txt");
                        //发送到京东
                        if (SendJdClient(ord) == true)
                        {
                            result = true;
                        }
                    }

                }
                else
                {
                    //记录日志
                    Globals.Debuglog("无订单可提交", "_JDDebuglog.txt");
                    result = false;
                }
                #endregion
            }
            catch (Exception err)
            {
                string errs = err.Message.ToString();  
                //记录日志
                Globals.Debuglog("京东获取订单错误："+ errs, "_JDDebuglog.txt");
                return false;

            }
            return result;
        }
    

        /// <summary>
        /// 发送至京东接口
        /// </summary>
        /// <param name="ord"></param>
        public static bool SendJdClient(OrderInfo ord)
        {
            bool res = false;
            try
            {
                IDictionary<String, String> pList = GetParameters(ord);
                string method = GetApiName();
                string access_token = accessToken;
                string app_key = appKey;
                string app_secret = appSecret;
                //编辑签名
                string sign = Jd.Api.Util.JdUtils.SignJdRequest(pList, appSecret, true);

                // 添加协议级(系统级)请求参数
                JdClient = new DefaultJdClient(url, appKey, appSecret);

                string Now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //时间戳（int类型无法使用，写死100000）
                long timeNow = Convert.ToInt64(Jd.Api.Util.JdUtils.GetCurrentTimeMillis());

                //访问提交参数
                JosJdClient d = new JosJdClient(url, appKey, appSecret, 100000);
                string result = d.execute(method, pList, access_token);
                Globals.Debuglog("SendJdClient方法result结果："+result, "_JDDebuglog.txt");
                JToken tk = JObject.Parse(result);
                JToken tks = (JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                string addOrderResponse = tk["jingdong_eclp_order_addOrder_responce"].ToString();
                if (!string.IsNullOrEmpty(addOrderResponse))
                {
                    if (string.IsNullOrEmpty(addOrderResponse))
                    {
                        //参数返回异常
                        Globals.Debuglog("SendJdClient方法参数返回异常", "_JDDebuglog.txt");
                    }
                    JToken ty = JObject.Parse(addOrderResponse);
                    //logs.Writelog("第一次JTOKEN开始；ty结果：" + ty, "2");
                    if (ty["code"].ToString() != "0")
                    {
                        //返回状态非正常
                        Globals.Debuglog("SendJdClient方法返回状态非正常", "_JDDebuglog.txt");

                    }
                    //前端提示订单提交已经成功
                    Globals.Debuglog("订单提交已经成功！订单号：" + ord.OrderId, "_JDDebuglog.txt");
                    //Response.Write("<script>alert('订单提交成功！')</script>");
                    //获取订单号用于查单
                    string SelectNo = ty["eclpSoNo"].ToString();
                    //查单操作
                    if (!string.IsNullOrEmpty(SelectNo))
                    {
                        int xsa = GetJDVDOrders(SelectNo, ord.OrderId);
                        if (xsa != 0)
                        {
                            res = true;
                        }
                    }

                }
               
            }
            catch (Exception er)
            {
                //订单提交失败报错提示
                string err = er.Message.ToString();

                //记录日志
                Globals.Debuglog("SendJdClient方法catch订单提交失败报错："+err, "_JDDebuglog.txt");
                res = false;
            }
            return res;
        }

        /// <summary>
        /// 查询京东订单号
        /// </summary>
        /// <param name="OrdersSelectNo"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static int GetJDVDOrders(string OrdersSelectNo, string OrderID)
        {
            Thread.Sleep(10000);
            int jdvdOrders = 0;
            try
            {
                string Selectmethod = GetApiSelectName();
                IDictionary<String, String> pSelectList = GetSelectParameters(OrdersSelectNo);
                JosJdClient dx = new JosJdClient(url, appKey, appSecret, 100000);
                string Selectresult = dx.execute(Selectmethod, pSelectList, accessToken);
                //获取订单查询结果
                JToken Selecttk = JObject.Parse(Selectresult);
                // logs.Writelog("订单查询返回结果：" + Selecttk, "2");
                string SelectOrderResponse = Selecttk["jingdong_eclp_order_queryOrder_responce"].ToString();
                if (!string.IsNullOrEmpty(SelectOrderResponse))
                {

                    JToken tySelect = JObject.Parse(SelectOrderResponse);
                    // logs.Writelog("第3次JTOKEN开始；tySelect结果：" + tySelect, "2");
                    string queryorder_result = tySelect["queryorder_result"].ToString();
                    if (!string.IsNullOrEmpty(queryorder_result))
                    {
                        JToken queryOrderObject = JObject.Parse(queryorder_result);
                        // logs.Writelog("第四次JTOKEN开始；queryOrderObject结果：" + queryOrderObject, "2");
                        string wak23yBillResponseResult = queryOrderObject["wayBill"].ToString();
                        //返单操作业务
                        // ****************************
                        // 用到的数据：订单号；京东返回单号
                        //把已经上送京东订单号加入新数据表防止重复提交
                        Globals.Debuglog("把已经上送京东订单号加入新数据表防止重复提交，新增订单号：" + OrderID + ",京东运单号：" + wak23yBillResponseResult, "_JDDebuglog.txt");
                        string sql = @" INSERT INTO [dbo].[hishop_OrderAlreadySend] VALUES(@OrderID,@wayBill)";
                        int SelectResult = connection.InsertAddOrders(sql, OrderID, wak23yBillResponseResult);
                        Globals.Debuglog("Hishop_OrderAlreadySend插入数据条数：" + SelectResult.ToString(), "_JDDebuglog.txt");
                        jdvdOrders = SelectResult;
                    }
                }
            }
            catch (Exception errr)
            {
                Globals.Debuglog("京东查单出错，正在尝试重新查询.............", "_JDDebuglog.txt");
            }

            return jdvdOrders;
        }
        public static string GetApiName()
        {
            return "jingdong.eclp.order.addOrder";
        }
        public static string GetApiSelectName()
        {
            return "jingdong.eclp.order.queryOrder";
        }
        public static IDictionary<string, string> GetSelectParameters(string eclpSoNo)
        {
            JdDictionary parameters = new JdDictionary();
            parameters.Add("eclpSoNo", eclpSoNo);
            parameters.AddAll(otherParameters);
            return parameters;
        }
        private static IDictionary<string, string> otherParameters;

        //加载参数
        public static IDictionary<string, string> GetParameters(OrderInfo orders)
        {

            JdDictionary parameters = new JdDictionary();
            string Address = orders.ShippingRegion.Replace("，", " ");
            Address += " " + orders.Address;
            parameters.Add("isvUUID", orders.OrderId
);
            parameters.Add("shopNo", "ESP0020000047160"
);
            parameters.Add("departmentNo", "EBU4418046568265"
);
            parameters.Add("warehouseNo", "110008721"
);
            parameters.Add("shipperNo", "CYS0000010"
);
            parameters.Add("salesPlatformOrderNo", ""
);
            parameters.Add("salePlatformSource", "6"
);
            parameters.Add("salesPlatformCreateTime", orders.OrderDate.ToString("yyyy-MM-dd HH:mm:ss")
);
            parameters.Add("soType", "1"
);
            parameters.Add("quantity", orders.Quantity.ToString()
);
            if (string.IsNullOrEmpty(orders.SKU))
            {
                parameters.Add("goodsNo", orders.ProductCode
);
            }
            else
            {
                parameters.Add("goodsNo", orders.SKU
);
            }
            parameters.Add("consigneeName", orders.Username
);
            parameters.Add("consigneeMobile", orders.CellPhone
);
            parameters.Add("consigneePhone", ""
);
            parameters.Add("consigneeEmail", ""
);
            parameters.Add("expectDate", ""
);
            parameters.Add("addressProvince", ""
);
            parameters.Add("addressCity", ""
);
            parameters.Add("addressCounty", ""
);
            parameters.Add("addressTown", ""
);
            parameters.Add("consigneeAddress", Address
);
            parameters.Add("consigneePostcode", ""
);
            parameters.Add("amount", ""
);
            parameters.Add("orderMark", "00000000000000000000000000000000000000000000000000"
);




            parameters.Add("isvSource", "ISV0020000000513"
);
            parameters.Add("receivable", ""
);
            parameters.Add("consigneeRemark", ""
);
            parameters.Add("setThirdWayBill", ""
);//三方运单号
            parameters.Add("bdOwnerNo", "028K304184"
);
            parameters.Add("packageMark", ""
);
            parameters.Add("businessType", ""
);
            parameters.Add("destinationCode", ""
);
            parameters.Add("destinationName", ""
);
            parameters.Add("sendWebsiteCode", ""
);
            parameters.Add("sendWebsiteName", ""
);
            parameters.Add("sendMode", ""
);
            parameters.Add("receiveMode", ""
);
            parameters.Add("appointDeliveryTime", ""
);
            parameters.Add("insuredPriceFlag", ""
);
            parameters.Add("insuredValue", ""
);
            parameters.Add("thirdPayment", ""
);
            parameters.Add("monthlyAccount", ""
);
            parameters.Add("shipment", ""
);
            parameters.Add("sellerRemark", ""
);
            parameters.Add("thirdSite", ""
);
            parameters.Add("gatherCenterName", ""
);
            parameters.Add("customsStatus", ""
);
            parameters.Add("customerName", ""
);
            parameters.Add("invoiceTitle", ""
);
            parameters.Add("iinvoiceContent", ""
);
            parameters.Add("goodsType", ""
);
            parameters.Add("goodsLevel", ""
);
            parameters.Add("customsPort", ""
);
            parameters.Add("billType", ""
);
            parameters.Add("orderPrice", ""
);
            parameters.Add("wlyInfo", ""
);
            parameters.Add("customerId", ""
);
            parameters.Add("urgency", ""
);
            parameters.Add("customerNo", ""
);
            parameters.Add("storeName", ""
);
            parameters.Add("invoiceState", ""
);
            parameters.Add("invoiceType", ""
);
            parameters.Add("invoiceNo", ""
);
            parameters.Add("invoiceTax", ""
);
            parameters.Add("bankName", ""
);
            parameters.Add("bankAccount", ""
);
            parameters.Add("address", ""
);
            parameters.Add("phoneNumber", ""
);
            parameters.Add("signType", ""
);
            parameters.Add("signIDCode", ""
);
            parameters.Add("supplierNo", ""
);
            parameters.Add("agingType", ""
);
            parameters.Add("sellerNote", ""
);
            parameters.Add("supervisionCode", ""
);
            parameters.Add("invoiceChecker", ""
);
            parameters.Add("paymentType", ""
);
            parameters.Add("saleType", ""
);
            parameters.Add("inStorageNo", ""
);
            parameters.Add("inStorageTime", ""
);
            parameters.Add("inStorageRemark", ""
);
            parameters.Add("grossReturnName", ""
);
            parameters.Add("grossReturnPhone", ""
);
            parameters.Add("grossReturnMobile", ""
);
            parameters.Add("grossReturnAddress", ""
);
            parameters.Add("isvPackTypeNo", ""
);
            parameters.Add("addrAnalysis", ""
);
            parameters.Add("rintExtendInfo", ""
);
            parameters.Add("logicParam", ""
);
            parameters.Add("combineNo", ""
);
            parameters.Add("activationService", ""
);
            parameters.Add("randomInspection", ""
);
            parameters.Add("vIPDeliWarehouse", ""
);
            parameters.Add("customField", ""
);
            parameters.Add("longitude", ""
);
            parameters.Add("latitude", ""
);
            parameters.Add("agingProductType", ""
);
            parameters.Add("crossDockPriority", ""
);
            parameters.Add("isvCompanyNo", ""
);
            parameters.Add("orderPriority", "");
            parameters.Add("orderBatchNo", "");
            parameters.Add("orderBatchQty", "");
            parameters.Add("productCode", "");
            parameters.Add("vehicleType", "");


            parameters.Add("skuGoodsLevel", ""
);
            parameters.Add("goodsName", ""
);
            parameters.Add("type", ""
);
            parameters.Add("unit", ""
);
            parameters.Add("remark", ""
);
            parameters.Add("rate", ""
);
            parameters.Add("price", ""
);
            parameters.Add("pAttributes", ""
);
            parameters.Add("isvLotattrs", ""
);
            parameters.Add("isvGoodsNo", ""
);
            parameters.Add("installVenderId", ""
);
            parameters.Add("orderLine", ""
);
            parameters.Add("batAttrs", ""
);
            parameters.Add("productionDate", ""
);
            parameters.Add("expirationDate", ""
);
            parameters.Add("packBatchNo", ""
);
            parameters.Add("poNo", ""
);
            parameters.Add("lot", ""
);
            parameters.AddAll(otherParameters);
            return parameters;
        }

    }
}
