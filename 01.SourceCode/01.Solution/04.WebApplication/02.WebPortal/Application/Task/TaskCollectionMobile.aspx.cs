using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.WebPortal.Application.Task
{
    public partial class TaskCollectionMobile : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "任务填报";
        }
    }
}