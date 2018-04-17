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
    public class VCollectUserTaskFilter : PagenationDataFilter, IDataFilter
    {
        [FilterFieldAttribute("TemplateConfigInstanceName", "like")]
        public string TemplateConfigInstanceName { get; set; }

        [FilterFieldAttribute("CreatorTime", ">=")]
        public DateTime CreatorTimeStart { get; set; }

        
        [FilterFieldAttribute("CreatorLoginOrName", "like")]
        public string CreatorLoginOrName { get; set; }

        [FilterFieldAttribute("CreatorLoginName", "=")]
        public string CreatorLoginName { get; set; }

        [FilterFieldAttribute("ProcessStatus", "in")]
        public List<int> StatusList { get; set; }

        [FilterFieldAttribute("TaskType", "in")]
        public List<int> TaskTypeList { get; set; }

        [FilterFieldAttribute("ID", "=")]
        public Guid ID { get; set; }

        [FilterFieldAttribute("unitFullPath", "startwith")]
        public string UnitFullPath { get; set; }
    }
}
