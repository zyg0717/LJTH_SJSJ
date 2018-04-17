using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.WebPortal.Application.Task
{
    public partial class TaskCollectionViewMobile : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "任务审批";
        }
    }
}