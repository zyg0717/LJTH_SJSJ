using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lib.BLL;
using Lib.Common;
using static Plugin.Auth.AuthHelper;
using Framework.Web.Utility;

namespace WebApplication.WebPortal.Application.Task
{
    public partial class TaskList : PageBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "我的任务";
            if (Request.RawUrl.ToLower() == "/")
            {
                var isStartToUse = TemplateConfigInstanceOperator.Instance.IsStartToUse(WebHelper.GetCurrentUser().LoginName);
                if (!isStartToUse)
                {
                    Response.Redirect("~/Application/Navigation/QuickStart.aspx");
                }
            }
            //if (UserInfoOperator.Instance.IsAdmin())
            //{
            //    Response.Redirect("~/Application/Admin/AdminTaskList.aspx");
            //}
        }
    }
}