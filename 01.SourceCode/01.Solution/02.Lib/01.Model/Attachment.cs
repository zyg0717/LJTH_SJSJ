using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateAttachment.
    /// </summary>
    [ORTableMapping("dbo.Attachment")]
    public class Attachment : BaseModel
    {

        [NoMapping]
        public string AttachmentFullName
        {
            get
            {
                return Name;
                //if (AttachmentPath != null)
                //{
                //    return string.Format("{0}{1}", Name, AttachmentPath.Substring(AttachmentPath.LastIndexOf(".")));
                //}
                //else
                //{
                //    return string.Empty;
                //}
            }
        }

        #region Public Properties

        [ORFieldMapping("IsUseV1")]
        public bool IsUseV1 { get; set; }

        [ORFieldMapping("BusinessType")]
        public string BusinessType { get; set; }



        [ORFieldMapping("BusinessID")]
        public string BusinessID { get; set; }


        /// <summary>
        /// 文件名（含扩展名）
        /// </summary>
        [ORFieldMapping("AttachmentName")]
        public string Name { get; set; }


        /// <summary>
        /// FileCode
        /// </summary>
        [ORFieldMapping("AttachmentPath")]
        public string AttachmentPath { get; set; }



        [ORFieldMapping("FileSize")]
        public string FileSize { get; set; }


        #endregion


        /// <summary>
        /// 不带后缀名的文件名
        /// </summary>
        [NoMapping]
        public string FileName
        {
            get
            {
                if (Name != null)
                {
                    var dotIndex = Name.LastIndexOf(".");
                    if (dotIndex >= 0)
                    {
                        return Name.Substring(0, Name.LastIndexOf("."));
                    }
                    return Name;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 文件后缀名
        /// </summary>
        [NoMapping]
        public string FileExt
        {
            get
            {
                if (Name != null)
                {
                    var dotIndex = Name.LastIndexOf(".");
                    if (dotIndex >= 0)
                    {
                        return Name.Substring(Name.LastIndexOf("."));
                    }
                    return "";
                }
                return string.Empty;
            }
        }
    }
}

