using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a Template.
    /// </summary>
    [ORTableMapping("dbo.Template")]
    public class Template : BaseModel
    {

        #region Public Properties


        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }



        [ORFieldMapping("CreatorName")]
        public string CreatorName { get; set; }

        [ORFieldMapping("TemplateNo")]
        public string TemplateNo { get; set; }



        [ORFieldMapping("OrgID")]
        public int OrgID { get; set; }



        [ORFieldMapping("OrgName")]
        public string OrgName { get; set; }



        [ORFieldMapping("UnitID")]
        public int UnitID { get; set; }



        [ORFieldMapping("UnitName")]
        public string UnitName { get; set; }



        [ORFieldMapping("ActualUnitID")]
        public int ActualUnitID { get; set; }



        [ORFieldMapping("ActualUnitName")]
        public string ActualUnitName { get; set; }



        [ORFieldMapping("IsPrivate")]
        public int IsPrivate { get; set; }



        [ORFieldMapping("Range")]
        public string Range { get; set; }


        /// <summary>
        /// 格式FileCode+"|"+扩展名（.xls或其它）
        /// </summary>
        [ORFieldMapping("TemplatePath")]
        public string TemplatePath { get; set; }



        [ORFieldMapping("Status")]
        public int Status { get; set; }

        [ORFieldMapping("CreatorTime")]
        public DateTime CreatorTime { get; set; }

        [ORFieldMapping("IsImport")]
        public int IsImport { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        [NoMapping]
        public string TemplatePathFileCode
        {
            get
            {
                if (TemplatePath != null)
                {
                    return TemplatePath.Split('|')[0];
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [NoMapping]
        public string TemplatePathFileExt
        {
            get
            {
                if (TemplatePath != null)
                {
                    var strs = TemplatePath.Split('|');
                    if (strs.Length == 2)
                    {
                        return strs[1];
                    }
                }
                return string.Empty;
            }
        }

        [NoMapping]
        public int PageCount { get; set; }

        [NoMapping]
        public int Count { get; set; }

        [NoMapping]
        public string CreateTimeString
        {
            get
            {
                return CreatorTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

       

        #endregion


    }
}

