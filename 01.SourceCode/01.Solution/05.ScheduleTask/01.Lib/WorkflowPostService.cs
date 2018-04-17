using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Framework.Web.Security.Authentication;
using Newtonsoft.Json.Linq;
using Quartz;
using Lib.BLL;
using Lib.Common;
using Plugin.Workflow;

namespace ScheduleTask.TaskLib
{
    /// <summary>
    /// 工作流发起流程
    /// </summary>
    [Quartz.DisallowConcurrentExecution]
    [Quartz.PersistJobDataAfterExecution]
    class WorkflowPostService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            LogMgnt.Instance["WorkflowPostService"].Info("------------工作流发起服务开始运行-------------");

            var pendingList = AutoProcessOperator.Instance.GetWaitingStartWorkflowList(2);
            pendingList.ForEach(x =>
            {
                try
                {
                    var json = JObject.Parse(x.Parameters);
                    switch (x.BusinessType)
                    {
                        case "StartProcess":
                            {
                                var flowCode = json["flowCode"].ToString();
                                var taskID = json["taskID"].ToString();
                                var taskTitle = json["taskTitle"].ToString();
                                var approvalContent = json["approvalContent"].ToString();
                                var startUser = json["startUser"]["LoginName"].ToString();
                                var lastUser = json["lastUser"]["LoginName"].ToString();
                                var startUserEntity = UserInfoOperator.Instance.GetWDUserInfoByLoginName(startUser);
                                var lastUserEntity = UserInfoOperator.Instance.GetWDUserInfoByLoginName(startUser);

                                WorkflowBuilder.StartWorkflowProcess(flowCode, taskID, new Lib.Model.Employee()
                                {
                                    LoginName = startUserEntity.LoginName,
                                    EmployeeStatus = startUserEntity.EmployeeStatus
                                }, new Lib.Model.Employee()
                                {
                                    LoginName = lastUserEntity.LoginName,
                                    EmployeeStatus = lastUserEntity.EmployeeStatus
                                }, taskTitle, approvalContent);
                            }
                            break;
                        case "CancelProcess":
                            {
                                var taskID = json["taskID"].ToString();
                                var currentUser = json["currentUser"]["LoginName"].ToString();
                                var approvalContent = json["approvalContent"].ToString();

                                var currentUserEntity = UserInfoOperator.Instance.GetWDUserInfoByLoginName(currentUser);
                                WorkflowBuilder.CancelWorkflowProcess(taskID, new Lib.Model.Employee()
                                {
                                    LoginName = currentUserEntity.LoginName,
                                    EmployeeStatus = currentUserEntity.EmployeeStatus
                                }, approvalContent);
                                //WorkflowBuilder.CancelWorkflowProcess();
                            }
                            break;
                    }
                    x.Status = 1;
                    AutoProcessOperator.Instance.UpdateAutoproces(x);
                }
                catch (Exception ex)
                {
                    x.Status = -1;
                    x.ErrorCount += 1;
                    x.ErrorInfo += ex.Message;
                    AutoProcessOperator.Instance.UpdateAutoproces(x);
                }

            });

            LogMgnt.Instance["WorkflowPostService"].Info("------------工作流发起服务结束运行-------------");
        }
    }
}
