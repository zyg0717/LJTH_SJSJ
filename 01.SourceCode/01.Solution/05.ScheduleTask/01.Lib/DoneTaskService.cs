using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Lib.BLL;

namespace ScheduleTask.TaskLib
{
    /// <summary>
    /// 任务完成状态更新服务
    /// </summary>
    [Quartz.DisallowConcurrentExecution]
    [Quartz.PersistJobDataAfterExecution]
    class DoneTaskService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogMgnt.Instance["DoneTaskService"].Info("------------任务完成状态更新服务开始-------------");
            try
            {
                TemplateConfigInstanceOperator.Instance.DoneTask();
            }
            catch (Exception ex)
            {
                LogMgnt.Instance["DoneTaskService"].Error("任务执行异常，错误信息{0}，错误堆栈{1}", ex.Message, ex.StackTrace);
            }
            LogMgnt.Instance["DoneTaskService"].Info("------------任务完成状态更新服务结束-------------");
        }
    }
}
