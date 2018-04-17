using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateConfigInstance.
    /// </summary>
    [ORTableMapping("dbo.TemplateConfigInstancePlan")]
    public class TemplateConfigInstancePlan : BaseModel
    {

        [ORFieldMapping("TemplateConfigInstanceID")]
        public string TemplateConfigInstanceID { get; set; }

        [ORFieldMapping("TimeNode")]
        public DateTime TimeNode { get; set; }

        [ORFieldMapping("SenderTime")]
        public DateTime SenderTime { get; set; }

        [ORFieldMapping("SubTemplateConfigInstanceID", true)]
        public Guid SubTemplateConfigInstanceID { get; set; }

        /// <summary>
        /// 1������ 2���ύ 3���ύ 4������
        /// </summary>
        [ORFieldMapping("Status")]
        public int Status { get; set; }

    }
}

