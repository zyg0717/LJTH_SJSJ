using Framework.Web.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Auth
{
    public static class AuthHelper
    {
        public enum PrivilegeCode
        {
            IsViewAllTemplate = 1,
            IsViewAllTask = 2,
            IsAdminSubmit = 3
        }
        public static bool IsAdmin()
        {
            var key = "IsAdminUser";
            var currentUser = WebHelper.GetCurrentUser().LoginName;
            return ConfigurationManager.AppSettings[key].IndexOf(currentUser + ",", StringComparison.CurrentCultureIgnoreCase) >= 0;
            //return true;
            //var result = Auth.Client.AuthClientSDK.GetRoleListByUserInfo();
            //if (!result.IsSuccess)
            //{
            //    throw new Exception("获取用户角色失败");
            //}
            //return result.Data.Any(x => x.RoleCode == ConstSet.AdminRoleCode);
        }
        public static bool IsAudit()
        {
            var key = "IsAuditUser";
            var currentUser = WebHelper.GetCurrentUser().LoginName;
            return ConfigurationManager.AppSettings[key].IndexOf(currentUser + ",", StringComparison.CurrentCultureIgnoreCase) >= 0;
            //var result = Auth.Client.AuthClientSDK.GetRoleListByUserInfo();
            //if (!result.IsSuccess)
            //{
            //    throw new Exception("获取用户角色失败");
            //}
            //return result.Data.Any(x => x.RoleCode == ConstSet.AuditRoleCode);
            //return false;
        }
        public static bool HasPrivilegeRight(PrivilegeCode privilegeCode)
        {
            
            var key = privilegeCode.ToString() + "User";
            var currentUser = WebHelper.GetCurrentUser().LoginName;
            return ConfigurationManager.AppSettings[key].IndexOf(currentUser + ",", StringComparison.CurrentCultureIgnoreCase) >= 0;
            //var result = Auth.Client.AuthClientSDK.IsExistsFuntionAuth(privilegeCode.ToString());
            //return result;
            //return true;
        }
        public static string GetPrivilegeStartOrg()
        {
            return "-1";
            //var data = Auth.Client.AuthClientSDK.GetAuthContext().DataAuthList;
            //if (data.Count == 0)
            //{
            //    return "-1";
            //}
            //var start = data[ConstSet.DataPrivilegeGroupKey].OrderBy(x => x.Level).FirstOrDefault();
            //if (start == null)
            //{
            //    return "-1";
            //}
            //return start.AuthValue;
        }
    }
}
