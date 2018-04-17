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
    [ORViewMapping(@"select ti.ProcessStatus,ti.ID,ti.TemplateID,t.TemplateName,ti.CreatorTime as SendTime,ti.CreatorTime,
ti.CreatorLoginName,t.CreatorName,ti.TemplateConfigInstanceName,
(select Count(1) from TemplateTask where TemplateConfigInstanceID=ti.ID and IsDeleted=0 and Status<>3 and ProcessStatus=0) AS Total,
(select Count(1) from TemplateTask where TemplateConfigInstanceID=ti.ID and IsDeleted=0 and Status=2 and ProcessStatus=0) AS AuCount,
(select count(1) from Attachment where BusinessType='UploadTaskAttach' and isdeleted=0 and BusinessID in (select ID from TemplateTask tt where tt.isdeleted=0 and tt.TemplateConfigInstanceID=ti.ID and tt.ProcessStatus=0 and tt.Status=2)) as TCount
from TemplateConfigInstance ti
INNER JOIN Template t on ti.TemplateID=t.ID
where ti.IsDeleted=0 and t.IsDeleted=0
", "CollectTaskView")]
    public class CollectUserTaskView : IBaseComposedModel
    {

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


        [ORFieldMapping("SendTime")]
        public DateTime SendTime { set; get; }

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

        [NoMapping]
        public string SendTimeString { get { return SendTime == DateTime.MinValue ? "" : SendTime.ToString("yyyy-MM-dd HH:mm:ss"); } }


        [NoMapping]
        public string CreatorTimeString { get { return CreatorTime == DateTime.MinValue ? "" : CreatorTime.ToString("yyyy-MM-dd HH:mm:ss"); } }

    }
}
