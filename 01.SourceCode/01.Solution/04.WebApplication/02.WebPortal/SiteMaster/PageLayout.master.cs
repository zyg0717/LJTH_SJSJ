
using Framework.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.WebPortal.SiteMaster
{
    public partial class PageLayout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //AuthContents authContentsFromCookie = HttpCookieHelper.GetAuthContentsFromCookie();
            //var ret = new AuthTokenFacade().Decrypt(authContentsFromCookie, GeneralConverter.ToBase64String(BytesGenerator.GeneratorCR1()));

            //MergeCookieHttpModule module = new MergeCookieHttpModule();
            //module.MergeSSOCookie("wangjian115");
            //AuthContents authContentsFromCookie = HttpCookieHelper.GetAuthContentsFromCookie();
            //var ret = new AuthTokenFacade().Decrypt(authContentsFromCookie, GeneralConverter.ToBase64String(BytesGenerator.GeneratorCR1()));
            //if (WebHelper.GetCurrentUser() == null)
            //{
            //    Response.Redirect("~/Public/Login.aspx?flag=1");
            //}
        }
    }
}