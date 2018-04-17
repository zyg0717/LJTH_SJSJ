using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.WebAPI.Models.FilterModels
{
    /// <summary>
    /// 待办、已办过滤器
    /// </summary>
    public class TodoFilter : BaseFilterModel
    {
        /// <summary>
        /// 任务类型 （1代办 2已办）
        /// </summary>
        public int TaskType { get; set; }
        /// <summary>
        /// 任务标题
        /// </summary>
        public string TaskTitle { get; set; }
        /// <summary>
        /// 时间范围 1今天 2过去24小时 3过去一周 4过去一个月 5过去三个月 6过去一年
        /// </summary>
        public int? TimeRange { get; set; }
    }
}