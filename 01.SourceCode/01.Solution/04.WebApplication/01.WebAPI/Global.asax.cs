using WebApplication.WebAPI.Helper.Log;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Lib.BLL;
using Lib.Common;

namespace WebApplication.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //Client.GetUserIdentityEvent += () =>
            //{
            //    return new Client.UserIdentity() { UserFromSource = "Inside", UserLoginId = HttpContext.Current.User.Identity.Name };
            //};
            NLogTraceHelper.Instace.Info("应用程序已启动");
        }
    }
}
