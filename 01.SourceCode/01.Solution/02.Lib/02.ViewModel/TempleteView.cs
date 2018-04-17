using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.ViewModel
{
    public class TempleteView
    {
        public bool Edit { get; set; }

        public string templateID { get; set; }
        public string TemplateName { get; set; }
        public bool IsPrivate { get; set; }
        public int IsImport { get; set; }
        public string RangeName { get; set; }
        public string Range { get; set; }
        public string OrgName { get; set; }
        public string UnitName { get; set; }
        public string ActualUnitName { get; set; }

        public string CNName { get; set; }

        public string AttachName { get; set; }

        public string AttachID { get; set; }

        public List<TempleteSheet> sheets { get; set; }
    }

    public class TempleteSheet
    {
        public string TemplateID { get; set; }
        public string TemplateName { get; set; }
        public string TemplateSheetID { get; set; }
        public string TemplateSheetName { get; set; }
        public string TemplateSheetTitle { get; set; }
        public string TemplateSheetRemark { get; set; }
        public int ColumnNum { get; set; }
        public int RowNum { get; set; }

        public List<TemplateConfig> TemplateConfigs { get; set; }
    }

    public class TemplateConfig
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public int FiledLength { get; set; }
        public int SortIndex { get; set; }
        public bool HasSelectedValue { get; set; }
        public bool IsRequired { get; set; }
        public int Digit { get; set; }
        public string BGColor { get; set; }
        public string RangeValue { get; set; }

        public List<Model.Enum> DataTypes { get; set; }

        public List<Model.Enum> BGColors { get; set; }
    }
}
