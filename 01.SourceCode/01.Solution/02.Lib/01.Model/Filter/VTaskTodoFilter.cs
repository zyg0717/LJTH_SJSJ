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
    public class VTaskTodoFilter : PagenationDataFilter, IDataFilter
    {

        
        [FilterFieldAttribute("BusinessID", "IN")]
        public List<string> BusinessIDS { get; set; }
        [FilterFieldAttribute("TaskTitle", "like")]
        public string TaskTitle { get; set; }

        [FilterFieldAttribute("CreatorTime", ">=")]
        public DateTime CreatorTimeStart { get; set; }

        [FilterFieldAttribute("EmployeeLoginName", "=")]
        public string EmployeeLoginName { get; set; }
    }
}
