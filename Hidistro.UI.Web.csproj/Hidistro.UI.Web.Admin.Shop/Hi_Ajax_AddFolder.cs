using Hidistro.ControlPanel.Store;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_AddFolder : System.Web.IHttpHandler
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
			context.Response.Write(this.InsertFolder());
		}

		public string InsertFolder()
		{
            string TenantID = ManagerHelper.GetCurrentTenantID();
            int num = GalleryHelper.AddPhotoCategory2("新建文件夹",TenantID);
			return "{\"status\":1,\"data\":" + num + ",\"msg\":\"\"}";
		}
	}
}
