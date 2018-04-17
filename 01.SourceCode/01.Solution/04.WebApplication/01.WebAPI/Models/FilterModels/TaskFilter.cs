using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.WebAPI.Models.FilterModels
{
    /// <summary>
    /// 任务查询筛选
    /// </summary>
    public class TaskFilter: BaseFilterModel
    {
        
        /// <summary>
        /// 任务标题（模糊匹配）
        /// </summary>
        public string TaskTitle { get; set; }
        /// <summary>
        /// 任务创建人用户名（模糊匹配）
        /// </summary>
        public string TaskCreatorLoginName { get; set; }
        /// <summary>
        /// 是否只自己
        /// </summary>
        public bool IsOnlySelf { get; set; }
        /// <summary>
        /// 时间范围
        /// </summary>
        public int? TimeRange { get; set; }
        /// <summary>
        /// 1当前任务 2归档任务 3计划任务
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 任务状态 0 已创建 1 进行中 2 已终止 3 已完成 4 已归档 全部 null
        /// </summary>
        public int? TaskStatus { get; set; }
    }
}