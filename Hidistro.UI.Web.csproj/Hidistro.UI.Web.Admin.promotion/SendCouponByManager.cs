using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class SendCouponByManager : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm form;

		protected System.Web.UI.WebControls.DropDownList ddlCouponList;

		protected ucDateTimePicker ucDateBeginDate;

		protected ucDateTimePicker ucDateEndDate;

        protected string TenantID = ManagerHelper.GetCurrentTenantID();

        protected SendCouponByManager() : base("m08", "yxp18")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindDdlCouponList();
			}
		}

		private void BindDdlCouponList()
		{
			System.Data.DataTable unFinishedCoupon = CouponHelper.GetUnFinishedCoupon(System.DateTime.Now, new CouponType?(CouponType.手工发送));
			if (unFinishedCoupon != null)
			{
				foreach (System.Data.DataRow dataRow in unFinishedCoupon.Rows)
				{
					this.ddlCouponList.Items.Add(new System.Web.UI.WebControls.ListItem
					{
						Text = dataRow["CouponName"].ToString(),
						Value = dataRow["CouponId"].ToString()
					});
				}
			}
		}

		protected string GetMemberGrande()
        {
            //客户端登录
            var manID = "";
            int userid = 0;
            userid = Globals.GetCurrentMemberUserId();
            if (userid != 0)
            {
                //获取管理员登录
                MemberInfo member = MemberHelper.GetMember(userid);
                if (member == null)
                {
                    manID = ManagerHelper.GetCurrentTenantID();
                }
                else
                {
                    manID = member.TenantID;
                }
            }
            //管理端登录
            else
            {
                manID = ManagerHelper.GetCurrentTenantID();
            }
            if (manID == "")
            {
                manID = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
            }
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades(manID);
			if (memberGrades != null && memberGrades.Count > 0)
			{
				foreach (MemberGradeInfo current in memberGrades)
				{
					stringBuilder.Append(" <label class=\"middle mr20\">");
					stringBuilder.AppendFormat("<input type=\"checkbox\" class=\"memberGradeCheck\" value=\"{0}\">{1}", current.GradeId, current.Name);
					stringBuilder.Append("  </label>");
				}
            }
            Globals.Debuglog("jiehsu", "_Debuglog.txt");
            return stringBuilder.ToString();
		}

		protected string GetMemberCustomGroup()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Collections.Generic.IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList(TenantID);
			if (customGroupingList != null && customGroupingList.Count > 0)
			{
				foreach (CustomGroupingInfo current in customGroupingList)
				{
					stringBuilder.Append(" <label class=\"middle mr20\">");
					stringBuilder.AppendFormat("<input type=\"checkbox\" class=\"customGroup\" value=\"{0}\">{1}", current.Id, current.GroupName);
					stringBuilder.Append("  </label>");
				}
			}
			return stringBuilder.ToString();
		}
	}
}
