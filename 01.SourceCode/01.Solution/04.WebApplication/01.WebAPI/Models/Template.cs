using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib.ViewModel;
using Lib.ViewModel.TemplateViewModel;

namespace WebApplication.WebAPI.Models
{
    /// <summary>
    /// 模板实体
    /// </summary>
    public class Template : BaseModel
    {
        /// <summary>
        /// 附件ID
        /// </summary>
        public string AttachmentId { get; set; }
        /// <summary>
        /// 模板sheet集合
        /// </summary>
        public List<ViewSheet> sheets { get; set; }

        /// <summary>
        /// 模板名称
        /// </summary>
        public string TemplateName { get; set; }
        /// <summary>
        /// 是否私有模板 （对应使用范围 true对应私有模板 false对应公有模板）
        /// </summary>
        public bool IsPrivate { get; set; }
        /// <summary>
        /// 模板创建人账号
        /// </summary>
        public string CreateLoginName { get; set; }
        /// <summary>
        /// 模板创建人姓名
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string TemplateID { get; set; }
        /// <summary>
        /// 是否为本地模板导入
        /// </summary>
        public bool IsImport { get; set; }

        internal void ConvertEntity(VTemplate model)
        {
            this.CreateDate = model.CreatorTime;
            this.CreateLoginName = model.CreatorLoginName;
            this.CreateName = model.CreatorName;
            this.IsPrivate = model.IsPrivate == 1;
            this.TemplateID = model.ID;
            this.TemplateName = model.TemplateName;
            this.IsImport = model.IsImport == 1;
            this.AttachmentId = model.AttachmentID;
        }
    }
}