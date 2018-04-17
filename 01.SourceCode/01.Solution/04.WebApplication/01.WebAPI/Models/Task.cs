using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib.ViewModel;

namespace WebApplication.WebAPI.Models
{
    /// <summary>
    /// 任务实体
    /// </summary>
    public class Task : BaseModel
    {
        /// <summary>
        /// 转换附件数据
        /// </summary>
        /// <param name="attachments"></param>
        public void ConvertAttachment(List<Lib.Model.Attachment> attachments)
        {
            this.Attachments = attachments.Select(x =>
            {
                var attachment = new Attachment();
                attachment.ConvertEntity(x);
                //attachment.BusinessID = x.BusinessID;
                //attachment.BusinessType = x.BusinessType;
                //attachment.CreateDate = x.CreateDate;
                //attachment.CreateLoginName = x.CreatorLoginName;
                //attachment.CreateName = x.CreatorName;
                //attachment.FileCode = x.AttachmentPath;
                //attachment.FileName = x.FileName;
                //attachment.FileSize = x.FileSize;
                //attachment.ID = Guid.Parse(x.ID);
                return attachment;
            }).ToList();
        }
        /// <summary>
        /// 转换时间节点方法 ** 调用该方法前需要先调用同一实例的ConvertEntity方法
        /// </summary>
        /// <param name="timeNodes"></param>
        public void ConvertTimeNodes(List<Lib.Model.TemplateConfigInstancePlan> timeNodes)
        {
            if (this.TaskType == 2 && (this.TaskCircleType == 3 || this.TaskCircleType == 2))
            {
                this.TaskTimeNodes = timeNodes.Select(x =>
                {
                    var timeNode = new TaskTimeNode();
                    timeNode.OwnTaskID = x.TemplateConfigInstanceID;
                    timeNode.SubTaskID = x.SubTemplateConfigInstanceID.ToString();
                    timeNode.TimeNode = x.TimeNode;
                    timeNode.TimeNodeStatus = x.Status;
                    return timeNode;
                }).ToList();
            }
        }
        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void ConvertEntity(VCollectUserTask model)
        {
            if (model == null)
            {
                return;
            }
            this.TaskTemplateAttachmentID = model.TemplateAttachmentID;
            this.Attachments = new List<Attachment>();
            this.TaskTimeNodes = new List<TaskTimeNode>();
            this.TaskName = model.TemplateConfigInstanceName;
            this.TaskUserNodeCount = model.Total;
            this.CompleteUserNodeCount = model.AuCount;
            this.TaskType = model.TaskType;
            this.IsFiling = model.ProcessStatus == 4;
            this.TaskCreateDate = model.CreatorTime;
            this.TaskAttachmentCount = model.Tcount;
            this.TaskID = model.ID;
            this.TaskTakingTime = model.TaskTakingTime;
            this.TaskStatus = model.ProcessStatus;
            this.TaskTemplateName = model.TemplateName;
            this.NextTaskApplyDate = model.NextTaskApplyDate;
            this.TaskTemplateID = Guid.Parse(model.TemplateID);
            string needApprovalWorkflowID = System.Configuration.ConfigurationManager.AppSettings["ApprovalWorkflowID"];
            this.IsNeedApprove = needApprovalWorkflowID.Equals(model.WorkflowID, StringComparison.CurrentCultureIgnoreCase);
            this.Remark = model.Remark;
            this.TaskCircleType = model.CircleType;
            this.TaskCreatorLoginName = model.CreatorLoginName;
            this.TaskCreatorName = model.CreatorName;
            this.TaskSubmitDate = model.SubmitDate;
            if (this.TaskType == 2)
            {
                this.PlanStart = model.PlanBeginDate;
                this.PlanEnd = model.PlanEndDate;
                this.PlanHour = model.PlanHour;
                this.PlanMinute = model.PlanMinute;
            }
            if (this.TaskType==3)
            {
                this.OwnerTaskID = model.OwnerTaskID;
            }
            this.TaskTemplateType = model.TaskTemplateType;
        }
        /// <summary>
        /// 下次任务发起时间（仅在计划任务是显示）
        /// </summary>
        public DateTime? NextTaskApplyDate { get; set; }
        /// <summary>
        /// 任务耗时
        /// </summary>
        public int TaskTakingTime { get; set; }
        /// <summary>
        /// 任务附件数量
        /// </summary>
        public int TaskAttachmentCount { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string TaskName { set; get; }
        /// <summary>
        /// 任务创建时间
        /// </summary>
        public DateTime? TaskCreateDate { get; set; }
        /// <summary>
        /// 任务类型 1 非周期性任务 2 周期性任务 3 子任务
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 是否已归档
        /// </summary>
        public bool IsFiling { get; set; }
        /// <summary>
        /// 下发子任务数量
        /// </summary>
        public int TaskUserNodeCount { get; set; }
        /// <summary>
        /// 已完成子任务数量
        /// </summary>
        public int CompleteUserNodeCount { get; set; }
        /// <summary>
        /// 任务状态 0 已创建 1 已发起 2 已终止 3 已完成 4 已归档
        /// </summary>
        public int TaskStatus { get; set; }
        /// <summary>
        /// 任务模板名称
        /// </summary>
        public string TaskTemplateName { get; set; }
        /// <summary>
        /// 任务模板ID
        /// </summary>
        public Guid TaskTemplateID { get; set; }
        /// <summary>
        /// 是否需要审批
        /// </summary>
        public bool IsNeedApprove { get; set; }
        /// <summary>
        /// 任务要求
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 相关附件 **仅在详情查看时进行显示
        /// </summary>
        public List<Attachment> Attachments { get; set; }
        /// <summary>
        /// 周期类型  1非周期任务 2每天 3自定义
        /// </summary>
        public int TaskCircleType { get; set; }
        /// <summary>
        /// 周期任务执行开始时间 非周期任务该值为null taskType=2 并且 circleType=2 
        /// </summary>
        public DateTime? PlanStart { get; set; }
        /// <summary>
        /// 周期任务执行结束时间 非周期任务该值为null taskType=2 并且 circleType=2 
        /// </summary>
        public DateTime? PlanEnd { get; set; }
        /// <summary>
        /// 周期任务 任务执行小时数 taskType=2
        /// </summary>
        public int PlanHour { get; set; }
        /// <summary>
        /// 周期任务 任务执行分钟数 taskType=2
        /// </summary>
        public int PlanMinute { get; set; }
        /// <summary>
        /// 周期任务 时间节点集合 （该集合仅在TaskType=2 并且（TaskCircleType=3 or 并且TaskCircleType=2） 时有数据）
        /// **仅在详情查看时进行显示 保存及提交时需提交该集合
        /// </summary>
        public List<TaskTimeNode> TaskTimeNodes { get; set; }
        /// <summary>
        /// 任务人员节点集合
        /// </summary>
        public List<TaskUserNode> UserNodes { get; set; }
        /// <summary>
        /// 任务创建人名称
        /// </summary>
        public string TaskCreatorName { get; set; }
        /// <summary>
        /// 任务创建人账号
        /// </summary>
        public string TaskCreatorLoginName { get; set; }
        /// <summary>
        /// 任务提交时间（计划任务的子任务有效）
        /// </summary>
        public DateTime? TaskSubmitDate { get; set; }
        /// <summary>
        /// 任务所选模板附件ID
        /// </summary>
        public string TaskTemplateAttachmentID { get; set; }
        /// <summary>
        /// 计划任务实例所属主任务ID
        /// </summary>
        public string OwnerTaskID { get; set; }

        /// <summary>
        /// 当值等于2时，用于更新二维表
        /// </summary>
        public int TaskTemplateType { get; set; }
    }
}