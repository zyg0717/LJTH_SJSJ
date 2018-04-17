using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib.ViewModel;
using Plugin.OAMessage;

namespace WebApplication.WebAPI.Models
{
    /// <summary>
    /// 代办实体
    /// </summary>
    public class Todo : BaseModel
    {
        /// <summary>
        /// 任务类型
        /// </summary>
        public int TaskAction { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessID { get; set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        public string TaskTitle { get; set; }
        /// <summary>
        /// 创建人账号
        /// </summary>
        public string CreatorLoginName { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string CreatorName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 接收时间
        /// </summary>
        public DateTime ReceiveDate { get; set; }
        /// <summary>
        /// 下发人账号
        /// </summary>
        public string AssignLoginName { get; set; }
        /// <summary>
        /// 下发人姓名
        /// </summary>
        public string AssignName { get; set; }
        /// <summary>
        /// 下发时间
        /// </summary>
        public DateTime AssignDate { get; set; }
        /// <summary>
        /// 填报文件
        /// </summary>
        public Attachment TaskAttachment { get; set; }
        /// <summary>
        /// 填报说明
        /// </summary>
        public string TaskRemark { get; set; }
        /// <summary>
        /// 上报说明 页面下面的
        /// </summary>
        public string TaskReportRemark { get; set; }
        /// <summary>
        /// 任务相关附件
        /// </summary>
        public List<Attachment> TaskAttachments { get; set; }
        /// <summary>
        /// 相关附件 页面下面的
        /// </summary>
        public List<Attachment> TaskReportAttachments { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        public string TemplateID { get; set; }
        /// <summary>
        /// 是否需要审批
        /// </summary>
        public bool IsNeedApprove { get; set; }
        /// <summary>
        /// 任务状态 0 未上报 1 审批中 2 批准 4 退回
        /// </summary>
        public int TaskStatus { get; set; }
        /// <summary>
        /// 接收人姓名
        /// </summary>
        public string ReceiveName { get; set; }
        /// <summary>
        /// 接收人账号
        /// </summary>
        public string ReceiveLoginName { get; set; }
        /// <summary>
        /// 流程编码
        /// </summary>
        public string WorkflowCode { get; set; }
        /// <summary>
        /// 流程上报人
        /// </summary>
        public string EmployeeLoginName { get; set; }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="todo"></param>
        /// <param name="taskAction"></param>
        public void ConvertEntity(VTaskTodo todo, int taskAction = 0)
        {
            this.AssignDate = todo.AssignDate;
            this.AssignLoginName = todo.AssignLoginName;
            this.AssignName = todo.AssignName;
            this.TaskAttachments = new List<Attachment>();
            this.TaskReportAttachments = new List<Attachment>();
            this.BusinessID = todo.BusinessID;
            this.CreateDate = todo.CreatorTime;
            this.CreatorLoginName = todo.CreatorLoginName;
            this.CreatorName = todo.CreatorName;
            this.ReceiveDate = todo.ReceiveDate;
            this.TaskAttachment = null;
            this.TaskRemark = todo.TaskRemark;
            this.TaskTitle = todo.TaskTitle;
            this.TaskStatus = todo.TaskStatus;
            this.TemplateID = todo.TemplateID;
            this.TaskID = todo.TaskID;
            this.ReceiveName = todo.ReceiveName;
            this.ReceiveLoginName = todo.ReceiveLoginName;
            this.EmployeeLoginName = todo.EmployeeLoginName;
            this.TaskReportRemark = todo.TaskReportRemark;
            this.WorkflowCode = todo.WorkflowID;
            this.TaskAction = taskAction;
            string needApprovalWorkflowID = System.Configuration.ConfigurationManager.AppSettings["ApprovalWorkflowID"];
            this.IsNeedApprove = needApprovalWorkflowID.Equals(todo.WorkflowID, StringComparison.CurrentCultureIgnoreCase);
        }


      
        /// <summary>
        /// 转换任务附件
        /// </summary>
        /// <param name="attachment"></param>
        public void ConvertTaskAttachment(Lib.Model.Attachment attachment)
        {
            if (attachment != null)
            {
                this.TaskAttachment = new Attachment();
                this.TaskAttachment.ConvertEntity(attachment);
            }
        }
        /// <summary>
        /// 转换附件集合
        /// </summary>
        /// <param name="attachments"></param>
        public void ConvertTaskAttachmentList(List<Lib.Model.Attachment> attachments)
        {
            this.TaskAttachments = attachments.Select(x =>
              {
                  var attachment = new Attachment();
                  attachment.ConvertEntity(x);
                  return attachment;
              }).ToList();
        }
        /// <summary>
        /// 转换相关附件集合
        /// </summary>
        /// <param name="attachments"></param>
        public void ConvertTaskReportAttachmentList(List<Lib.Model.Attachment> attachments)
        {
            this.TaskReportAttachments = attachments.Select(x =>
            {
                var attachment = new Attachment();
                attachment.ConvertEntity(x);
                return attachment;
            }).ToList();
        }
    }
}