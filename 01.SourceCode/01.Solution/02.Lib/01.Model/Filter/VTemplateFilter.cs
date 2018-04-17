using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Model.Filter
{
    [Serializable]
    public class VTemplateFilter : PagenationDataFilter, IDataFilter
    {
        [FilterFieldAttribute("TemplateName", "like")]
        public string TemplateName { get; set; }

        [FilterFieldAttribute("CreatorTime", ">=")]
        public DateTime CreatorTimeStart { get; set; }

        [FilterFieldAttribute("CreatorLoginName", "=")]
        public string CreatorLoginName { get; set; }

        [FilterFieldAttribute("IsImport", "=", true)]
        public int? IsImport { get; set; }

        [FilterFieldAttribute("CreatorLoginOrName", "like")]
        public string CreatorLoginOrName { get; set; }

        [FilterFieldAttribute("ID", "=")]
        public string TemplateID { get; set; }

        [FilterFieldAttribute("unitFullPath", "startwith")]
        public string UnitFullPath { get; set; }
    }
}
