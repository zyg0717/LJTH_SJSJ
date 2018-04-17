using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.WebAPI.Models.Helper
{
    /// <summary>
    /// 返回值视图模型 包含当前页数据以及总数据行数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ViewData<T> where T : BaseModel, new()
    {
        /// <summary>
        /// 数据结果
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        /// <summary>
        /// 总记录行数
        /// </summary>
        public long TotalCount { get; set; }
    }
}