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
    public class DataCollectUserFilter : PagenationDataFilter, IDataFilter
    {
        [FilterFieldAttribute("EmployeeName")]
        public string EmployeeName { get; set; }

        [FilterFieldAttribute("UnitName")]
        public string UnitName { get; set; }

        [FilterFieldAttribute("(select count(1) from TemplateTask where DataCollectUserID=DataCollectUser.ID and ProcessStatus=0 and isdeleted=0 and Status=2)")]
        public int FeedBack { get; set; }


        [FilterFieldAttribute("TemplateConfigInstanceID")]
        public string TemplateConfigInstanceID { get; set; }
    }
}
