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
    public class TemplateConfigInstancePlanFilter : PagenationDataFilter, IDataFilter
    {
        [FilterFieldAttribute("TemplateConfigInstanceID")]
        public string TemplateConfigInstanceID { get; set; }

        [FilterFieldAttribute("SubTemplateConfigInstanceID")]
        public string SubTemplateConfigInstanceID { get; set; }
        

        [FilterFieldAttribute("Status")]
        public int Status { get; set; }
    }
}
