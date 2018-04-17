using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.ViewModel.TemplateViewModel
{
    public class DataRangeSetting
    {
        public string name { get; set; }
        public int row { get; set; }
        public int column { get; set; }
    }
    public class PostData
    {
        public string name { get; set; }
        public List<ViewSheet> data { get; set; }
        public int render { get; set; }
        public string TemplateID { get; set; }
        public string SelectTemplateID { get; set; }
    }
    public class TemplateViewModel
    {
        public List<ViewSheet> sheets { get; set; }
    }
    public class ViewSheet
    {
        public bool changeflag { get; set; }
        public bool edit { get; set; }
        public bool select { get; set; }
        public string code { get; set; }
        public int sort { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string remark { get; set; }
        public int firstrow { get; set; }
        public int firstcolumn { get; set; }
        public List<ViewColumn> columns { get; set; }
        public List<ViewRow> rows { get; set; }
    }
    public class ViewRow
    {
        public int sort { get; set; }
        public List<ViewCell> cells { get; set; }
    }
    public class ViewCell
    {
        public int ssort { get; set; }
        public int rsort { get; set; }
        public int csort { get; set; }
        public string content { get; set; }
    }
    public class ViewColumn
    {
        public bool select { get; set; }
        public int sort { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public int digit { get; set; }
        public bool required { get; set; }
        public string range { get; set; }
        public string bgcolor { get; set; }

        /// <summary>
        /// 是否包含公式
        /// </summary>
        public bool isformula { get; set; }

        /// <summary>
        /// 示例公式
        /// </summary>
        public string tempformula { get; set; }

        /// <summary>
        /// 转义后的公式（行用占位符{R}代替）
        /// </summary>
        public string cellformula { get; set; }

    }
}
