using Hidistro.Core;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal
{
    public class SqlFind
    {
        private Database database;
        public SqlFind()
        {
            this.database = DatabaseFactory.CreateDatabase();
        }
        public static string FindTenantID() {
            string TenantID = "";
            var userid = Globals.GetCurrentMemberUserId();
            if (userid == 0)
            {
                var mamagerid = Globals.GetCurrentManagerUserId();
                if (mamagerid == 0)
                {
                    TenantID = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
                }
                else
                {
                    var managerInfo = (new MessageDao()).GetManager(mamagerid);
                }
            }
            else
            {
                var memberInfo = (new MemberDao()).GetMember(userid);
                if (memberInfo == null)
                {
                    TenantID = ConfigurationManager.AppSettings["SuperAdminTID"].ToString();
                }
                else
                {
                    TenantID = memberInfo.TenantID;
                }
            }
            return TenantID;
        }
    }
}
