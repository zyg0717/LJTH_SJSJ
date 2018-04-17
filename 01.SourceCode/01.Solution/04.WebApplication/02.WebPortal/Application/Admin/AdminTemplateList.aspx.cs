using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lib.BLL;

namespace WebApplication.WebPortal.Application.Admin
{
    public partial class AdminTemplateList : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "全部模板";
            if (!UserInfoOperator.Instance.IsAdmin())
            {
                Response.Write("您无该页面访问权限");
                Response.End();
            }
        }
    }
}