using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Caching;

namespace Hidistro.ControlPanel.Store
{
	public static class ManagerHelper
	{
		public static void AddPrivilegeInRoles(int roleId, string strPermissions)
		{
			(new RoleDao()).AddPrivilegeInRoles(roleId, strPermissions);
		}

		public static bool AddRole(RoleInfo role)
		{
			return (new RoleDao()).AddRole(role);
		}

		public static bool AddRolePermission(IList<RolePermissionInfo> models, int roleId = 0)
		{
			if (models.Count > 0)
			{
				roleId = models[0].RoleId;
			}
			HiCache.Remove(string.Format("DataCache-RolePermissions-{0}", roleId));
			return (new RolePermissionDao()).AddRolePermission(models, roleId);
		}

		public static bool AddRolePermission(RolePermissionInfo model)
		{
			return ManagerHelper.AddRolePermission(new List<RolePermissionInfo>()
			{
				model
			}, 0);
		}

		public static void CheckPrivilege(Privilege privilege)
		{
			if (ManagerHelper.GetCurrentManager() == null)
			{
				HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath(string.Concat("/accessDenied.aspx?privilege=", privilege.ToString())));
			}
		}

		public static void ClearRolePrivilege(int roleId)
		{
			(new RoleDao()).ClearRolePrivilege(roleId);
		}

		public static bool Create(ManagerInfo manager)
		{
			return (new MessageDao()).Create(manager);
		}

		public static bool Delete(int userId)
		{
			bool flag;
			if (ManagerHelper.GetManager(userId).UserId != Globals.GetCurrentManagerUserId())
			{
				HiCache.Remove(string.Format("DataCache-Manager-{0}", userId));
				flag = (new MessageDao()).DeleteManager(userId);
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		public static bool DeleteRole(int roleId)
		{
			return (new RoleDao()).DeleteRole(roleId);
		}

		public static ManagerInfo GetCurrentManager()
		{
            var currentUserID = Globals.GetCurrentManagerUserId();

            return ManagerHelper.GetManager(currentUserID);
		}
        public static string GetCurrentTenantID()
        {
            var currentUserID = Globals.GetCurrentManagerUserId();
            var m= GetManager(currentUserID);
            if (m == null)
            {
                m = GetManagerByTenantID(ConfigurationManager.AppSettings["SuperAdminTID"].ToString());
            }
            return m.TenantID.ToString() ;
        }

        public static ManagerInfo GetManager(int userId)
		{
			ManagerInfo manager = HiCache.Get(string.Format("DataCache-Manager-{0}", userId)) as ManagerInfo;
			if (manager == null)
			{
				manager = (new MessageDao()).GetManager(userId);
				HiCache.Insert(string.Format("DataCache-Manager-{0}", userId), manager, 360, CacheItemPriority.Normal);
			}
            return manager;
		}
        public static ManagerInfo GetManager(string userName)
		{
			return (new MessageDao()).GetManager(userName);
		}
        public static ManagerInfo GetManagerByTenantID(string tID)
        {
            return (new MessageDao()).GetManagerByTenantID(tID);
        }
        public static DbQueryResult GetManagers(ManagerQuery query)
		{
			return (new MessageDao()).GetManagers(query);
		}

		public static IList<int> GetPrivilegeByRoles(int roleId)
		{
			return (new RoleDao()).GetPrivilegeByRoles(roleId);
		}

		public static RoleInfo GetRole(int roleId)
		{
			return (new RoleDao()).GetRole(roleId);
		}

		public static IList<RolePermissionInfo> GetRolePremissonsByRoleId(int roleId)
		{
			return (new RolePermissionDao()).GetPermissionsByRoleId(roleId);
		}

		public static IList<RoleInfo> GetRoles()
		{
			return (new RoleDao()).GetRoles();
		}

		public static bool IsHavePermission(RolePermissionInfo model)
		{
			bool flag;
			string str = HiCache.Get(string.Format("DataCache-RolePermissions-{0}", model.RoleId)) as string;
			List<RolePermissionInfo> rolePermissionInfos = new List<RolePermissionInfo>();
			if (!string.IsNullOrEmpty(str))
			{
				rolePermissionInfos = JsonConvert.DeserializeObject<List<RolePermissionInfo>>(HiCryptographer.Decrypt(str));
			}
			if ((rolePermissionInfos == null ? true : rolePermissionInfos.Count == 0))
			{
				rolePermissionInfos = (new RolePermissionDao()).GetPermissionsByRoleId(model.RoleId);
				string str1 = HiCryptographer.Encrypt(JsonConvert.SerializeObject(rolePermissionInfos));
				HiCache.Insert(string.Format("DataCache-RolePermissions-{0}", model.RoleId), str1, 360, CacheItemPriority.Normal);
			}
			if ((rolePermissionInfos == null ? false : rolePermissionInfos.Count != 0))
			{
				RolePermissionInfo rolePermissionInfo = rolePermissionInfos.FirstOrDefault<RolePermissionInfo>((RolePermissionInfo p) => p.PermissionId == model.PermissionId);
				flag = rolePermissionInfo != null;
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		public static bool IsHavePermission(string itemId, string pageLinkId, int roleId)
		{
			bool flag;
			if ((pageLinkId == "dpp01" ? false : !(pageLinkId == "00000")))
			{
				RolePermissionInfo rolePermissionInfo = new RolePermissionInfo()
				{
					PermissionId = RolePermissionInfo.GetPermissionId(itemId, pageLinkId),
					RoleId = roleId
				};
				flag = ManagerHelper.IsHavePermission(rolePermissionInfo);
			}
			else
			{
				flag = true;
			}
			return flag;
		}

		public static bool RoleExists(string roleName)
		{
			return (new RoleDao()).RoleExists(roleName);
		}

		public static bool Update(ManagerInfo manager)
		{
			bool flag;
			HiCache.Remove(string.Format("DataCache-Manager-{0}", manager.UserId));
			if (!(new MessageDao()).Update(manager))
			{
				flag = false;
			}
			else
			{
				HiCache.Insert(string.Format("DataCache-Manager-{0}", manager.UserId), manager, 360, CacheItemPriority.Normal);
				flag = true;
			}
			return flag;
		}

		public static bool UpdateRole(RoleInfo role)
		{
			return (new RoleDao()).UpdateRole(role);
		}
	}
}