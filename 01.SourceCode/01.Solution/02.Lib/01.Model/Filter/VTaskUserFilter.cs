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
    public class VTaskUserFilter : PagenationDataFilter, IDataFilter
    {

        
        [FilterFieldAttribute("ID", "IN")]
        public List<string> BusinessIDS { get; set; }
        [FilterFieldAttribute("TemplateConfigInstanceName", "like")]
        public string TemplateConfigInstanceName { get; set; }

        [FilterFieldAttribute("CreatorTime", ">=")]
        public DateTime CreatorTimeStart { get; set; }

        [FilterFieldAttribute("CreatorLoginName", "=")]
        public string CreatorLoginName { get; set; }
    }
}
