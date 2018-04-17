using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Common;
using Lib.ViewModel;

namespace Lib.DAL
{
    public class CollectUserTaskAdapter : AppBaseCompositionAdapterT<CollectUserTaskView>
    {
        public List<CollectUserTaskView> GetList(string taskID, string templateID)
        {
            string sql = ORMapping.GetSelectSql<CollectUserTaskView>(TSqlBuilder.Instance);
            sql += string.Format("WHERE 1=1");
            if (!string.IsNullOrEmpty(taskID))
            {
                sql += string.Format(" and ID = '{0}'", SafeQuote(taskID));
            }
            if (!string.IsNullOrEmpty(templateID))
            {
                sql += string.Format(" and TemplateID='{0}'", templateID);
            }

            return ExecuteQuery(sql);
        }
        public List<CollectUserTaskView> GetProcessList()
        {
            string sql = ORMapping.GetSelectSql<CollectUserTaskView>(TSqlBuilder.Instance);
            return ExecuteQuery(sql);
        }
        public CollectUserTaskView LoadByID(string id)
        {
            string sql = ORMapping.GetSelectSql<CollectUserTaskView>(TSqlBuilder.Instance);
            sql += " WHERE ID=@ID";
            return ExecuteQuery(sql
                ,
                CreateSqlParameter("@ID", System.Data.DbType.String, id)
                ).FirstOrDefault();
        }

        public List<CollectUserTaskView> GetMyTaskList(string LoginName)
        {
            string sql = ORMapping.GetSelectSql<CollectUserTaskView>(TSqlBuilder.Instance);
            sql += string.Format("where CreatorLoginName = '{0}'", LoginName);

            return ExecuteQuery(sql);
        }
        public List<CollectUserTaskView> GetListT(string takName, string tempDate, string creatorName, string tempName)
        {
            string SQL = ORMapping.GetSelectSql<CollectUserTaskView>(TSqlBuilder.Instance);
            SQL += string.Format("Where 1=1");
            if (!string.IsNullOrEmpty(takName))
            {
                SQL += string.Format(" and TemplateConfigInstanceName like '%{0}%'", SafeQuote(takName));
            }

            if (!string.IsNullOrEmpty(tempDate))
            {
                SQL += string.Format(" and SendTime between '{0} 00:00:00' and '{0} 23:59:59'", DateTime.Parse(tempDate).ToString("yyyy-MM-dd"));
            }

            if (!string.IsNullOrEmpty(creatorName))
            {
                SQL += string.Format(" and (CreatorLoginName like '%{0}%' or CreatorName like '%{0}%') ", creatorName);
            }


            if (!string.IsNullOrEmpty(tempName))
            {
                SQL += string.Format(" and TemplateName like '%{0}%'", tempName);
            }

            return ExecuteQuery(SQL);
        }
        public List<CollectUserTaskView> GetMoreMyTaskList(string LoginName, string name, string time)
        {
            string SQL = ORMapping.GetSelectSql<CollectUserTaskView>(TSqlBuilder.Instance);
            SQL += string.Format("WHERE 1=1");
            if (!string.IsNullOrEmpty(LoginName))
            {
                SQL += string.Format(" and CreatorLoginName like '%{0}%'", SafeQuote(LoginName));
            }
            if (!string.IsNullOrEmpty(name))
            {
                SQL += string.Format(" and TemplateConfigInstanceName like '%{0}%'", SafeQuote(name));
            }
            if (!string.IsNullOrEmpty(time))
            {
                SQL += string.Format(" and CreatorTime  >= '{0} 00:00:00' ", DateTime.Parse(time).ToString("yyyy-MM-dd"));
            }



            //if (!string.IsNullOrEmpty(time))
            //{
            //    beginTime = DateTime.Parse(time);
            //}
            //if (!string.IsNullOrEmpty(timet))
            //{
            //    endtime = DateTime.Parse(timet);
            //}


            return ExecuteQuery(SQL);
        }
        public List<CollectUserTaskView> SearchUserTask(string taskID, string UnitName, string EmployeeName, int FeedBack)
        {
            string SQL = ORMapping.GetSelectSql<CollectUserTaskView>(TSqlBuilder.Instance);
            SQL += string.Format("Where 1=1");
            if (!string.IsNullOrEmpty(UnitName))
            {
                SQL += string.Format(" and UnitName like '%{0}%'", SafeQuote(UnitName));
            }

            if (!string.IsNullOrEmpty(EmployeeName))
            {
                SQL += string.Format(" and EmployeeName like '%{0}%'", SafeQuote(EmployeeName));
            }
            if (!string.IsNullOrEmpty(taskID))
            {
                SQL += string.Format(" and ID like '{0}'", SafeQuote(taskID));
            }

            if (FeedBack > -1)
            {
                SQL += string.Format(" and Status {1} {0} ", (int)ProcessStatus.Draft, FeedBack == 1 ? "<>" : "=");
            }
            return ExecuteQuery(SQL);
        }

    }
}
