using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.ViewModel
{
    public class TaskView
    {
        public string ID { get; set; }
        public string TemplateID { get; set; }

        public string TemplateName { get; set; }

        public string TemplateConfigInstanceName { get; set; }
        public string WorkflowID { get; set; }
        public string WorkflowInfo { get; set; }
        public string Remark { get; set; }
        public List<TaskUser> Users { get; set; }


        public int TaskType { get; set; }
        public int CircleType { get; set; }
        public List<DateTime> SelectDates { get; set; }
        public DateTime? PlanBeginDate { get; set; }
        public DateTime? PlanEndDate { get; set; }

        public int PlanHour { get; set; }
        public int PlanMinute { get; set; }

    }
    public class TaskUser
    {
        public string EmployeeCode { get; set; }

        public string UserName { get; set; }

        public string EmployeeName { get; set; }

        public int OrgID { get; set; }

        public string OrgName { get; set; }

        public int UnitID { get; set; }

        public string UnitName { get; set; }

        #region 当TaskTemplateType=2时，用于更新二维表

        public int TaskTemplateType { get; set; }

        public string UpdateArea { get; set; }

        public string AreaValue { get; set; }

        #endregion
    }

    public class UserView
    {
        public string UserLoginID { get; set; }

        public string UserName { get; set; }

        public string UserOrgPathName { get; set; }

        public string UserJobName { get; set; }
    }
}
