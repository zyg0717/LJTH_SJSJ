using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Framework.Data;
using Quartz;
using Lib.BLL;
using Lib.Model;
using ConstSet = Lib.Common.ConstSet;
using Lib.Common;

namespace ScheduleTask.TaskLib
{
    /// <summary>
    /// 任务完成通知服务
    /// </summary>
    [Quartz.DisallowConcurrentExecution]
    [Quartz.PersistJobDataAfterExecution]
    class DoneTaskNotifyService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogMgnt.Instance["DoneTaskNotifyService"].Info("------------任务完成通知服务开始-------------");
            try
            {
                List<TemplateConfigInstance> pendingList =
                    TemplateConfigInstanceOperator.Instance.LoadPendingDoneTaskNotifyList(20);
                if (pendingList != null)
                {
                    pendingList.ForEach(x =>
                    {
                        using (TransactionScope scope = TransactionScopeFactory.Create())
                        {
                            var userName = x.UserName;
                            var EmployeeName = x.EmployeeName;
                            var baseUrl = ConstSet.SiteBaseUrl;
                            var format = ConstSet.CtxNotifyContentFormat;
                            var content = format
                                .Replace("{TaskTitle}", x.TemplateConfigInstanceName)
                                .Replace("{SiteBaseUrl}", baseUrl)
                                .Replace("{TaskID}", x.ID);
                            TSM_Messages message = new TSM_Messages()
                            {
                                ID = Guid.NewGuid().ToString(),
                                Target = userName,
                                Sender = userName,
                                Title = "数据收集任务完成通知",
                                SenderName = EmployeeName,
                                Content = content,
                                Priority = 3,
                                MessageType = MessageTypeEnum.Alert.GetHashCode(), //CTX
                                TargetTime = DateTime.Now,
                                SendTime = DateTime.Now,
                                Status = 0,
                                TryTimes = 0
                            };
                            TSM_MessagesOperator.Instance.AddTSM_Messages(message);
                            x.NotifyStatus = true;
                            TemplateConfigInstanceOperator.Instance.UpdateModel(x);
                            scope.Complete();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                LogMgnt.Instance["DoneTaskNotifyService"].Error("任务执行异常，错误信息{0}，错误堆栈{1}", ex.Message, ex.StackTrace);
            }
            LogMgnt.Instance["DoneTaskNotifyService"].Info("------------任务完成通知服务结束-------------");
        }
    }
}
