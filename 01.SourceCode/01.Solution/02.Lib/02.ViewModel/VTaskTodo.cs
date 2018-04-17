using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.ViewModel
{
    [Serializable]
    [ORViewMapping(@"(SELECT 
TT.EmployeeLoginName ReceiveLoginName,
TT.EmployeeName ReceiveName,
TT.Status TaskStatus,
TCI.TemplateID TemplateID,
TCI.WorkflowID WorkflowID,
TCI.ID TaskID,
TT.ID BusinessID,TCI.TemplateConfigInstanceName TaskTitle,TT.CreatorLoginName CreatorLoginName,TT.CreatorName CreatorName,TT.CreatorTime CreatorTime, TT.CreatorTime ReceiveDate,
TCI.CreatorLoginName AssignLoginName,TCI.CreatorName AssignName,TCI.CreatorTime AssignDate,TCI.Remark TaskRemark,TT.Remark TaskReportRemark,TT.EmployeeLoginName EmployeeLoginName

 FROM dbo.TemplateTask TT

 INNER JOIN dbo.TemplateConfigInstance TCI ON tt.TemplateConfigInstanceID=TCI.ID

 WHERE tt.IsDeleted=0 AND tt.ProcessStatus=0 AND TCI.IsDeleted=0 ) "
            , "VTaskTodo")]
    public class VTaskTodo : IBaseComposedModel
    {
        public string EmployeeLoginName { get; set; }
        /// <summary>
        /// 接收人姓名
        /// </summary>
        public string ReceiveName { get; set; }
        /// <summary>
        /// 接受人账号
        /// </summary>
        public string ReceiveLoginName { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public string TaskID { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public int TaskStatus { get; set; }
        /// <summary>
        /// 流程编码
        /// </summary>
        public string WorkflowID { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string TemplateID { get; set; }

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
        public DateTime CreatorTime { get; set; }
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
        /// 填报说明
        /// </summary>
        public string TaskRemark { get; set; }
        /// <summary>
        /// 上报说明
        /// </summary>
        public string TaskReportRemark { get; set; }

    }
}
