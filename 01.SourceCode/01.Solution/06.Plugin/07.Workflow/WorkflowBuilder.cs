using BPF.Workflow.Client;
using BPF.Workflow.Object;
using Lib.Common;
using Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.Workflow
{
    public static class WorkflowBuilder
    {
        //public static void ReadWorkflowProcess(string businessID, Employee lastUser)
        //{
        //    OAMessage.OAMessageBuilder.ReceiveRead(businessID, "填报", lastUser.LoginName);
        //}
        /// <summary>
        /// 启动流程
        /// </summary>
        /// <param name="flowCode">流程编号</param>
        /// <param name="businessID">业务id</param>
        /// <param name="startUser">下发人</param>
        /// <param name="lastUser">接收人</param>
        /// <param name="flowTitle">流程标题</param>
        /// <param name="approvalContent">备注</param>
        public static void StartWorkflowProcess(string flowCode, string businessID, Employee startUser, Employee lastUser, string flowTitle, string approvalContent)
        {

            //给接收人下发待办任务
            //string url = "/Application/Task/TaskCollection.aspx?businessId=" + businessID;
            //OAMessage.OAMessageBuilder.ReceiveTodo(businessID, flowTitle, flowCode, "填报", url, url, lastUser.LoginName, startUser.LoginName);


            //如果任务发起人或任务处理人任务状态为非正常状态 则不发起工作流程
            if (
                !startUser.IsNormalEmployeeStatus
                ||
                !lastUser.IsNormalEmployeeStatus
                )
            {
                return;
            }
            WFStartupParameter startParam = new WFStartupParameter();
            startParam.BusinessID = businessID;
            startParam.CurrentUser = new UserInfo() { UserLoginID = startUser.LoginName };
            startParam.FlowCode = flowCode;
            startParam.ProcessTitle = flowTitle;
            startParam.FormParams = new Dictionary<string, object>();
            startParam.FormParams.Add("TaskName", flowTitle);
            startParam.DynamicRoleUserList = new Dictionary<string, List<UserInfo>>();
            startParam.DynamicRoleUserList.Add("DyRole", new List<UserInfo>() { new UserInfo() { UserLoginID = startUser.LoginName } });
            startParam.DynamicRoleUserList.Add("ApprovalRole", new List<UserInfo>() { new UserInfo() { UserLoginID = lastUser.LoginName } });
            WorkflowContext workflow = WFClientSDK.CreateProcess(null, startParam);
            if (workflow.StatusCode != 0)
            {
                throw workflow.LastException;
            }

            BizContext bizContext = new BizContext();
            bizContext.NodeInstanceList = workflow.NodeInstanceList;
            bizContext.CcNodeInstanceList = workflow.CcNodeInstanceList;
            bizContext.ProcessRunningNodeID = workflow.ProcessInstance.RunningNodeID;
            bizContext.FormParams = startParam.FormParams;
            bizContext.DynamicRoleUserList = startParam.DynamicRoleUserList;
            bizContext.FlowCode = flowCode;
            bizContext.BusinessID = businessID;
            bizContext.ApprovalContent = approvalContent;
            bizContext.ProcessTitle = flowTitle;
            bizContext.CurrentUser = new UserInfo() { UserLoginID = startUser.LoginName };
            WorkflowContext result = WFClientSDK.ExecuteMethod("SaveProcess", bizContext);

            if (result.StatusCode != 0)
            {
                throw result.LastException;
            }

        }
        /// <summary>
        /// 撤销流程
        /// </summary>
        /// <param name="businessID">业务id</param>
        /// <param name="currentUser">撤销操作人</param>
        /// <param name="approvalContent">备注</param>
        public static void CancelWorkflowProcess(string businessID, Employee currentUser, string approvalContent)
        {
            //OAMessage.OAMessageBuilder.CancelProcess(businessID);


            WorkflowContext workflow = WFClientSDK.GetProcess(null, businessID, currentUser.LoginName);
            if (workflow.StatusCode != 0)
            {
                //业务ID不存在并且当前人已离职
                if (workflow.StatusCode == 211 && !currentUser.IsNormalEmployeeStatus)
                {
                    return;
                }
                throw workflow.LastException;
            }

            if (workflow.ProcessInstance.Status == 3 || workflow.ProcessInstance.Status == -1) return;


            var todoList = WFTaskSDK.QueryToDoByBusinessID(businessID);
            if (todoList.TotalCount > 0)
            {
                var todo = todoList.TaskList.Find(f => f.TaskAction == 1 || f.TaskAction == 2);
                if (todo != null)
                {
                    currentUser = new Employee()
                    {
                        LoginName = todo.User.UserLoginID
                    };
                }
            }
            //判断 如果当前代办人是离职等状态  强制调用运维工具方法进行流程作废
            if (!currentUser.IsNormalEmployeeStatus)
            {
                //获取管理员登陆账号
                currentUser = new Employee()
                {
                    LoginName = ConstSet.AdminUserName
                };
                CallMaintenanceMethod("RejectOrCancel", new { ProcessID = workflow.ProcessInstance.ProcessID, Type = "CancelProcess" }, currentUser.LoginName);
            }
            BizContext bizContext = new BizContext();

            bizContext.ProcessRunningNodeID = workflow.ProcessInstance.RunningNodeID;
            bizContext.BusinessID = workflow.BusinessID;
            bizContext.ApprovalContent = approvalContent;
            bizContext.CurrentUser = new UserInfo() { UserLoginID = currentUser.LoginName };

            WorkflowContext result = WFClientSDK.ExecuteMethod("CancelProcess", bizContext);
            if (result.StatusCode != 0)
            {
                throw result.LastException;
            }
        }

        public static bool CallMaintenanceMethod(string methodName, object param, string ApplyUser)
        {
            string result = CallWebService(methodName, Newtonsoft.Json.JsonConvert.SerializeObject(param), ApplyUser);

            var resultBase = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultBaseContext>(result);
            if (resultBase.IsSuccess)
            {
                string data = Newtonsoft.Json.JsonConvert.SerializeObject(resultBase.Data);
                return true;
            }
            else
            {
                throw resultBase.LastException;
            }
            return false;
        }

        private static string CallWebService(string method, string param, string ApplyUser)
        {
            var dicParamWebService = BuildParamWebService(method, param, ApplyUser);
            try
            {

                string url = AppSettingInfo.WorkflowServerUrl;
                string workFlowServerFullURL = SDKHelper.GetWorkflowServerUrlFullPath(url);
                var result = SDKHelper.QueryPostWebService(workFlowServerFullURL, "CommonHandler", dicParamWebService);
                return result;
            }
            catch (Exception ex)
            {
                SDKSystemSupportException newEX = new SDKSystemSupportException(ClientConstDefine.WORKFLOW_SERVICE_ERRORCODE_USERSELECT_SERVERWEBSERVICEERROR
                    , ClientConstDefine.WORKFLOW_SERVICE_ERRORCONTENT_USERSELECT_SERVERWEBSERVICEERROR
                    );
                throw (Exception)ex;
            }

        }


        private static Dictionary<string, object> BuildParamWebService(string method, string param, string ApplyUser)
        {
            string version = "1.0";
            string token = Guid.NewGuid().ToString();
            //将appCode，action，method，param（序列化后的dicParam）添加到dicParamWebService中调用WebService
            return SDKHelper.BuildParamWebService("ProcessMaintenance", method, token, Newtonsoft.Json.JsonConvert.SerializeObject(
                new
                {
                    Version = version,
                    Param = param,
                    CurrentUserLoginID = ApplyUser,
                    BizAppCode = AppSettingInfo.ApplicationCode
                }), "");
        }
        ///// <summary>
        ///// 审批流程
        ///// </summary>
        ///// <param name="businessID">业务id</param>
        ///// <param name="flowCode">流程编码</param>
        ///// <param name="flowTitle">流程标题</param>
        ///// <param name="currentUser">当前审批人</param>
        ///// <param name="approvalContent">审批意见</param>
        //public static void ApproveWorkflowProcess(string businessID, string flowCode, string flowTitle, Employee currentUser, string approvalContent)
        //{
        //    --
        //    //由于没有审批流程 只要点了调用了该方法就说明待办已经完成 直接更新成办结就行了

        //    string url = "/Application/Task/TaskCollectionView.aspx?businessId=" + businessID;
        //    OAMessage.OAMessageBuilder.ReceiveOver(businessID, flowTitle, "填报", currentUser.LoginName, url, url);
        //}
    }
}
