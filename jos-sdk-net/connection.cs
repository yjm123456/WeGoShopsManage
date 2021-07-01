using Hidistro.Core;
using Hidistro.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace jos_sdk_net
{
    public static class connection
    {

        public static string connStr = ConfigurationManager.ConnectionStrings["HidistroSqlServer"].ConnectionString;
        /// <summary>
        /// 获取订单ID
        /// </summary>
        /// <param name="strSQL"></param>
        /// <returns></returns>
        public static List<OrderInfo> GetOrders(string strSQL)
        {
            List<OrderInfo> hsProductList = new List<OrderInfo>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(strSQL, conn))
                {
                    conn.Open();
                    using (SqlDataReader rd = comm.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            OrderInfo o = new OrderInfo();
                            o.OrderId = (string)rd["OrderId"];
                            o.ShippingRegion = (string)rd["ShippingRegion"];
                            o.Address = (string)rd["Address"];
                            o.Remark = rd["Remark"].ToString();
                            o.ShipTo = rd["ShipTo"].ToString();
                            o.CellPhone = rd["cellPhone"].ToString();
                            o.OrderDate = (DateTime)rd["OrderDate"];
                            hsProductList.Add(o);
                        }
                    }
                    conn.Close();
                }

            }
            catch (Exception err)
            {
                //记录日志
                Globals.Debuglog("Jos-sdk获取订单ID出错！" + err.Message.ToString(), "_Debuglog.txt");
            }
            return hsProductList;
        }
        /// <summary>
        /// 获取产品ID
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<OrderInfo> GetProductID(string strSQL, string value)
        {
            List<OrderInfo> hsProductList = new List<OrderInfo>();
            try
            {

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(strSQL, conn))
                {
                    conn.Open();
                    if (!string.IsNullOrEmpty(value))
                        comm.Parameters.Add("@OrderID", SqlDbType.NVarChar, 50).Value = value;
                    using (SqlDataReader rd = comm.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            OrderInfo o = new OrderInfo();
                            o.ProductId = string.IsNullOrEmpty(rd["productid"].ToString()) ? 0 : int.Parse(rd["productid"].ToString());
                            o.SKU = rd["SKU"].ToString();
                            o.Quantity = string.IsNullOrEmpty(rd["Quantity"].ToString()) ? "0" : rd["Quantity"].ToString();
                            o.ItemDescription = rd["ItemDescription"].ToString();
                            hsProductList.Add(o);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                //记录日志
                Globals.Debuglog("获取产品ID出错", "_Debuglog.txt");
            }
            return hsProductList;
        }
        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetProductDetails(string strSQL, string value)
        {
            string ProductCode = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(strSQL, conn))
                {
                    conn.Open();
                    if (!string.IsNullOrEmpty(value))
                        comm.Parameters.Add("@ProductID", SqlDbType.Int).Value = value;
                    using (SqlDataReader rd = comm.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            ProductCode = (string)rd["ProductCode"];
                        }
                    }
                    conn.Close();
                }

            }
            catch (Exception err)
            {
                //记录日志
                Globals.Debuglog("获取产品ID出错", "_Debuglog.txt");
            }
            return ProductCode;
        }
        /// <summary>
        /// 获取产品tag(订单提交方向)
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetProductTag(string strSQL, string value)
        {
            int returnTags = 0;
            try
            {
                OrderInfo hsProductList = new OrderInfo();
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(strSQL, conn))
                {
                    conn.Open();
                    if (!string.IsNullOrEmpty(value))
                        comm.Parameters.Add("@ProductID", SqlDbType.Int).Value = value;
                    using (SqlDataReader rd = comm.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            returnTags = (int)rd["TagId"];
                        }
                    }
                }
            }
            catch (Exception err)
            {
                //记录日志
                Globals.Debuglog("获取产品tag错误", "_Debuglog.txt");
            }
            return returnTags;
        }
        /// <summary>
        /// 查询已提交京东订单
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static List<OrderInfo> GetProductCheckResult(string strSQL, string OrderID)
        {
            List<OrderInfo> ord = new List<OrderInfo>();
            OrderInfo o = new OrderInfo();
            try
            {
                OrderInfo hsProductList = new OrderInfo();
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(strSQL, conn))
                {
                    conn.Open();
                    if (!string.IsNullOrEmpty(OrderID))
                        comm.Parameters.Add("@OrderID", SqlDbType.NVarChar, 100).Value = OrderID;
                    using (SqlDataReader rd = comm.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            o.OrderId = rd["OrderID"].ToString();
                            ord.Add(o);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                //记录日志
                Globals.Debuglog("查询已提交京东订单出错", "_Debuglog.txt");
            }
            return ord;
        }

        /// <summary>
        /// 添加以上送订单数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static int InsertAddOrders(string sql, string OrderID, string JDVID)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    comm.Parameters.Add("@OrderID", SqlDbType.NVarChar, 100).Value = OrderID;
                    comm.Parameters.Add("@wayBill", SqlDbType.NVarChar, 100).Value = JDVID;
                    result = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception err)
            {

                //记录日志
                Globals.Debuglog("插入已上送京东订单数据错误", "_Debuglog.txt");
            }

            return result;

        }

        /// <summary>
        /// 彻底删除订单时删除OrderAlreadySend表数据
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static int DeleteOrderAlreadySend(string OrderID)
        {
            int resultDeleteOrderAlreadys = 0;
            try
            {
                string sql = @" DELETE FROM Hishop_OrderAlreadySend WHERE OrderId='" + OrderID + "'";
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    resultDeleteOrderAlreadys += comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception err)
            {
                string erMsg = err.Message.ToString();
                Globals.Debuglog("删除OrderAlreadySend表失败，原因：" + erMsg, "_Debuglog.txt");
                resultDeleteOrderAlreadys = 0;
            }
            return resultDeleteOrderAlreadys;
        }
    }
}
