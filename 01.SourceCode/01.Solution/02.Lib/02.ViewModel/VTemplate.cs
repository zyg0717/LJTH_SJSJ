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
    [ORViewMapping(@"
SELECT T.*,U.unitFullPath,

(SELECT TOP 1 ID FROM dbo.Attachment A WHERE A.BusinessID=CAST(t.ID AS NVARCHAR(36)) AND A.BusinessType='UploadModelAttach' ORDER BY CreatorTime DESC) AttachmentID,
(
CASE WHEN T.IsPrivate=0
THEN 
(
 STUFF((SELECT ','+deptName FROM Dept WHERE ID IN  (select str2table from StrToTable(T.Range) )
FOR XML PATH(''))
,1, 1, '') 
)
ELSE 
'私有模板'
END
) AS OrgScope,
(T.CreatorLoginName+'!@#$'+T.CreatorName) CreatorLoginOrName
 FROM Template T 
  INNER JOIN dbo.V_Employee U
  ON T.CreatorLoginName=U.username
 WHERE T.IsDeleted=0


", "TemplateView")]
    public class VTemplate : IBaseComposedModel
    {
        [ORFieldMapping("AttachmentID")]
        public string AttachmentID { get; set; }

        [ORFieldMapping("CreatorTime")]
        public DateTime CreatorTime { get; set; }

        [ORFieldMapping("ID")]
        public string ID { get; set; }

        /// <summary>
        /// 模板适用范围
        /// </summary>
        [ORFieldMapping("OrgScope")]
        public string OrgScope { get; set; }

        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }



        [ORFieldMapping("CreatorName")]
        public string CreatorName { get; set; }

        [ORFieldMapping("CreatorLoginName")]
        public string CreatorLoginName { get; set; }

        [ORFieldMapping("TemplateNo")]
        public string TemplateNo { get; set; }



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

        /// <summary>
        /// 格式FileCode+"|"+扩展名（.xls或其它）
        /// </summary>
        [ORFieldMapping("TemplatePath")]
        public string TemplatePath { get; set; }

        [ORFieldMapping("Status")]
        public int Status { get; set; }


        [ORFieldMapping("IsImport")]
        public int IsImport { get; set; }

    }
}
