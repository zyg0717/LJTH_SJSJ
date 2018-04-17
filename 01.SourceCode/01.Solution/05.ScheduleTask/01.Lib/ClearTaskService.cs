using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Lib.BLL;
using Lib.Model.Filter;

namespace ScheduleTask.TaskLib
{
    /// <summary>
    /// 清理无效人员的计划任务
    /// </summary>
    [Quartz.DisallowConcurrentExecution] 
    [Quartz.PersistJobDataAfterExecution]
    class ClearTaskService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogMgnt.Instance["ClearTaskService"].Info("------------清理无效人员的计划任务开始-------------");
            try
            {
                var pendingList = TemplateConfigInstanceOperator.Instance.LoadClearTaskList(20);
                if (pendingList != null)
                {
                    pendingList.ForEach(x =>
                    {
                        if (x.TaskType == 2 && x.ProcessStatus == 0)
                        {
                            x.ProcessStatus = 2;
                            TemplateConfigInstanceOperator.Instance.UpdateModel(x);
                            int totalCount = 0;
                            //找到所有未发起的计划节点进行作废
                            var plans =
                                    TemplateConfigInstancePlanOperator.Instance.GetList(new TemplateConfigInstancePlanFilter() { TemplateConfigInstanceID = x.ID, Status = 1 },
                                        out totalCount).ToList();
                            plans.ForEach(p =>
                            {
                                p.Status = 4;
                                TemplateConfigInstancePlanOperator.Instance.UpdateModel(p);
                            });
                        }
                        else if (x.TaskType == 3 && x.ProcessStatus == 1)
                        {
                            //调用通用方法处理任务终止
                            TemplateConfigInstanceOperator.Instance.CancelTask(x.ID);
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogMgnt.Instance["ClearTaskService"].Error("任务执行异常，错误信息{0}，错误堆栈{1}", ex.Message, ex.StackTrace);
            }
            LogMgnt.Instance["ClearTaskService"].Info("------------清理无效人员的计划任务完成-------------");
        }
    }
}
