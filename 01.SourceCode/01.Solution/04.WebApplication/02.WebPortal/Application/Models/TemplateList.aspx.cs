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

namespace WebApplication.WebPortal.Application.Models
{
    public partial class TemplateList : PageBase
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "我的模板";

            //if (UserInfoOperator.Instance.IsAdmin())
            //{
            //    Response.Redirect("~/Application/Admin/AdminTemplateList.aspx");
            //}
        }
    }
}