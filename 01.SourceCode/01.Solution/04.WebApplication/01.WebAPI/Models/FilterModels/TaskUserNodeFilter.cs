using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.WebAPI.Models.FilterModels
{
    /// <summary>
    /// 任务人员节点筛选
    /// </summary>
    public class TaskUserNodeFilter
    {
        /// <summary>
        /// 姓名或用户名
        /// </summary>
        public string LoginOrName { get; set; }
        /// <summary>
        /// 人员节点状态 0 全部  1已完成  2进行中
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 任务ID
        /// </summary>
        public Guid TaskID { get; set; }
    }
}