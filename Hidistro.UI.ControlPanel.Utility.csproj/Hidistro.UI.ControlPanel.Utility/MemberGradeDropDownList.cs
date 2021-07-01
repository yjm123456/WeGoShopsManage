using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class MemberGradeDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}

		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = null;
				}
				else
				{
					result = new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
            }
            var manID = "";
            //客户端登录
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
            else {
                manID = ManagerHelper.GetCurrentTenantID();
            }
            if (manID == "")
            {
                manID = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
            }

            IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades(manID);
			foreach (MemberGradeInfo current in memberGrades)
			{
				this.Items.Add(new ListItem(Globals.HtmlDecode(current.Name), current.GradeId.ToString()));
			}

        }
	}
}
