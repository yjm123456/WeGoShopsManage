using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class WXConfigChangeBind : AdminPage
	{
		protected int BindOpenIDAndNoUserNameCount;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected Script Script5;

		protected Script Script6;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtUserName;

		protected System.Web.UI.WebControls.TextBox txtPhone;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected PageSize hrefPageSize;

		protected Pager pager;

		protected System.Web.UI.WebControls.Button btnBatchSave;

		protected Grid grdMemberList;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hdUserId;

		protected System.Web.UI.WebControls.Button huifuUser;

		protected System.Web.UI.WebControls.Button BatchHuifu;

		protected Pager pager1;

		protected WXConfigChangeBind() : base("m06", "wxp01")
		{
		}

		protected void grdMemberList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				return;
			}
			e.Row.Visible = false;
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdMemberList.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdMemberList_RowDataBound);
			this.grdMemberList.RowUpdating += new System.Web.UI.WebControls.GridViewUpdateEventHandler(this.gvList_RowUpdating);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnBatchSave.Click += new System.EventHandler(this.btnSaveAll_Click);
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			MemberQuery memberQuery = new MemberQuery();
			memberQuery.PageIndex = this.pager.PageIndex;
			memberQuery.SortBy = this.grdMemberList.SortOrderBy;
			memberQuery.PageSize = this.pager.PageSize;
			memberQuery.Stutas = new UserStatus?(UserStatus.Normal);
			memberQuery.EndTime = new System.DateTime?(System.DateTime.Now);
			memberQuery.Username = this.txtUserName.Text.Trim();
			memberQuery.CellPhone = this.txtPhone.Text.Trim();
			if (this.grdMemberList.SortOrder.ToLower() == "desc")
			{
				memberQuery.SortOrder = SortAction.Desc;
			}
			DbQueryResult members = MemberHelper.GetMembers(memberQuery, true);
			this.grdMemberList.DataSource = members.Data;
			this.grdMemberList.DataBind();
			if (members.TotalRecords == 0)
			{
				base.Response.Redirect("WXConfigBindOK.aspx");
				base.Response.End();
			}
			this.pager1.TotalRecords = (this.pager.TotalRecords = members.TotalRecords);
			this.BindOpenIDAndNoUserNameCount = this.pager1.TotalRecords;
			this.ViewState["BindOpenIDAndNoUserNameCount"] = this.BindOpenIDAndNoUserNameCount;
		}

		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}

		protected void btnSaveAll_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < this.grdMemberList.Rows.Count; i++)
			{
				System.Web.UI.WebControls.CheckBox checkBox = this.grdMemberList.Rows[i].Cells[0].Controls[0] as System.Web.UI.WebControls.CheckBox;
				if (checkBox.Checked)
				{
					System.Web.UI.WebControls.TextBox textBox = this.grdMemberList.Rows[i].FindControl("txtUserName") as System.Web.UI.WebControls.TextBox;
					int userId = (int)this.grdMemberList.DataKeys[i].Value;
					string empty = string.Empty;
					bool flag = this.UpdateMemeberBindName(textBox.Text.Trim(), userId, out empty);
					if (flag)
					{
						num++;
					}
					else
					{
						num2++;
					}
				}
			}
			if (num + num2 > 0)
			{
				this.BindData();
				this.ShowResult(num, num2);
				return;
			}
			try
			{
				this.BindOpenIDAndNoUserNameCount = (int)this.ViewState["BindOpenIDAndNoUserNameCount"];
			}
			catch (System.Exception)
			{
			}
			this.ShowMsg("????????????????????????????????????", false);
		}

		protected void gvList_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
		{
			int userId = (int)this.grdMemberList.DataKeys[e.RowIndex].Value;
			string text = ((System.Web.UI.WebControls.TextBox)this.grdMemberList.Rows[e.RowIndex].FindControl("txtUserName")).Text.Trim();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("????????????????????????", false);
				try
				{
					this.BindOpenIDAndNoUserNameCount = (int)this.ViewState["BindOpenIDAndNoUserNameCount"];
				}
				catch (System.Exception)
				{
				}
				return;
			}
			string empty = string.Empty;
			bool flag = this.UpdateMemeberBindName(text, userId, out empty);
			if (flag)
			{
				this.ShowMsg("???????????????", true);
				this.BindData();
				return;
			}
			try
			{
				this.BindOpenIDAndNoUserNameCount = (int)this.ViewState["BindOpenIDAndNoUserNameCount"];
			}
			catch (System.Exception)
			{
			}
			if (!string.IsNullOrEmpty(empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			this.ShowMsg("???????????????", false);
		}

		protected bool UpdateMemeberBindName(string bindName, int userId, out string msg)
		{
			msg = string.Empty;
			string password = HiCryptographer.Md5Encrypt("123456");
			if (bindName.Length < 2)
			{
				msg = "???????????????????????????";
				return false;
			}
			if (!MemberHelper.IsExistUserBindName(bindName))
			{
				return MemberHelper.BindUserName(userId, bindName, password);
			}
			msg = "???????????????";
			return false;
		}

		private void ShowResult(int success, int fail)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("<div class='modal fade' role='dialog' aria-labelledby='mySmallModalLabel' id='myModal'>");
			stringBuilder.Append(" <div class='modal-dialog modal-sm'>\r\n                <div class='modal-content'>\r\n                    <div class='w-modalbox'>\r\n                        <h5>????????????</h5>\r\n                        <div class='titileBorderBox borderSolidB'>\r\n                            <div class='contentBox pl20 modalcontext'>\r\n                         <div>\r\n                             ??????????????????????????????????????????????????????????????????????????????????????????????????????\r\n                             ?????????????????????????????????????????????\r\n                         </div>");
			if (fail > 0)
			{
				stringBuilder.AppendFormat("<p style='text-align:left;text-indent:20px;'><span>{0}</span>?????????????????????????????????</p>", success);
				stringBuilder.AppendFormat("<p style='text-align:left;text-indent:20px;margin-top:0px;'><span>{0}</span>?????????????????????????????????????????????????????????????????????????????????</p>", fail);
			}
			else
			{
				stringBuilder.AppendFormat("<p>???????????????<span>{0}</span>????????????????????????</p>", success);
			}
			stringBuilder.Append(" </div></div> <div class='y-ikown pt10 pb10'>");
			stringBuilder.AppendFormat("<input type='submit'  value='{0}' class='btn btn-success inputw100' data-dismiss='modal' onclick='return ModifyMemo1();'>", (fail > 0) ? "??????????????????" : "????????????");
			stringBuilder.Append(" </div> </div> </div> </div> </div>");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScriptMsg"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScriptMsg", stringBuilder.ToString());
			}
		}
	}
}
