using WebApplication.WebAPI.Models.Helper;
using Framework.Web.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib.BLL;
using Lib.Common;
using Framework.Web.Utility;
using Plugin.Auth;

namespace WebApplication.WebAPI.Helper.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class CommonValidation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerLoginUser"></param>
        public static void ValidateRoleRight(string ownerLoginUser)
        {
            var currentLoginUser = WebHelper.GetCurrentUser().LoginName;
            if (string.Equals(currentLoginUser, ownerLoginUser, StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }
            if (!AuthHelper.HasPrivilegeRight(AuthHelper.PrivilegeCode.IsAdminSubmit))
            {
                throw new BizException("无权限进行该操作");
            }
        }
    }
}