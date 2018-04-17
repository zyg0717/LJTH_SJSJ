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
    class SubmitTaskService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {

            LogMgnt.Instance["SubmitTaskService"].Info("------------计划任务提交服务开始运行-------------");

            try
            {
                var pendingList = TemplateConfigInstancePlanOperator.Instance.GetPendingSubmitList(10);

                if (pendingList != null)
                {
                    pendingList.ForEach(x =>
                    {
                        using (TransactionScope scope = TransactionScopeFactory.Create())
                        {
                            var finder = TemplateConfigInstanceOperator.Instance.GetModel(x.SubTemplateConfigInstanceID.ToString());
                            if (finder.ProcessStatus == 0)
                            {
                                finder.ProcessStatus = 1;
                                TemplateConfigInstanceOperator.Instance.UpdateModel(finder);
                                var orgCollectUserList = DataCollectUserOperator.Instance.GetList(finder.ID);
                                var taskUserList = orgCollectUserList.Select(task => new TaskUser()
                                {
                                    EmployeeCode = task.EmployeeCode,
                                    OrgID = task.OrgID,
                                    UnitID = task.UnitID,
                                    UnitName = task.UnitName,
                                    EmployeeName = task.EmployeeName,
                                    OrgName = task.OrgName,
                                    UserName = task.UserName
                                }).ToList();
                                TemplateConfigInstanceOperator.Instance.UpdateTaskInfo(finder, taskUserList, true);
                            }
                            x.Status = 3;
                            TemplateConfigInstancePlanOperator.Instance.UpdateModel(x);

                            scope.Complete();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogMgnt.Instance["SubmitTaskService"].Error("任务执行异常，错误信息{0}，错误堆栈{1}", ex.Message, ex.StackTrace);
            }
            LogMgnt.Instance["SubmitTaskService"].Info("------------计划任务提交服务结束运行-------------");
        }
    }
}
