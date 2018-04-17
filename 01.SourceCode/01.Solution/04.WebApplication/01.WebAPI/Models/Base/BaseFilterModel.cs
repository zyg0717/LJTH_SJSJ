using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.WebAPI.Models.Base
{
    /// <summary>
    /// 筛选器基础类
    /// </summary>
    public class BaseFilterModel
    {
        /// <summary>
        /// 行索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页码
        /// </summary>
        public int PageSize { get; set; }
    }
}