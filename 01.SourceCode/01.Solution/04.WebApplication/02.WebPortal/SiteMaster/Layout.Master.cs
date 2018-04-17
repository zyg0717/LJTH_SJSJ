using WebApplication.WebPortal.Application.Navigation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lib.BLL;
using Lib.Common;
using static Plugin.Auth.AuthHelper;

namespace WebApplication.WebPortal.SiteMaster
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        public string PageName { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = "数据收集系统";

        }
    }
}