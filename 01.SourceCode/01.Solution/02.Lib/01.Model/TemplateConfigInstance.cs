using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateConfigInstance.
    /// </summary>
    [ORTableMapping("dbo.TemplateConfigInstance")]
    public class TemplateConfigInstance : BaseModel
    {
        [NoMapping]
        public DateTime? SubmitDate { get; set; }
        public bool NotifyStatus { get; set; }

        [NoMapping]
        public List<TemplateConfigInstancePlan> SubTasks { get; set; }

        [NoMapping]
        public List<DateTime> SelectDates { get; set; }

        #region Public Properties
        [ORFieldMapping("PlanHour")]
        public int PlanHour { get; set; }

        [ORFieldMapping("PlanMinute")]
        public int PlanMinute { get; set; }

        /// <summary>
        /// 1常规一次性任务 2周期任务 3子任务
        /// </summary>
        [ORFieldMapping("TaskType")]
        public int TaskType { get; set; }
        /// <summary>
        /// 周期类型  1非周期任务 2每天 3自定义
        /// </summary>
        [ORFieldMapping("CircleType")]
        public int CircleType { get; set; }
        /// <summary>
        /// 计划任务开始时间
        /// </summary>
        [ORFieldMapping("PlanBeginDate", true)]
        public DateTime? PlanBeginDate { get; set; }
        [NoMapping]
        public string PlanBeginDateString
        {
            get
            {
                if (PlanBeginDate.HasValue)
                {
                    return PlanBeginDate.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        [NoMapping]
        public string PlanEndDateString
        {
            get
            {
                if (PlanEndDate.HasValue)
                {
                    return PlanEndDate.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        /// <summary>
        /// 计划任务结束时间
        /// </summary>
        [ORFieldMapping("PlanEndDate", true)]
        public DateTime? PlanEndDate { get; set; }
        /// <summary>
        /// 0 已创建 1 已发起 2 已终止 3 已完成 4 已归档
        /// </summary>
        [ORFieldMapping("ProcessStatus")]
        public int ProcessStatus { get; set; }

        [ORFieldMapping("TemplateID")]
        public string TemplateID { get; set; }



        [ORFieldMapping("TemplateConfigInstanceName")]
        public string TemplateConfigInstanceName { get; set; }



        [ORFieldMapping("WorkflowID")]
        public string WorkflowID { get; set; }



        [ORFieldMapping("WorkflowInfo")]
        public string WorkflowInfo { get; set; }



        [ORFieldMapping("UserName")]
        public string UserName { get; set; }



        [ORFieldMapping("EmployeeName")]
        public string EmployeeName { get; set; }



        [ORFieldMapping("Remark")]
        public string Remark { get; set; }



        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }

        /// <summary>
        /// 格式FileCode+"|"+扩展名（.xls或其它）
        /// </summary>
        [ORFieldMapping("TemplatePath")]
        public string TemplatePath { get; set; }


        /// <summary>
        /// 文件地址
        /// </summary>
        [NoMapping]
        public string TemplatePathFileCode
        {
            get
            {
                if (TemplatePath != null)
                {
                    return TemplatePath.Split('|')[0];
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [NoMapping]
        public string TemplatePathFileExt
        {
            get
            {
                if (TemplatePath != null)
                {
                    var strs = TemplatePath.Split('|');
                    if (strs.Length == 2)
                    {
                        return strs[1];
                    }
                }
                return string.Empty;
            }
        }

        [NoMapping]
        public string CreatorTimeString
        {
            get
            {
                return CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion

        [NoMapping]
        public int TotalTask { get; set; }


        [NoMapping]
        public int CompleteTask { get; set; }
        [NoMapping]
        public int PageCount { get; set; }
        [NoMapping]
        public List<Attachment> Attachments { get; set; }

        #region 用于更新二维表

        /// <summary>
        /// 1：表示原始任务，2：表示更新任务
        /// </summary>
        [ORFieldMapping("TaskTemplateType")]
        public int TaskTemplateType { get; set; }

        #endregion

    }
}

