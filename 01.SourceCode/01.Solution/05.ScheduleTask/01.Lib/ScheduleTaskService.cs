using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data;
using Lib.BLL;

namespace ScheduleTask.TaskLib
{
    /// <summary>
    /// 任务调度  用于每日周期任务定时发起当日计划
    /// 每5分钟执行一次
    /// </summary>
    [Quartz.DisallowConcurrentExecution]
    [Quartz.PersistJobDataAfterExecution]
    public class ScheduleTaskService : Quartz.IJob
    {
        public void Execute(Quartz.IJobExecutionContext context)
        {

            LogMgnt.Instance["ScheduleTaskService"].Info("------------任务调度服务开始运行-------------");
            try
            {
                TemplateConfigInstanceOperator.Instance.ScheduleTask(20);
            }
            catch (Exception ex)
            {
                LogMgnt.Instance["ScheduleTaskService"].Error("任务执行异常，错误信息{0}，错误堆栈{1}", ex.Message, ex.StackTrace);
            }

            LogMgnt.Instance["ScheduleTaskService"].Info("------------任务调度服务结束运行-------------");
        }

    }
}
