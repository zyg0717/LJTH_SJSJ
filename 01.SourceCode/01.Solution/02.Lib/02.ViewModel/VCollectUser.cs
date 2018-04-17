using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.ViewModel
{
    [Serializable]
    [ORViewMapping(@" SELECT tt.ID as LastTaskID
             ,tt.SubmitTime
             ,tt.AuthTime
             ,ISNULL(tt.[Status],0) as DataStatus
            ,(
            select count(1) from Attachment  where BusinessType='UploadTaskAttach'
             and BusinessID=tt.ID and IsDeleted=0
            ) as TCount
			,u.jobName JobName 
            ,dcu.*
             FROM  dbo.DataCollectUser AS dcu
             LEFT JOIN dbo.TemplateTask tt ON tt.DataCollectUserID=dcu.ID AND tt.TemplateConfigInstanceID=dcu.TemplateConfigInstanceID AND tt.IsDeleted=0
			 INNER JOIN dbo.V_Employee u ON u.username=dcu.UserName
             WHERE dcu.IsDeleted=0  
             AND  (
			 EXISTS
             (
	            SELECT DataCollectUserID,TemplateConfigInstanceID,MAX(itt.CreatorTime) AS CreateDate
	            FROM  dbo.TemplateTask AS itt 
	            WHERE itt.IsDeleted=0 AND itt.ProcessStatus=0
	            AND itt.TemplateConfigInstanceID=tt.TemplateConfigInstanceID AND itt.DataCollectUserID=tt.DataCollectUserID AND itt.CreatorTime=tt.CreatorTime
	            GROUP BY DataCollectUserID,TemplateConfigInstanceID
             )
			 OR tt.TemplateConfigInstanceID IS NULL
			 )
            ", "VCollectUser")]
    public class VCollectUser : IBaseComposedModel
    {
        [ORFieldMapping("JobName")]
        public virtual string JobName { get; set; }

        [ORFieldMapping("CreatorTime")]
        public virtual DateTime CreateDate { get; set; }


        [ORFieldMapping("CreatorLoginName")]
        public virtual string CreatorLoginName { get; set; }

        [ORFieldMapping("CreatorName")]
        public virtual string CreatorName { get; set; }

        [ORFieldMapping("ModifierLoginName")]
        public virtual string ModifierLoginName { get; set; }

        [ORFieldMapping("ModifierName")]
        public virtual string ModifierName { get; set; }


        [SqlBehavior(BindingFlags = ClauseBindingFlags.Insert | ClauseBindingFlags.Update | ClauseBindingFlags.Select | ClauseBindingFlags.Where)]
        public virtual DateTime ModifyTime { get; set; }


        [ORFieldMapping("TCount")]
        public int TCount { get; set; }

        [ORFieldMapping("LastTaskID")]
        public string LastTaskID { set; get; }

        [ORFieldMapping("ID")]
        public string ID { set; get; }

        [ORFieldMapping("SubmitTime")]
        public DateTime SubmitTime { set; get; }

        [ORFieldMapping("AuthTime")]
        public DateTime AuthTime { set; get; }

        [ORFieldMapping("DataStatus")]
        public int Status { set; get; }

        [NoMapping]
        public string SubmitTimeString { get { return SubmitTime == DateTime.MinValue ? "" : SubmitTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

        [NoMapping]
        public string AuthTimeString { get { return AuthTime == DateTime.MinValue ? "" : AuthTime.ToString("yyyy-MM-dd HH:mm:ss"); } }



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
