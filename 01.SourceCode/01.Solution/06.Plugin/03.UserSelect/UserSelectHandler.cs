using Framework.Web.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Plugin.UserSelect
{
    public class UserSelectHandler : IHttpHandler
    {
        /// <summary>
        /// 您将需要在网站的 Web.config 文件中配置此处理程序 
        /// 并向 IIS 注册它，然后才能使用它。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //在此处写入您的处理程序实现。
            var request = context.Request;
            var cmd_type = request["request_type"];
            var result = "";

            switch (cmd_type)
            {
                case "init_tree":
                    result = InitTree(request);
                    break;
                case "init_tree_pre":
                    result = InitParentTree(request);
                    break;
                case "init_tree_all":
                    result = InitAllTree(request);
                    break;
                case "init_tree_user":
                    result = InitTreeUser(request);
                    break;
                case "search_user":
                    result = SearchUser(request);
                    break;

            }
            context.Response.ClearContent();
            context.Response.Write(result);
            context.Response.End();
        }

        private string InitAllTree(HttpRequest request)
        {
            var pid = request["pid"].ToString();
            var parentId = 0;
            if (!string.IsNullOrEmpty(pid) && pid != "-1")
            {
                parentId = Convert.ToInt32(pid);
            }
            var searchKey = request["key"];
            List<VDeptEntity> children = null;
            List<VEmployeeEntity> users = null;

            var bySearch = request["bysearch"].ToString() == "1" ? true : false;


            var previous = VDeptOperator.Instance.LoadPreDeptList(parentId);


            if (bySearch)
            {
                users = VEmployeeOperator.Instance.LoadUserList(searchKey);
                children = new List<VDeptEntity>();
            }
            else
            {
                users = VEmployeeOperator.Instance.LoadUserList(parentId);
                children = VDeptOperator.Instance.LoaDeptList(parentId);
            }


            return JsonHelper.Serialize(new
            {
                Children = children,
                Previous = previous,
                Users = users
            });
        }

        private string InitParentTree(HttpRequest request)
        {
            var id = request["id"].ToString();
            var oid = 0;
            if (!string.IsNullOrEmpty(id) && id != "-1")
            {
                oid = Convert.ToInt32(id);
            }
            var result = VDeptOperator.Instance.LoadPreDeptList(oid);
            return JsonHelper.Serialize(result);
        }

        private string SearchUser(HttpRequest request)
        {
            var employeeName = request["employee_name"];
            var dept = request["dept"];
            var job = request["job"];
            var userName = request["user_name"];
            var keyWord = request["key_word"];
            var result = VEmployeeOperator.Instance.LoadUserList(employeeName, dept, job, userName, keyWord);
            return JsonHelper.Serialize(result);
        }

        private string InitTreeUser(HttpRequest request)
        {
            var deptId = Convert.ToInt32(request["deptid"]);
            var result = VEmployeeOperator.Instance.LoadUserList(deptId);
            return JsonHelper.Serialize(result);
        }

        private string InitTree(HttpRequest request)
        {
            var pid = request["pid"].ToString();
            var parentId = 0;
            if (!string.IsNullOrEmpty(pid) && pid != "-1")
            {
                parentId = Convert.ToInt32(pid);
            }
            var result = VDeptOperator.Instance.LoaDeptList(parentId);
            return JsonHelper.Serialize(result);
        }


        #endregion
    }
}
