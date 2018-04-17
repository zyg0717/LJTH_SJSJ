using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lib.BLL;
using Lib.Common;

namespace WebApplication.WebPortal.Application.Navigation
{
    public partial class QuickStart : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "新手教程";

            //var IsStartToUse = TemplateConfigInstanceOperator.Instance.IsStartToUse(WebHelper.GetCurrentLoginUser());
            //if (IsStartToUse)
            //{
            //    Response.Redirect("~/Application/Task/TaskList.aspx");
            //}
        }
    }
}