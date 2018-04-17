using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateTask.
    /// </summary>
    [ORTableMapping("dbo.TemplateTask")]
    public class TemplateTask : BaseModel
    {

        #region Public Properties


        /// <summary>
        /// 0为有效数据 1为删除数据 2为作废数据
        /// </summary>
        [ORFieldMapping("ProcessStatus")]
        public int ProcessStatus { get; set; }


        [ORFieldMapping("TemplateConfigInstanceID")]
        public string TemplateConfigInstanceID { get; set; }



        [ORFieldMapping("DataCollectUserID")]
        public string DataCollectUserID { get; set; }



        [ORFieldMapping("EmployeeCode")]
        public string EmployeeCode { get; set; }



        [ORFieldMapping("EmployeeLoginName")]
        public string EmployeeLoginName { get; set; }



        [ORFieldMapping("EmployeeName")]
        public string EmployeeName { get; set; }



        [ORFieldMapping("OrgID")]
        public int OrgID { get; set; }



        [ORFieldMapping("OrgName")]
        public string OrgName { get; set; }

        [ORFieldMapping("FileName")]
        public string FileName { get; set; }


        [ORFieldMapping("FilePath")]
        public string FilePath { get; set; }



        [ORFieldMapping("Content")]
        public string Content { get; set; }

        [ORFieldMapping("SubmitTime")]
        public DateTime SubmitTime { get; set; }

        [ORFieldMapping("AuthTime")]
        public DateTime AuthTime { get; set; }

        [ORFieldMapping("Status")]
        public int Status { get; set; }


        [ORFieldMapping("Remark")]
        public string Remark { get; set; }

        [NoMapping]
        public int PageCount { get; set; }
        [NoMapping]

        public int Tcount { get; set; }

        [NoMapping]

        public string UserJobName { get; set; }
        #endregion

        [NoMapping]
        public string SubmitTimeString { get { return SubmitTime == DateTime.MinValue ? "" : SubmitTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        [NoMapping]
        public string AuthTimeString { get { return AuthTime == DateTime.MinValue ? "" : AuthTime.ToString("yyyy-MM-dd HH:mm:ss"); } }


        #region 二维表更新

        /// <summary>
        /// 任务类型1、数据汇总  2、数据更新
        /// </summary>
        [ORFieldMapping("TaskTemplateType")]
        public int TaskTemplateType { get; set; }
        
        #endregion
    }
}

