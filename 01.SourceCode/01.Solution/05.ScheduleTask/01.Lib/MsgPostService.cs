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
    /// 消息发送服务
    /// </summary>
    [Quartz.DisallowConcurrentExecution]
    [Quartz.PersistJobDataAfterExecution]
    class MsgPostService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogMgnt.Instance["MsgPostService"].Info("------------消息发送服务开始运行-------------");

            LogMgnt.Instance["MsgPostService"].Info("------------消息发送服务结束运行-------------");
        }
    }
}
