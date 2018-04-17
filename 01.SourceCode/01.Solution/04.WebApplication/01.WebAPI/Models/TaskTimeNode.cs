using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.WebAPI.Models
{
    /// <summary>
    /// 任务时间节点实体
    /// </summary>
    public class TaskTimeNode : BaseModel
    {
        /// <summary>
        /// 节点时间
        /// </summary>
        public DateTime? TimeNode { get; set; }
        /// <summary>
        /// 节点状态 1待发起 2待提交 3已提交 4已作废
        /// </summary>
        public int TimeNodeStatus { get; set; }
        /// <summary>
        /// 节点所属计划任务
        /// </summary>
        public string OwnTaskID { get; set; }
        /// <summary>
        /// 节点实例任务
        /// </summary>
        public string SubTaskID { get; set; }
    }
}