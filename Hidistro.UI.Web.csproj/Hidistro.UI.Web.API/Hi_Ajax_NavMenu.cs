using ControlPanel.Settings;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Settings;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using Hidistro.SqlDal.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_NavMenu : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
        private string manID;
        private string TenantID;
        public void ProcessRequest(System.Web.HttpContext context)
        {
            //�Ȼ�ȡ�ֻ�������
            MemberInfo Menber = MemberHelper.GetMember(Globals.GetCurrentMemberUserId());
            if (Menber == null)
            {
                //��ȡ��ǰ����ԱID
                TenantID = ManagerHelper.GetCurrentTenantID();
            }
            else
            {
                //��ȡ��ǰ�⻧ID
                TenantID = Menber.TenantID;
                if (string.IsNullOrEmpty(TenantID))
                {
                    //��ǰ�⻧IDΪ��ʱ��ȡ���ϼ��⻧ID
                    int referralUserID = Menber.ReferralUserId;
                    if (referralUserID == 0)
                    {
                        //���ϼ����ȡ�����̼�ID
                        TenantID = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
                    }
                    else
                    {
                        var members = MemberHelper.GetMember(referralUserID);
                        if (members == null)
                        {
                            //�޷������ϼ����ȡ����ID
                            TenantID = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
                        }
                        else
                        {
                            var ParentTenantID = members.TenantID;
                            if (string.IsNullOrEmpty(ParentTenantID))
                            {
                                //�ϼ��ް��⻧ID���ȡ����ID
                                TenantID = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
                            }
                            else
                            {
                                TenantID = ParentTenantID;
                            }
                        }
                    }
                }
            }
            string userAgent = context.Request.UserAgent;
            context.Response.ContentType = "text/plain";
            SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
            System.Collections.Generic.IList<MenuInfo> allMenu = this.GetAllMenu();
            string guidePage = masterSettings.GuidePageSet;
            if (userAgent.ToLower().Contains("alipay"))
            {
                guidePage = masterSettings.AliPayFuwuGuidePageSet;
            }
            string s = JsonConvert.SerializeObject(new
            {
                status = 1,
                msg = "",
                Phone = this.GetPhone(),
                GuidePage = guidePage,
                ShopDefault = masterSettings.ShopDefault,
                MemberDefault = masterSettings.MemberDefault,
                GoodsType = masterSettings.GoodsType,
                GoodsCheck = masterSettings.GoodsCheck,
                ActivityMenu = masterSettings.ActivityMenu,
                DistributorsMenu = masterSettings.DistributorsMenu,
                GoodsListMenu = masterSettings.GoodsListMenu,
                BrandMenu = masterSettings.BrandMenu,
                ShopMenuStyle = masterSettings.ShopMenuStyle,
                menuList = allMenu
            });
            context.Response.Write(s);
        }

        public string GetPhone()
		{
			int currentDistributorId = Globals.GetCurrentDistributorId();
			if (currentDistributorId == 0)
			{
				return SettingsManager.GetMasterSettings(true).ShopTel;
			}
			MemberInfo member = MemberProcessor.GetMember(currentDistributorId, true);
			if (member != null)
			{
				return member.CellPhone;
			}
			return SettingsManager.GetMasterSettings(true).ShopTel;
		}

		public System.Collections.Generic.IList<MenuInfo> GetAllMenu()
		{
			System.Collections.Generic.IList<MenuInfo> list = new System.Collections.Generic.List<MenuInfo>();
			return MenuHelper.GetMenus(manID);
		}
	}
}
