using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Lib;
using Framework.Data;
using Quartz;
using Lib.BLL;
using Lib.Model;
using Lib.ViewModel;
using Lib.Common;

namespace ScheduleTask.TaskLib
{
    /// <summary>
    /// 计划任务发起  根据已经存在的计划任务发起收集任务
    /// </summary>
    [Quartz.DisallowConcurrentExecution]
    [Quartz.PersistJobDataAfterExecution]
    class StartTaskService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            LogMgnt.Instance["StartTaskService"].Info("------------计划任务发起服务开始运行-------------");

            try
            {
                var pendingList = TemplateConfigInstancePlanOperator.Instance.GetPendingList(10);

                if (pendingList != null)
                {
                    pendingList.ForEach(x =>
                    {
                        using (TransactionScope scope = TransactionScopeFactory.Create())
                        {
                            //创建任务
                            var targetTemplateConfigInstance =
                                    TemplateConfigInstanceOperator.Instance.GetModel(x.TemplateConfigInstanceID);

                            var dcuList = DataCollectUserOperator.Instance.GetList(targetTemplateConfigInstance.ID);
                            var taskUserList = dcuList.Select(dcu =>
                            {
                                TaskUser taskUser = new TaskUser
                                {
                                    UserName = dcu.UserName,
                                    EmployeeCode = dcu.EmployeeCode,
                                    EmployeeName = dcu.EmployeeName,
                                    OrgID = dcu.OrgID,
                                    OrgName = dcu.OrgName,
                                    UnitID = dcu.UnitID,
                                    UnitName = dcu.UnitName
                                };
                                return taskUser;
                            }).ToList();

                            if (targetTemplateConfigInstance == null)
                            {
                                x.Status = -3;
                                TemplateConfigInstancePlanOperator.Instance.UpdateModel(x);
                                return;
                            }
                            //发起任务
                            targetTemplateConfigInstance.CreateDate = DateTime.Now;
                            targetTemplateConfigInstance.CircleType = 1;
                            targetTemplateConfigInstance.PlanBeginDate = null;
                            targetTemplateConfigInstance.PlanEndDate = null;
                            targetTemplateConfigInstance.TaskType = 3;
                            targetTemplateConfigInstance.ID = Guid.NewGuid().ToString();
                            targetTemplateConfigInstance.TemplateConfigInstanceName += $"-{x.TimeNode:yyyyMMdd}";
                            targetTemplateConfigInstance.ProcessStatus = 0;
                            targetTemplateConfigInstance.NotifyStatus = false;
                            TemplateConfigInstanceOperator.Instance.AddModel(targetTemplateConfigInstance);

                            TemplateConfigInstanceOperator.Instance.UpdateTaskInfo(targetTemplateConfigInstance, taskUserList, false);

                            //更新任务计划发起状态
                            x.Status = 2;
                            x.SubTemplateConfigInstanceID = Guid.Parse(targetTemplateConfigInstance.ID);
                            TemplateConfigInstancePlanOperator.Instance.UpdateModel(x);


                            scope.Complete();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogMgnt.Instance["StartTaskService"].Error("任务执行异常，错误信息{0}，错误堆栈{1}", ex.Message, ex.StackTrace);
            }
            LogMgnt.Instance["StartTaskService"].Info("------------计划任务发起服务结束运行-------------");
        }
    }
}
