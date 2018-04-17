using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Data;
using Framework.Data.AppBase;

namespace Lib.Model
{
    [ORTableMapping("dbo.AutoProcess")]
    public class AutoProcess : BaseModel
    {
        [ORFieldMapping("BusinessType")]
        public string BusinessType { get; set; }

        [ORFieldMapping("BusinessID")]
        public string BusinessID { get; set; }

        [ORFieldMapping("Parameters")]
        public string Parameters { get; set; }

        [ORFieldMapping("Status")]
        public int Status { get; set; }

        [ORFieldMapping("ErrorCount")]
        public int ErrorCount { get; set; }

        [ORFieldMapping("ErrorInfo")]
        public string ErrorInfo { get; set; }
    }
}
