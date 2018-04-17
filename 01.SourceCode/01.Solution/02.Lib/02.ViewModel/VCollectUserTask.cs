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
    [ORViewMapping(
         @"


SELECT  

ti.TaskTemplateType,
U.unitFullPath, 

TCIP.TemplateConfigInstanceID as OwnerTaskID,TCIP.SenderTime SubmitDate,ti.ProcessStatus,ti.ID,ti.TemplateID,t.TemplateName,ti.CreatorTime,
ti.CreatorLoginName,t.CreatorName,(t.CreatorLoginName+'!@#$'+t.CreatorName) CreatorLoginOrName,ti.TemplateConfigInstanceName,ti.TaskType,ti.CircleType,ti.PlanBeginDate,ti.PlanEndDate,ti.PlanHour,ti.PlanMinute,
ti.WorkflowID
,
(
CASE WHEN TI.TaskType<>2
THEN (
(
CASE 
WHEN ISNULL(ti.ProcessStatus,0)=0 THEN 0 
WHEN ISNULL(ti.ProcessStatus,0)=1 THEN  DATEDIFF( SECOND, 


(CASE WHEN ti.TaskType=3 THEN TCIP.SenderTime ELSE  ti.CreatorTime END)
, GETDATE() )
WHEN ISNULL(ti.ProcessStatus,0)=2 THEN 0
WHEN ISNULL(ti.ProcessStatus,0)=3 THEN DATEDIFF( SECOND, 

(CASE WHEN ti.TaskType=3 THEN TCIP.SenderTime ELSE  ti.CreatorTime END)

, (SELECT MAX(AuthTime) FROM dbo.TemplateTask TT WITH(NOLOCK) WHERE TT.TemplateConfigInstanceID=TI.ID AND TT.IsDeleted=0 AND TT.ProcessStatus=0 AND TT.Status=2 ))
WHEN ISNULL(ti.ProcessStatus,0)=4  THEN  
(
CASE   WHEN 
(SELECT COUNT(1) FROM dbo.TemplateTask TT WITH(NOLOCK) WHERE TT.TemplateConfigInstanceID=ti.ID AND tt.ProcessStatus=0 AND tt.Status<>2 AND tt.IsDeleted=0 )>0
THEN DATEDIFF( SECOND, 
(CASE WHEN ti.TaskType=3 THEN TCIP.SenderTime ELSE  ti.CreatorTime END)

, GETDATE() )
ELSE DATEDIFF( SECOND, 
(CASE WHEN ti.TaskType=3 THEN TCIP.SenderTime ELSE  ti.CreatorTime END)

, (SELECT ISNULL(MAX(AuthTime),GETDATE()) FROM dbo.TemplateTask TTI WITH(NOLOCK) WHERE TTI.TemplateConfigInstanceID=TI.ID AND TTI.IsDeleted=0 AND TTI.ProcessStatus=0 AND TTI.Status=2 ))
END
)
END
)
)ELSE
0
END
)
 TaskTakingTime,
(
SELECT TOP 1 ID FROM dbo.Attachment A  WITH(NOLOCK) WHERE A.BusinessID=CAST(t.ID AS NVARCHAR(36)) AND A.BusinessType='UploadModelAttach' ORDER BY CreatorTime DESC
)
 TemplateAttachmentID,
 (
 CASE WHEN TaskType=2 THEN 
 (
 SELECT MIN(TCIP.SenderTime) FROM dbo.TemplateConfigInstancePlan TCIP WITH(NOLOCK) WHERE TCIP.IsDeleted=0 AND TCIP.TemplateConfigInstanceID=TI.ID
 AND TCIP.TimeNode>=GETDATE()
 )
 ELSE NULL END
 )

  NextTaskApplyDate,
ti.Remark,

(select Count(1) from TemplateTask  WITH(NOLOCK) WHERE TemplateConfigInstanceID=ti.ID and IsDeleted=0 and Status<>3 and ProcessStatus=0) AS Total,
(select Count(1) from TemplateTask  WITH(NOLOCK) where TemplateConfigInstanceID=ti.ID and IsDeleted=0 and Status=2 and ProcessStatus=0) AS AuCount,
(select count(1) from Attachment  WITH(NOLOCK) where BusinessType='UploadTaskAttach' and isdeleted=0 
AND BusinessID 
IN (select ID from TemplateTask tt WITH(NOLOCK) where tt.isdeleted=0 and tt.TemplateConfigInstanceID=ti.ID and tt.ProcessStatus=0 and tt.Status=2)
) as TCount
from TemplateConfigInstance ti  WITH(NOLOCK)
INNER JOIN Template t on ti.TemplateID=t.ID
LEFT JOIN TemplateConfigInstancePlan TCIP
on ti.ID=TCIP.SubTemplateConfigInstanceID and TCIP.IsDeleted=0
INNER JOIN dbo.V_Employee U ON ti.CreatorLoginName=U.username
where ti.IsDeleted=0 and t.IsDeleted=0 

", "VCollectTask")]
    public class VCollectUserTask : IBaseComposedModel
    {
        [NoMapping]
        public int DayDiff
            => ((TimeSpan)(DateTime.Today - new DateTime(CreatorTime.Year, CreatorTime.Month, CreatorTime.Day))).Days;

        [ORFieldMapping("OwnerTaskID")]
        public string OwnerTaskID { get; set; }
        
        [ORFieldMapping("TemplateAttachmentID")]
        public string TemplateAttachmentID { get; set; }

        [ORFieldMapping("NextTaskApplyDate", true)]
        public DateTime? NextTaskApplyDate { get; set; }

        [ORFieldMapping("TaskTakingTime")]
        public int TaskTakingTime { get; set; }
        [ORFieldMapping("SubmitDate")]
        public DateTime? SubmitDate { get; set; }
        [ORFieldMapping("Remark")]
        public string Remark { get; set; }

        [ORFieldMapping("WorkflowID")]
        public string WorkflowID { get; set; }

        //[ORFieldMapping("SelectDates")]
        //public string SelectDates { get; set; }

        [ORFieldMapping("PlanBeginDate")]
        public DateTime PlanBeginDate { set; get; }

        [ORFieldMapping("PlanEndDate")]
        public DateTime PlanEndDate { set; get; }

        [ORFieldMapping("PlanHour")]
        public int PlanHour { set; get; }

        [ORFieldMapping("PlanMinute")]
        public int PlanMinute { set; get; }

        [ORFieldMapping("TaskType")]
        public int TaskType { set; get; }

        [ORFieldMapping("CircleType")]
        public int CircleType { set; get; }

        [ORFieldMapping("ProcessStatus")]
        public int ProcessStatus { set; get; }

        [ORFieldMapping("ID")]
        public string ID { set; get; }

        [ORFieldMapping("TemplateID")]
        public string TemplateID { set; get; }

        [ORFieldMapping("TemplateName")]
        public string TemplateName { set; get; }

        [ORFieldMapping("CreatorName")]
        public string CreatorName { set; get; }

        [ORFieldMapping("CreatorLoginName")]
        public string CreatorLoginName { set; get; }


        [ORFieldMapping("TemplateConfigInstanceName")]
        public string TemplateConfigInstanceName { set; get; }


        //[ORFieldMapping("SendTime")]
        //public DateTime SendTime { set; get; }

        [ORFieldMapping("CreatorTime")]
        public DateTime CreatorTime { set; get; }


        [ORFieldMapping("Total")]
        public int Total { get; set; }

        [ORFieldMapping("AuCount")]
        public int AuCount { get; set; }

        [NoMapping]
        public int Status { get; set; }

        [NoMapping]
        public int PageCount { get; set; }

        [NoMapping]
        public string DataCollectUserID { get; set; }

        [ORFieldMapping("Tcount")]
        public int Tcount { get; set; }

        [NoMapping]
        public int TempTcount { get; set; }

        //[NoMapping]
        //public string SendTimeString
        //{
        //    get { return SendTime == DateTime.MinValue ? "" : SendTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        //}


        [NoMapping]
        public string CreatorTimeString
        {
            get { return CreatorTime == DateTime.MinValue ? "" : CreatorTime.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        [NoMapping]
        public string PlanBeginDateString
        {
            get { return PlanBeginDate.ToString("yy/MM/dd"); }
        }

        [NoMapping]
        public string PlanEndDateString
        {
            get { return PlanEndDate.ToString("yy/MM/dd"); }
        }


        #region 用于更新二维表

        /// <summary>
        /// 1：表示原始任务，2：表示更新任务
        /// </summary>
        [ORFieldMapping("TaskTemplateType")]
        public int TaskTemplateType { get; set; }

        #endregion
    }
}
