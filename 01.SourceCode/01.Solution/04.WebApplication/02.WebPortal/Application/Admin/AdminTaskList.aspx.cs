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
    public partial class AdminTaskList : PageBase
    {
        public static bool IsUseV1
        {
            get
            {
                if (System.Configuration.ConfigurationManager.AppSettings["FileSystemMode"].ToLower() == "v1")
                {
                    return true;
                }
                if (System.Configuration.ConfigurationManager.AppSettings["FileSystemMode"].ToLower() == "v2")
                {
                    return false;
                }
                throw new Exception("配置节点FileSystemMode不正确");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.PageName = "全部任务";
            if (!UserInfoOperator.Instance.IsAdmin())
            {
                Response.Write("您无该页面访问权限");
                Response.End();
            }
        }
    }
}