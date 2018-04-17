using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a DataCollectUser.
    /// </summary>
    [ORTableMapping("dbo.DataCollectUser")]
    public class DataCollectUser : BaseModel
    {

        #region Public Properties 



        [ORFieldMapping("ProcessStatus")]
        public int ProcessStatus { get; set; }

        [ORFieldMapping("TemplateConfigInstanceID")]
        public string TemplateConfigInstanceID { get; set; }


        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }



        [ORFieldMapping("TemplateID")]
        public string TemplateID { get; set; }



        [ORFieldMapping("EmployeeCode")]
        public string EmployeeCode { get; set; }



        [ORFieldMapping("UserName")]
        public string UserName { get; set; }



        [ORFieldMapping("EmployeeName")]
        public string EmployeeName { get; set; }



        [ORFieldMapping("OrgID")]
        public int OrgID { get; set; }



        [ORFieldMapping("OrgName")]
        public string OrgName { get; set; }



        [ORFieldMapping("UnitID")]
        public int UnitID { get; set; }



        [ORFieldMapping("UnitName")]
        public string UnitName { get; set; }


        #endregion
        [NoMapping]
        public string JobName { get; set; }


        [NoMapping]
        public string SubmitTimeString { get; set; }

        [NoMapping]
        public string AuthTimeString { get; set; }

        [NoMapping]
        public string LastTaskID { get; set; }
        [NoMapping]
        public int PageCount { get; set; }
        [NoMapping]
        public int TCount { get; set; }

        #region 用于二维表更新

        /// <summary>
        /// 1：表示原始任务，2：表示更新任务
        /// </summary>
        [ORFieldMapping("TaskTemplateType")]
        public int TaskTemplateType { get; set; }

        /// <summary>
        /// 更新区域
        /// </summary>
        [ORFieldMapping("UpdateArea")]
        public string UpdateArea { get; set; }

        /// <summary>
        /// 区域值
        /// </summary>
        [ORFieldMapping("AreaValue")]
        public string AreaValue { get; set; }
        #endregion

    }
}

