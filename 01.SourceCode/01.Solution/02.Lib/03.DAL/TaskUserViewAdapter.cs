using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.ViewModel;

namespace Lib.DAL
{
    public class TaskUserViewAdapter : AppBaseCompositionAdapterT<TaskUserView>
    {
        public List<TaskUserView> GetList()
        {
            string sql = ORMapping.GetSelectSql<TaskUserView>(TSqlBuilder.Instance);
            return ExecuteQuery(sql);
        }
        public List<TaskUserView> GetAgencyList(List<string> businessids)
        {
            string sql = ORMapping.GetSelectSql<TaskUserView>(TSqlBuilder.Instance);
            sql += string.Format("WHERE ID IN ('{0}')", businessids.Aggregate((p, q) => p + "','" + q));

            return ExecuteQuery(sql);
        }
        public List<TaskUserView> SearchAgencyList(List<string> businessids)
        {
            string sql = ORMapping.GetSelectSql<TaskUserView>(TSqlBuilder.Instance);
            sql += string.Format("WHERE ID IN ('{0}') ", businessids.Aggregate((p, q) => p + "','" + q));

            //if (!string.IsNullOrEmpty(Title))
            //{
            //    sql += string.Format(" and TemplateConfigInstanceName like '%{0}%'", Title);
            //}
            //if (!string.IsNullOrEmpty(Time))
            //{
            //    sql += string.Format("and  CreatorTime >= '{0} 00:00:00'", DateTime.Parse(Time).ToString("yyyy-MM-dd"));
            //}

            return ExecuteQuery(sql);
        }
    }
}
