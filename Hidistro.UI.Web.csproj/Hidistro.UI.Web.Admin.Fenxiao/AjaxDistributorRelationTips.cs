using Ajax;
using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.StatisticsReport;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class AjaxDistributorRelationTips : AdminPage
	{


		protected AjaxDistributorRelationTips() : base("m05", "fxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["HidistroSqlServer"].ConnectionString;
                string UserName = Request["UserName"];
                string UserID = "";
                if (string.IsNullOrEmpty(UserName))
                {
                    Response.Write("0");
                    return;
                }
                string sql = @" select UserId from aspnet_Distributors where StoreName like '%"+ UserName+"%'";
                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (SqlDataReader rd = comm.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            UserID = Convert.ToString((int)rd["UserId"]);
                        }
                    }
                    conn.Close();
                }
                if (string.IsNullOrEmpty(UserID))
                {
                    Response.Write("0");
                    return;
                }
                DataTable dt = GetMyTeams(int.Parse(UserID));
                StringBuilder str = new StringBuilder();
                if (dt.Rows.Count > 0)
                {
                    str.Append(Recursion(dt, UserID));
                    str = str.Remove(str.Length - 2, 2);
                }
                string returnstr = str.ToString();
                Response.Write(returnstr);
            }
            catch (Exception err)
            {
                string error = err.Message.ToString();
                Response.Write("0");
            }
        }

        private string Recursion(DataTable dt, object parentId)
        {
            StringBuilder sbJson = new StringBuilder();
            DataRow[] rows = dt.Select("referraluserid = " + parentId);
            if (rows.Length > 0)
            {
                sbJson.Append("[");
                for (int i = 0; i < rows.Length; i++)
                {
                    string childString = Recursion(dt, rows[i]["userid"]);
                    if (!string.IsNullOrEmpty(childString))
                    {
                        //comboTree必须设置【id】和【text】，一个是id一个是显示值
                        sbJson.Append("{\"id\":\"" + rows[i]["userid"].ToString() + "\",\"ParentId\":\"" + rows[i]["referraluserid"].ToString() + "\",\"Sort\":\"" + rows[i]["tLevel"].ToString() + "\",\"CreateDate\":\"" + rows[i]["CreateDate"].ToString() + "\",\"LastOrderDate\":\"" + rows[i]["LastOrderDate"].ToString() + "\",\"text\":\"" + rows[i]["username"].ToString() + "的小店" + "\",\"children\":");
                        sbJson.Append(childString);
                    }
                    else
                        sbJson.Append("{\"id\":\"" + rows[i]["userid"].ToString() + "\",\"ParentId\":\"" + rows[i]["referraluserid"].ToString() + "\",\"Sort\":\"" + rows[i]["tLevel"].ToString() + "\",\"CreateDate\":\"" + rows[i]["CreateDate"].ToString() + "\",\"LastOrderDate\":\"" + rows[i]["LastOrderDate"].ToString() + "\",\"text\":\"" + rows[i]["username"].ToString() + "的小店" + "\"},");
                }
                sbJson.Remove(sbJson.Length - 1, 1);
                sbJson.Append("]},");
            }
            return sbJson.ToString();
        }
        public System.Data.DataTable GetMyTeams(int UserID)
        {
            StringBuilder sql_user = new StringBuilder();
            sql_user.Append(" WITH COMMENT_CTE(userid,referraluserid,username,tLevel,CreateDate,LastOrderDate)AS( ");
            sql_user.Append(" SELECT userid,referraluserid,username,0 AS tLevel,CreateDate,LastOrderDate FROM aspnet_Members WHERE referraluserid = " + UserID + " ");
            sql_user.Append(" UNION ALL ");
            sql_user.Append(" SELECT c.userid,c.referraluserid,c.username,ce.tLevel + 1,c.CreateDate,c.LastOrderDate FROM aspnet_Members AS c  ");
            sql_user.Append(" INNER JOIN COMMENT_CTE AS ce ");
            sql_user.Append(" ON c.referraluserid = ce.userid ");
            sql_user.Append(" )SELECT * FROM COMMENT_CTE ");
            DataTable dt_UserIds = Query(sql_user.ToString()).Tables[0];
            return dt_UserIds;
        }
        public static DataSet Query(string SQLString)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["HidistroSqlServer"].ToString();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
    }
}
