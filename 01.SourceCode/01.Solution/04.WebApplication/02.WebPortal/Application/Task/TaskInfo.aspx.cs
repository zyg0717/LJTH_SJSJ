using WebApplication.WebPortal.SiteMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication.WebPortal.Application.Task
{
    public partial class TaskInfo : PageBase
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
            this.PageName = "任务管理";
        }
    }
}