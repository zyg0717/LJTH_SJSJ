using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateConfigInstance.
    /// </summary>
    [ORTableMapping("dbo.TemplateConfigInstance")]
    public class TemplateConfigInstance : BaseModel
    {
        [NoMapping]
        public DateTime? SubmitDate { get; set; }
        public bool NotifyStatus { get; set; }

        [NoMapping]
        public List<TemplateConfigInstancePlan> SubTasks { get; set; }

        [NoMapping]
        public List<DateTime> SelectDates { get; set; }

        #region Public Properties
        [ORFieldMapping("PlanHour")]
        public int PlanHour { get; set; }

        [ORFieldMapping("PlanMinute")]
        public int PlanMinute { get; set; }

        /// <summary>
        /// 1����һ�������� 2�������� 3������
        /// </summary>
        [ORFieldMapping("TaskType")]
        public int TaskType { get; set; }
        /// <summary>
        /// ��������  1���������� 2ÿ�� 3�Զ���
        /// </summary>
        [ORFieldMapping("CircleType")]
        public int CircleType { get; set; }
        /// <summary>
        /// �ƻ�����ʼʱ��
        /// </summary>
        [ORFieldMapping("PlanBeginDate", true)]
        public DateTime? PlanBeginDate { get; set; }
        [NoMapping]
        public string PlanBeginDateString
        {
            get
            {
                if (PlanBeginDate.HasValue)
                {
                    return PlanBeginDate.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        [NoMapping]
        public string PlanEndDateString
        {
            get
            {
                if (PlanEndDate.HasValue)
                {
                    return PlanEndDate.Value.ToString("yyyy-MM-dd");
                }
                return "";
            }
        }
        /// <summary>
        /// �ƻ��������ʱ��
        /// </summary>
        [ORFieldMapping("PlanEndDate", true)]
        public DateTime? PlanEndDate { get; set; }
        /// <summary>
        /// 0 �Ѵ��� 1 �ѷ��� 2 ����ֹ 3 ����� 4 �ѹ鵵
        /// </summary>
        [ORFieldMapping("ProcessStatus")]
        public int ProcessStatus { get; set; }

        [ORFieldMapping("TemplateID")]
        public string TemplateID { get; set; }



        [ORFieldMapping("TemplateConfigInstanceName")]
        public string TemplateConfigInstanceName { get; set; }



        [ORFieldMapping("WorkflowID")]
        public string WorkflowID { get; set; }



        [ORFieldMapping("WorkflowInfo")]
        public string WorkflowInfo { get; set; }



        [ORFieldMapping("UserName")]
        public string UserName { get; set; }



        [ORFieldMapping("EmployeeName")]
        public string EmployeeName { get; set; }



        [ORFieldMapping("Remark")]
        public string Remark { get; set; }



        [ORFieldMapping("TemplateName")]
        public string TemplateName { get; set; }

        /// <summary>
        /// ��ʽFileCode+"|"+��չ����.xls��������
        /// </summary>
        [ORFieldMapping("TemplatePath")]
        public string TemplatePath { get; set; }


        /// <summary>
        /// �ļ���ַ
        /// </summary>
        [NoMapping]
        public string TemplatePathFileCode
        {
            get
            {
                if (TemplatePath != null)
                {
                    return TemplatePath.Split('|')[0];
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// �ļ���չ��
        /// </summary>
        [NoMapping]
        public string TemplatePathFileExt
        {
            get
            {
                if (TemplatePath != null)
                {
                    var strs = TemplatePath.Split('|');
                    if (strs.Length == 2)
                    {
                        return strs[1];
                    }
                }
                return string.Empty;
            }
        }

        [NoMapping]
        public string CreatorTimeString
        {
            get
            {
                return CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion

        [NoMapping]
        public int TotalTask { get; set; }


        [NoMapping]
        public int CompleteTask { get; set; }
        [NoMapping]
        public int PageCount { get; set; }
        [NoMapping]
        public List<Attachment> Attachments { get; set; }

        #region ���ڸ��¶�ά��

        /// <summary>
        /// 1����ʾԭʼ����2����ʾ��������
        /// </summary>
        [ORFieldMapping("TaskTemplateType")]
        public int TaskTemplateType { get; set; }

        #endregion

    }
}

