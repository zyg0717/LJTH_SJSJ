using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Plugin.UserSelect
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateAttachment.
    /// </summary>
    [ORTableMapping("dbo.VDept")]
    public class VDeptEntity : BaseModel
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
        /// <summary>
        /// 是否有下级节点
        /// </summary>
        [ORFieldMapping("isParent")]
        public bool isParent { get; set; }
    }
}

