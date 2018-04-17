using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;

namespace Plugin.UserSelect
{
    /// <summary>
    /// This object represents the properties and methods of a WD_User.
    /// </summary>
    [ORTableMapping("dbo.V_Employee")]
    public class VEmployeeEntity : IBaseComposedModel
    {
        #region Public Properties

        [ORFieldMapping("employeeID")]
        public string EmployeeID { get; set; }

        [ORFieldMapping("Username")]
        public string LoginName { get; set; }

        [ORFieldMapping("EmployeeName")]
        public string DisplayName { get; set; }

        [ORFieldMapping("EmployeeCode")]
        public string EmployeeCode { get; set; }

        [ORFieldMapping("UnitName")]
        public string Department { get; set; }

        [ORFieldMapping("ActualUnitName")]
        public string ActualUnitName { get; set; }

        [ORFieldMapping("JobName")]
        public string JobTitle { get; set; }

        [ORFieldMapping("EmployeeStatus")]
        public int EmployeeStatus { get; set; }


        /// <summary>
        /// 头像大图
        /// </summary>
        [ORFieldMapping("AvatarPath")]
        public string AvatarPath { get; set; }
        /// <summary>
        ///头像缩略图
        [ORFieldMapping("Thumb")]
        /// </summary>
        public string Thumb { get; set; }


        #endregion
    }
}
