using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class HomePage : NewTemplatedWebControl
	{
		[System.ComponentModel.Bindable(true)]
		public string TempFilePath
		{
			get;
			set;
		}

		protected override void OnInit(System.EventArgs e)
		{
            ManagerInfo manager = ManagerHelper.GetManager(Globals.GetCurrentManagerUserId());
            if (manager == null)
            {
                var tid = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
                //���ͻ������Ĺ���Ա��Ӧ����
                MemberInfo member = MemberHelper.GetMember(Globals.GetCurrentMemberUserId());
                if (member != null)
                {
                    var tenantID = member.TenantID;
                    if (string.IsNullOrEmpty(tenantID))
                    {
                        this.TempFilePath = "Skin-HomePage" + tid + ".html";
                    }
                    else
                    {
                        this.TempFilePath = "Skin-HomePage" + tenantID + ".html";
                    }
                }
                else {
                    //���û�󶨹���Ա��ʾt1
                    this.TempFilePath = "Skin-HomePage" + tid + ".html";
                }

               
            }
            else {
                this.TempFilePath = "Skin-HomePage" + manager.TenantID + ".html";
            }
			if (this.SkinName == null)
			{
				this.SkinName = this.TempFilePath;
			}
			base.OnInit(e);
		}
	}
}
