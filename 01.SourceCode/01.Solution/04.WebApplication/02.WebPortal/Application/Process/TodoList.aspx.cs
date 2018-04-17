using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.WebPortal.Application.Process
{
    public partial class TodoList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "待办/已办";
        }
    }
}