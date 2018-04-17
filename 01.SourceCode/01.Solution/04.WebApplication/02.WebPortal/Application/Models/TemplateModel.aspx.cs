using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.WebPortal.Application.Models
{
    public partial class TemplateModel : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "编辑模板";
        }
    }
}