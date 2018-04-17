using WebApplication.WebAPI.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lib.Common;
using Lib.Model;

namespace WebApplication.WebAPI.Models
{
    /// <summary>
    /// 附件
    /// </summary>
    public class Attachment : BaseModel
    {
        /// <summary>
        /// 附件主键ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize { get; set; }
        /// <summary>
        /// 附件名称（不带后缀名）
        /// </summary>
        public string Name { get; set; }
        //public long FileByteLength { get; set; }
        /// <summary>
        /// 文件系统编码
        /// </summary>
        public string FileCode { get; set; }
        /// <summary>
        /// 是否使用第一版本文件平台
        /// </summary>
        public bool IsUseV1 { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        internal void ConvertEntity(Lib.Model.Attachment finder)
        {
            this.BusinessID = finder.BusinessID;
            this.BusinessType = finder.BusinessType;
            this.CreateDate = finder.CreateDate;
            this.CreateLoginName = finder.CreatorLoginName;
            this.CreateName = finder.CreatorName;
            this.FileCode = finder.AttachmentPath;
            this.FileName = finder.Name;
            this.FileSize = finder.FileSize;
            this.Name = finder.FileName;
            this.IsUseV1 = finder.IsUseV1;
            this.ID = Guid.Parse(finder.ID);
        }

        /// <summary>
        /// 业务ID
        /// </summary>
        public string BusinessID { get; set; }
        /// <summary>
        /// 业务类型
        /// </summary>
        public string BusinessType { get; set; }
        /// <summary>
        /// 上传人登陆账号
        /// </summary>
        public string CreateLoginName { get; set; }
        /// <summary>
        /// 上传人姓名
        /// </summary>
        public string CreateName { get; set; }

    }
}