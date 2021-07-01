using Hidistro.Entities.Settings;
using Hidistro.SqlDal.Settings;
using System;
using System.Collections.Generic;

namespace ControlPanel.Settings
{
	public static class MenuHelper
	{
		public static bool CanAddMenu(int parentId,string TenantID)
		{
			bool flag;
			IList<MenuInfo> menusByParentId = (new MenuDao()).GetMenusByParentId(parentId,TenantID);
			if ((menusByParentId == null ? false : menusByParentId.Count != 0))
			{
				flag = (parentId != 0 ? menusByParentId.Count < 5 : menusByParentId.Count < 5);
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		public static bool DeleteMenu(int menuId)
		{
			return (new MenuDao()).DeleteMenu(menuId);
		}

		public static MenuInfo GetMenu(int menuId,string TenantID)
		{
			return (new MenuDao()).GetMenu(menuId,TenantID);
		}

		public static IList<MenuInfo> GetMenus(string TenantID )
		{
			IList<MenuInfo> menuInfos;
			IList<MenuInfo> menuInfos1 = new List<MenuInfo>();
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus(TenantID);
			if (topMenus != null)
			{
				foreach (MenuInfo topMenu in topMenus)
				{
					IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(topMenu.MenuId,TenantID);

                    if (topMenu.ShopMenuStyle == 0)
                    {
                        topMenu.ShopMenuPic = "";
                    }
					
					topMenu.SubMenus = menusByParentId;
					menuInfos1.Add(topMenu);
				}
				menuInfos = menuInfos1;
			}
			else
			{
				menuInfos = menuInfos1;
			}
			return menuInfos;
		}

		public static IList<MenuInfo> GetMenusByParentId(int parentId,string TenantID)
		{
			return (new MenuDao()).GetMenusByParentId(parentId,TenantID);
		}

		public static IList<MenuInfo> GetTopMenus(string TenantID)
		{
			return (new MenuDao()).GetTopMenus(TenantID);
		}
        public static bool UpdateShopMenuStyle(string TenantID, int typeID)
        {
            return (new MenuDao()).UpdateShopMenuStyle(TenantID, typeID);
        }

        public static int SaveMenu(MenuInfo menu)
		{
			return (new MenuDao()).SaveMenu(menu);
		}

		public static bool UpdateMenu(MenuInfo menu)
		{
			return (new MenuDao()).UpdateMenu(menu);
		}

		public static bool UpdateMenuName(MenuInfo menu)
		{
			return (new MenuDao()).UpdateMenuName(menu);
		}
	}
}