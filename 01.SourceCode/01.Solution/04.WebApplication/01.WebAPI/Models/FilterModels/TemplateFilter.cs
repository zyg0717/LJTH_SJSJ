using WebApplication.WebAPI.Models.Base;
using WebApplication.WebAPI.Models.FilterModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.WebAPI.Models.FilterModels
{
    /// <summary>
    /// 模板筛选
    /// </summary>
    public class TemplateFilter : BaseFilterModel
    {
        /// <summary>
        /// 模板名称 （模糊匹配）
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// 模板创建人账号或姓名
        /// </summary>
        public string TemplateCreateLoginOrName { get; set; }
        /// <summary>
        /// 创建时间范围 1今天 2过去24小时 3过去一周 4过去一个月 5过去三个月 6过去一年
        /// </summary>
        public int? TimeRange { get; set; }
        /// <summary>
        /// 是否仅自己
        /// </summary>
        public bool IsOnlySelf { get; set; }
        /// <summary>
        /// 是否加载在线导入模板（列表查询要选择true 在模板新增页面中的克隆模板点击后展现的列表中该字段为false）
        /// </summary>
        public bool WithImport { get; set; }
    }
}