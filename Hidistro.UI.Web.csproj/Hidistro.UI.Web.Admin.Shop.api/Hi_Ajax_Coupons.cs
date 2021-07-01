using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop.api
{
	public class Hi_Ajax_Coupons : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			context.Response.Write(this.GetModelJson(context));
		}

		public string GetModelJson(System.Web.HttpContext context)
		{
			DbQueryResult couponsTable = this.GetCouponsTable(context);
			int pageCount = TemplatePageControl.GetPageCount(couponsTable.TotalRecords, 10);
			if (couponsTable != null)
			{
				string str = "{\"status\":1,";
				str = str + this.GetCouponsListJson(couponsTable, context) + ",";
				str = str + "\"page\":\"" + this.GetPageHtml(pageCount, context) + "\"";
				return str + "}";
			}
			return "{\"status\":1,\"list\":[],\"page\":\"\"}";
		}

		public string GetPageHtml(int pageCount, System.Web.HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetCouponsListJson(DbQueryResult CouponsTable, System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("\"list\":[");
			System.Data.DataTable dataTable = (System.Data.DataTable)CouponsTable.Data;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"game_id\":\"" + dataTable.Rows[i]["CouponId"].ToString() + "\",");
				stringBuilder.Append("\"title\":\"" + dataTable.Rows[i]["CouponName"].ToString() + "\",");
				stringBuilder.Append("\"create_time\":\"" + System.DateTime.Now + "\",");
				stringBuilder.Append("\"type\":\"1\",");
				stringBuilder.Append("\"link\":\"/VShop/CouponDetails.aspx?CouponId=" + dataTable.Rows[i]["CouponId"].ToString() + "\"");
				stringBuilder.Append("},");
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			return str + "]";
		}

		public DbQueryResult GetCouponsTable(System.Web.HttpContext context)
		{
			return CouponHelper.GetCouponInfos(this.GetCouponsSearch(context));
		}

		public CouponsSearch GetCouponsSearch(System.Web.HttpContext context)
		{
			return new CouponsSearch
			{
				beginDate = new System.DateTime?(System.DateTime.Now),
				endDate = new System.DateTime?(System.DateTime.Now),
				PageIndex = ((context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"])),
				Finished = new bool?(false),
				SortOrder = SortAction.Desc,
				SortBy = "CouponId",
				SearchType = new int?(2)
			};
		}
	}
}
