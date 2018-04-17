using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateConfig.
    /// </summary>
    [ORTableMapping("dbo.TemplateConfig")]
    public class TemplateConfig : BaseModel
    {

        #region Public Properties 



        [ORFieldMapping("TemplateID")]
        public string TemplateID { get; set; }



        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }

        [ORFieldMapping("TemplateSheetID")]
        public string TemplateSheetID { get; set; }



        [ORFieldMapping("TemplateSheetName")]
        public string TemplateSheetName { get; set; }


        [ORFieldMapping("FieldName")]
        public string FieldName { get; set; }



        [ORFieldMapping("FieldType")]
        public string FieldType { get; set; }



        [ORFieldMapping("FiledLength")]
        public int FiledLength { get; set; }



        [ORFieldMapping("SortIndex")]
        public int SortIndex { get; set; }



        [ORFieldMapping("HasSelectedValue")]
        public int HasSelectedValue { get; set; }



        [ORFieldMapping("IsRequired")]
        public int IsRequired { get; set; }



        [ORFieldMapping("Digit")]
        public int Digit { get; set; }



        [ORFieldMapping("BGColor")]
        public string BGColor { get; set; }

        #region 用于设置公式

        /// <summary>
        /// 是否公式
        /// </summary>
        [ORFieldMapping("IsFormula")]
        public int IsFormula { get; set; }

        /// <summary>
        /// 示例公式
        /// </summary>
        [ORFieldMapping("TempFormula")]
        public string TempFormula { get; set; }


        /// <summary>
        /// 通用公式
        /// </summary>
        [ORFieldMapping("CellFormula")]
        public string CellFormula { get; set; }

        #endregion


        #endregion


    }
}

