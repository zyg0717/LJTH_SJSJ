using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Model
{
    [ORTableMapping("dbo.Dept")]
    public class Dept : BaseModel
    {
        [ORFieldMapping("DeptName")]
        public string DeptName { get; set; }
        [ORFieldMapping("OrderLevel")]
        public int OrderLevel { get; set; }
        [ORFieldMapping("ParentID")]
        public string ParentID { get; set; }
        [ORFieldMapping("DeptPath")]
        public string DeptPath { get; set; }
        [ORFieldMapping("DeptFullName")]
        public string DeptFullName { get; set; }
        [ORFieldMapping("BatchTime")]
        public DateTime BatchTime { get; set; }
    }
}
