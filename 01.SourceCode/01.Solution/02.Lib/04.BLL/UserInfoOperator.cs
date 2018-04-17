using Framework.Core;
using Framework.Web.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Common;
using Lib.DAL;
using Lib.Model;
using Plugin.Auth;

namespace Lib.BLL
{
    public class UserInfoOperator
    {
        //public bool CheckIsNormalEmployeeStatus(int employeeStatus)
        //{
        //    var normalEmployeeStatusList = ConstSet.NormalEmployeeStatus.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
        //    return normalEmployeeStatusList.Any(x => x == employeeStatus);
        //}
        //public bool IsAudit()
        //{
        //    return AuthHelper.IsAudit();
        //    //var roles = RoleinfoOperator.Instance.GetBelongsToRoles(WebHelper.GetCurrentUser().EmployeeCode);
        //    //return roles.Any(x => x.ID.ToLower() == ConstSet.AuditRoleID.ToLower());
        //}
        public bool IsAdmin()
        {
            return AuthHelper.IsAdmin() || AuthHelper.IsAudit();
            //if (IsAudit())
            //{
            //    return true;
            //}
            //var roles = RoleinfoOperator.Instance.GetBelongsToRoles(WebHelper.GetCurrentUser().EmployeeCode);
            //return roles.Any(x => x.ID.ToLower() == ConstSet.AdminRoleID.ToLower());
        }

        private static readonly UserInfoAdapter userAdapter = new UserInfoAdapter();

        public static readonly UserInfoOperator Instance = new UserInfoOperator();
        public LoginUserInfo GetWDUserInfoByID(string userId)
        {
            return userAdapter.GetWDUserInfoByID(userId);
        }
        public LoginUserInfo GetWDUserInfoByEmployeeID(string employeeID)
        {
            return userAdapter.GetWDUserInfoByEmployeeID(employeeID);
        }
        public LoginUserInfo GetWDUserInfoByLoginName(string loginName)
        {
            return userAdapter.GetWDUserInfoByLoginName(loginName);
        }
        public List<LoginUserInfo> GetWDUserInfoByLoginNameList(List<string> loginNames)
        {
            return userAdapter.GetWDUserInfoByLoginNameList(loginNames);
        }


    }
}
