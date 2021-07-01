using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class MessageDao
	{
		private Database database;

		public MessageDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetManagers(ManagerQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			if (query.RoleId != 0)
			{
				stringBuilder.AppendFormat(" AND RoleId = {0}", query.RoleId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Managers", "UserId", stringBuilder.ToString(), "*");
		}
        /// <summary>
        /// 获取管理员数据
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
		public ManagerInfo GetManager(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, userId);
			ManagerInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ManagerInfo>(dataReader);
			}
			return result;
		}

		public ManagerInfo GetManager(string userName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserName = @UserName");
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, userName);
			ManagerInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ManagerInfo>(dataReader);
			}
			return result;
		}
        /// <summary>
        /// 通过租户ID查询所属商户
        /// </summary>
        /// <param name="TenantID"></param>
        /// <returns></returns>
        public ManagerInfo GetManagerByTenantID(string TenantID)
        {
            System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE TenantID = @TenantID");
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, TenantID);
            ManagerInfo result = null;
            using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
            {
                result = ReaderConvert.ReaderToModel<ManagerInfo>(dataReader);
            }
            return result;
        }

        public bool Create(ManagerInfo manager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (RoleId, UserName, Password, Email, CreateDate,TenantID) VALUES (@RoleId, @UserName, @Password, @Email, @CreateDate,@TenantID)");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, manager.RoleId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, manager.UserName);
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, manager.Password);
			this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, manager.Email);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, manager.CreateDate);
            this.database.AddInParameter(sqlStringCommand, "TenantID", System.Data.DbType.String, manager.TenantID);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteManager(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Managers WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Update(ManagerInfo manager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Managers SET RoleId = @RoleId, UserName = @UserName, Password = @Password, Email = @Email, tMenu = @tMenu WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, manager.UserId);
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, manager.RoleId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, manager.UserName);
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, manager.Password);
			this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, manager.Email);
            this.database.AddInParameter(sqlStringCommand, "tMenu", System.Data.DbType.String, manager.tMenu);
            return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
