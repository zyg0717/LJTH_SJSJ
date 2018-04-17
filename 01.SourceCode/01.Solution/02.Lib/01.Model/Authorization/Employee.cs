using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;
using System.Configuration;

namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a WD_User.
    /// </summary>
    [ORTableMapping("dbo.V_Employee")]
    public class Employee : BaseNodeModel
    {
        #region Public Properties

        [ORFieldMapping("username")]
        public string LoginName { get; set; }

        [ORFieldMapping("employeeName")]
        public string DisplayName { get; set; }

        [ORFieldMapping("employeeCode")]
        public string EmployeeCode { get; set; }

        [ORFieldMapping("unitName")]
        public string Department { get; set; }

        [ORFieldMapping("jobName")]
        public string JobTitle { get; set; }


        [ORFieldMapping("EmployeeStatus")]
        public int EmployeeStatus { get; set; }

        public bool IsNormalEmployeeStatus
        {
            get
            {
                var normalEmployeeStatusList = ConfigurationManager.AppSettings["NormalEmployeeStatus"].Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
                return normalEmployeeStatusList.Any(x => x == EmployeeStatus);
            }
        }

        #endregion
    }
}
