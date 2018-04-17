using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib.BLL;
using Lib.Common;
using static Plugin.Auth.AuthHelper;
using Plugin.Auth;

namespace WebApplication.WebPortal.SiteMaster
{
    public class PageBase : System.Web.UI.Page
    {
        public string PageName { get; set; }
        public bool IsHasAllTask
        {
            get
            {
                if (ViewState["IsHasAllTask"] == null)
                {
                    ViewState["IsHasAllTask"] = AuthHelper.HasPrivilegeRight(PrivilegeCode.IsViewAllTask);
                }
                return (bool)ViewState["IsHasAllTask"];
            }
        }
        public bool IsHasAllTemplate
        {
            get
            {
                if (ViewState["IsHasAllTemplate"] == null)
                {
                    ViewState["IsHasAllTemplate"] = AuthHelper.HasPrivilegeRight(PrivilegeCode.IsViewAllTemplate);
                }
                return (bool)ViewState["IsHasAllTemplate"];
            }
        }
        public bool IsAdmin
        {
            get
            {
                if (ViewState["IsAdmin"] == null)
                {
                    ViewState["IsAdmin"] = UserInfoOperator.Instance.IsAdmin();
                }
                return (bool)ViewState["IsAdmin"];
            }
        }
    }
}