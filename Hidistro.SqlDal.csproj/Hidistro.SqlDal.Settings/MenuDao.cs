using Hidistro.Entities;
using Hidistro.Entities.Settings;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Settings
{
	public class MenuDao
	{
		private Database database;

		public MenuDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public MenuInfo GetMenu(int menuId,string TenantID)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE MenuId = @MenuId AND TenantID=@TenantID");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, TenantID);
            MenuInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MenuInfo>(dataReader);
			}
			return result;
		}

        public IList<MenuInfo> GetTopMenus(string TenantID)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE ParentMenuId = 0 and TenantID=@TenantID");
            if (string.IsNullOrEmpty(TenantID))
            {
                this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, ConfigurationManager.AppSettings["SuperAdminTID"].ToString());
            }
            else
            {
                this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, TenantID);
            }
            IList<MenuInfo> result = null;
            try
            {
                using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
                {
                    result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
                }
            }
            catch (Exception err)
            {
                string errres = err.ToString();
            }
            return result;
        }

        public bool UpdateMenu(MenuInfo menu)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE VShop_NavMenu SET ParentMenuId = @ParentMenuId, Name = @Name, Type = @Type,DisplaySequence = @DisplaySequence,  [Content] = @Content,ShopMenuPic=@ShopMenuPic WHERE MenuId = @MenuId and TenantID=@TenantID");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, menu.DisplaySequence);
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menu.MenuId);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			this.database.AddInParameter(sqlStringCommand, "ShopMenuPic", System.Data.DbType.String, menu.ShopMenuPic);
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, menu.TenantID);
            return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
        public bool UpdateShopMenuStyle(string TenantID, int styleID)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE VShop_NavMenu SET ShopMenuStyle = @ShopMenuStyle WHERE  TenantID=@TenantID");
            this.database.AddInParameter(sqlStringCommand, "ShopMenuStyle", System.Data.DbType.Int32, styleID);
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, TenantID);
            return this.database.ExecuteNonQuery(sqlStringCommand)>0;
        }

        public int SaveMenu(MenuInfo menu)
		{
			int allMenusCount = this.GetAllMenusCount(menu.TenantID);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO VShop_NavMenu (ParentMenuId, Name, Type,  DisplaySequence,  [Content],ShopMenuPic,TenantID) VALUES(@ParentMenuId, @Name, @Type, @DisplaySequence, @Content,@ShopMenuPic,@TenantID);select @@IDENTITY ;");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menu.ParentMenuId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.String, menu.Type);
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, allMenusCount);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, menu.Content);
			this.database.AddInParameter(sqlStringCommand, "ShopMenuPic", System.Data.DbType.String, menu.ShopMenuPic);
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, menu.TenantID);
            return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool UpdateMenuName(MenuInfo menu)
		{
			int allMenusCount = this.GetAllMenusCount(menu.TenantID);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update VShop_NavMenu  set Name=@Name  where MenuId=@MenuId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, menu.Name);
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menu.MenuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		private int GetAllMenusCount(string TenantID)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from VShop_NavMenu Where TenantID=@TenantID");
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, TenantID);
            return 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DeleteMenu(int menuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE VShop_NavMenu WHERE (MenuId = @MenuId or ParentMenuId=@ParentMenuId)");
			this.database.AddInParameter(sqlStringCommand, "MenuId", System.Data.DbType.Int32, menuId);
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, menuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<MenuInfo> GetMenusByParentId(int parentId,string TenantID)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM VShop_NavMenu WHERE ParentMenuId = @ParentMenuId And TenantID=@TenantID");
			this.database.AddInParameter(sqlStringCommand, "ParentMenuId", System.Data.DbType.Int32, parentId);
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, TenantID);
            IList<MenuInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MenuInfo>(dataReader);
			}
			return result;
		}
	}
}
