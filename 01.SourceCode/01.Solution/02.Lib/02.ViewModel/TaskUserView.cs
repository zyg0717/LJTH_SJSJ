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
    [ORViewMapping(@"select T.ID as TID,k.ID,T.TemplateConfigInstanceName,k.DataCollectUserID,k.CreatorLoginName,k.CreatorName,k.CreatorTime,k.Status  from dbo.TemplateConfigInstance t join dbo.TemplateTask k on t.ID=k.TemplateConfigInstanceID where t.IsDeleted=0 and k.IsDeleted=0 and k.ProcessStatus=0", "InstanceTaskView")]
    public class TaskUserView : IBaseComposedModel
    {
        [ORFieldMapping("ID")]
        public string ID { set; get; }

        [ORFieldMapping("TemplateConfigInstanceName")]
        public string TemplateConfigInstanceName { set; get; }

        [ORFieldMapping("CreatorLoginName")]
        public string CreatorLoginName { set; get; }
        [ORFieldMapping("DataCollectUserID")]
        public string DataCollectUserID { set; get; }
        [ORFieldMapping("CreatorName")]
        public string CreatorName { set; get; }
        [ORFieldMapping("TID")]
        public string TID { get; set; }

        [ORFieldMapping("CreatorTime")]
        public DateTime CreatorTime { set; get; }
        [NoMapping]
        public int PageCount { get; set; }
        [NoMapping]
        public int Status { get; set; }

        [NoMapping]
        public string CreatorTimeString { get { return CreatorTime == DateTime.MinValue ? "" : CreatorTime.ToString("yyyy-MM-dd HH:mm:ss"); } }


    }
}
