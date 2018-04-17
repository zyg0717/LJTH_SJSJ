using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lib.BLL;
using Lib.Model;

namespace WebApplication.WebPortal.Application.OWA
{
    public partial class Preview : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "文档在线预览";
        }
    }
}