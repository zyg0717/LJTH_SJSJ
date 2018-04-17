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
    [ORViewMapping(@"select t.* from (
select  
     a.*,b.tRange 
 from  
     (select *,Range1=convert(xml,' <root> <v>'+replace(Range,',',' </v> <v>')+' </v> </root>') from Template where IsDeleted=0 and IsPrivate=0)a 
 outer apply 
     (select tRange=C.v.value('.','nvarchar(100)') from a.Range1.nodes('/root/v')C(v))b ) t
where t.tRange in (
select orgid from dbo.wd_org
)", "TaskOrgView")]
    public class TempTaskOrgView : IBaseComposedModel
    {
        #region Public Properties

        [ORFieldMapping("ID")]
        public string ID { get; set; }


        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }

        [ORFieldMapping("CreatorName")]
        public string CreatorName { get; set; }

         [ORFieldMapping("CreatorLoginName")]
        public string CreatorLoginName { get; set; }

        [ORFieldMapping("TemplateNo")]
        public string TemplateNo { get; set; }

         [ORFieldMapping("CreatorTime")]
        public DateTime CreatorTime { get; set; }

        [ORFieldMapping("OrgID")]
        public int OrgID { get; set; }



        [ORFieldMapping("OrgName")]
        public string OrgName { get; set; }



        [ORFieldMapping("UnitID")]
        public int UnitID { get; set; }



        [ORFieldMapping("UnitName")]
        public string UnitName { get; set; }



        [ORFieldMapping("ActualUnitID")]
        public int ActualUnitID { get; set; }



        [ORFieldMapping("ActualUnitName")]
        public string ActualUnitName { get; set; }



        [ORFieldMapping("IsPrivate")]
        public int IsPrivate { get; set; }



        [ORFieldMapping("Range")]
        public string Range { get; set; }



        [ORFieldMapping("TemplatePath")]
        public string TemplatePath { get; set; }



        [ORFieldMapping("Status")]
        public int Status { get; set; }



        [NoMapping]
        public string CreateTimeString
        {
            get
            {
                return CreatorTime.ToString("yyyy年MM月dd日");
            }
        }

        #endregion
    }
}
