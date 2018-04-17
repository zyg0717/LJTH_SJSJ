using WebApplication.WebPortal.SiteMaster;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.WebPortal.Application.Task
{
    public partial class TaskCollection : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "任务填报";
            string IsFromWeb = Request.QueryString["IsFromWeb"];
            IsFromWeb = string.IsNullOrWhiteSpace(IsFromWeb) ? "0" : "1";
            if (Request.Browser.IsMobileDevice)
            {
                Server.Transfer("TaskCollectionMobile.aspx?IsFromWeb=" + IsFromWeb);
            }
        }
    }
}