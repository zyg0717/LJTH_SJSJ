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
    public class UserFilter : PagenationDataFilter, IDataFilter
    {
        [FilterFieldAttribute("LoginName")]
        public string LoginName { get; set; }

        [FilterFieldAttribute("Name")]
        public string Name { get; set; }

        [FilterFieldAttribute("DisplayName")]
        public string DisplayName { get; set; }

        [FilterFieldAttribute("Department")]
        public string Department { get; set; }

        [FilterFieldAttribute("JobTitle")]
        public string JobTitle { get; set; }

        //[FilterFieldAttribute("Status")]
        //public string Status { get; set; }
        

        [FilterFieldAttribute("employeecode", "in")]
        public List<string> UserIds { get; set; } //根据角色Id查找相应用户

        


        public string DepartmentName { get; set; }
    }
}
