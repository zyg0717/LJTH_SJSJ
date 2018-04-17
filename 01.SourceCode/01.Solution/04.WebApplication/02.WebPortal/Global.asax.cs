using Framework.Web.Security.Authentication;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Lib.BLL;
using Lib.Common;
using Framework.Web.Utility;

namespace WebApplication.WebPortal
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            WebHelper.Instance.GetUser += UserInfoOperator.Instance.GetWDUserInfoByLoginName;
            //Client.GetUserIdentityEvent += () =>
            //{
            //    return new Client.UserIdentity() { UserFromSource = "Inside", UserLoginId = HttpContext.Current.User.Identity.Name };
            //};
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Client.GetUserIdentityEvent += () =>
            //{
            //    return new Client.UserIdentity() { UserFromSource = "Inside", UserLoginId = HttpContext.Current.User.Identity.Name };
            //};
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex is HttpException)
            {
                if (((HttpException)(ex)).GetHttpCode() == 500)
                {
                    Response.Redirect("~/Public/500.html");
                }
                else if (((HttpException)(ex)).GetHttpCode() == 404)
                {
                    Response.Redirect("~/Public/404.html");
                }
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}