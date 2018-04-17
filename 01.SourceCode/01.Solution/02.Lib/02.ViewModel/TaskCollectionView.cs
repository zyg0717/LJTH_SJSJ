using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Model;

namespace Lib.ViewModel
{
    public class TaskCollectionView
    {
        public string BusinessID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDone { get; set; }

        public bool IsApprove { get; set; }

        public bool IsNeedApprove { get; set; }

        public string CollectionUserID { get; set; }

        public string FlowName { get; set; }

        public string CreatorName { get; set; }
        public string CreatorLoginName { get; set; }
        public string CreatorUnit { get; set; }

        public string Requirement { get; set; }

        public string TempleteID { get; set; }

        public string TempleteName { get; set; }

        public string TempletePath { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public string FileSize { get; set; } 

        public string Content { get; set; }

        public string CreaTime { get; set; }
        public string TemplateConfigInstanceName { get; set; }

        public int Status { get; set; }

        public bool Useable { get; set; }

        public string ReContent { get; set; }


        public List<Attachment> Attachments { get; set; }
        public bool HasTaskFile { get; set; }
        public string TaskFileName { get; set; }
        public string TaskOverName { get; set; }
        public string OverTime { get; set; }
    }


    public class TaskCollectionData
    {
        public List<DataSheet> Sheets { get; set; }
    }
    public class DataSheet
    {
        public string SheetName { get; set; }


        public List<DataRows> Rows { get; set; }
    }
    public class DataRows
    {
        public List<RowCells> Cells { get; set; }
        public string UserName { get; set; }
        public string LoginName { get; set; }
        public string OrgName { get; set; }
    }

    public class RowCells
    {
        public int Index { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }

        public int TotalRow { get; set; }

        public int TotalColumn { get; set; }

        public bool NeedNewColumn { get; set; }

        /// <summary>
        /// 表示单元格的值是不是通过公式计算出来的
        /// </summary>
        public bool IsFormula { get; set; }

        /// <summary>
        /// 表示单元格的公式内容
        /// </summary>
        public string Formula { get; set; }


        /// <summary>
        /// 用于二维表更新不同值时 高亮显示
        /// </summary>
        public bool IsShowColorForUpdateData { get; set; }

        public bool NeedMearge
        {
            get
            {
                return TotalRow > 1 || TotalColumn > 1;
            }
        }
    }
}
