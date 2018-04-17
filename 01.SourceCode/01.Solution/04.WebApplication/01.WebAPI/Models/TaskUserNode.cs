using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib.ViewModel;

namespace WebApplication.WebAPI.Models
{
    /// <summary>
    /// 任务人员节点
    /// </summary>
    public class TaskUserNode : BaseModel
    {

        //public Guid TaskUserNodeID { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string TaskUserDeparment { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public string TaskUserPosition { get; set; }
        /// <summary>
        /// 节点人登陆账号
        /// </summary>
        public string TaskUserLoginName { get; set; }
        /// <summary>
        /// 节点人姓名
        /// </summary>
        public string TaskUserName { get; set; }
        /// <summary>
        /// 任务接收时间
        /// </summary>
        public DateTime? ReceiveDate { get; set; }
        /// <summary>
        /// 任务审批完成时间
        /// </summary>
        public DateTime? ApproveDate { get; set; }
        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessID { get; set; }
        /// <summary>
        /// 节点状态 转换实体 0 未上报 1 审批中 2 已完成 4 已退回
        /// </summary>
        public int NodeStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        public void ConvertEntity(VCollectUser x)
        {
            this.ApproveDate = x.AuthTime == DateTime.MinValue ? new Nullable<DateTime>() : x.AuthTime;
            this.BusinessID = x.LastTaskID;
            this.ReceiveDate = x.SubmitTime == DateTime.MinValue ? new Nullable<DateTime>() : x.SubmitTime;
            this.TaskUserDeparment = x.UnitName;
            this.TaskUserLoginName = x.UserName;
            this.TaskUserName = x.EmployeeName;
            this.TaskUserPosition = x.JobName;
            this.NodeStatus = x.Status;

            this.AreaValue = x.AreaValue;
            this.UpdateArea = x.UpdateArea;
            this.TaskTemplateTypeVal = x.UpdateArea + "列=" + x.AreaValue;
        }

        #region 用于二维表更新

        public string UpdateArea { get; set; }

        public string AreaValue { get; set; }

        public string TaskTemplateTypeVal { get; set; }
        #endregion
    }
}