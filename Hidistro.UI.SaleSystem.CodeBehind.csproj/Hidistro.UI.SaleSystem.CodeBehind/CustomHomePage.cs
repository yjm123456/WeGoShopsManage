using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.UI;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class CustomHomePage : CustomTemplatedWebControl
	{
		[System.ComponentModel.Bindable(true)]
		public string TempFilePath
		{
			get;
			set;
		}

		[System.ComponentModel.Bindable(true)]
		public string CustomPagePath
		{
			get;
			set;
		}

		protected override string SkinPath
		{
			get
			{
				string text = "/Templates/vshop/custom/" + this.CustomPagePath + "/" + this.SkinName;
				if (!System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(text)))
				{
					System.Web.HttpContext.Current.Response.Redirect("/Default.aspx");
				}
				return text;
			}
		}

		protected override void OnInit(System.EventArgs e)
		{
            ManagerInfo manager = ManagerHelper.GetManager(Globals.GetCurrentManagerUserId());
            if (manager == null)
            {
                throw new ArgumentNullException("ÉÌ»§Î´µÇÂ¼£¡");
            }
            this.TempFilePath = "Skin-HomePage"+manager.TenantID+".html";
			if (this.SkinName == null)
			{
				this.SkinName = this.TempFilePath;
			}
			base.OnInit(e);
		}
	}
}
