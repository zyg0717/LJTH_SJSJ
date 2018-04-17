using Framework.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.Model.Filter;
using System.Text;

namespace Lib.DAL
{
    public class EmployeeAdapter : AppBaseAdapterT<Employee>
    {
        public static EmployeeAdapter Instance = new EmployeeAdapter();

        public List<Employee> FindUser(string userName)
        {
            string sql = @"
SELECT employeeName,username,unitName,jobName,gender,mobile,employeeStatus,employeeCode 
    FROM v_Employee where username='" + SafeQuote(userName) + "' or employeeName='" + SafeQuote(userName) + "'";
            DataTable dt = ExecuteReturnTable(sql);

            List<Employee> users = new List<Employee>();
            if (dt != null)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Employee wuser = new Employee();
                    ORMapping.DataRowToObject<Employee>(item, wuser);

                    users.Add(wuser);
                }
            }

            return users;
        }

        public List<Employee> GetEmployeeList(UserFilter filter, out int itemCount)
        {
            WhereSqlClauseBuilder where = filter.ConvertToWhereBuilder();
            where.AppendItem("employeeStatus", 3, "<>");

            QueryCondition qc = new QueryCondition(
                  filter.RowIndex,
                  filter.PageSize,
                  " * ",
                  ORMapping.GetTableName<Employee>(),
                  " employeecode ",
                 where.ToSqlString(TSqlBuilder.Instance)
                );


            PartlyCollection<Employee> tuple = GetPageSplitedCollection(qc);

            itemCount = tuple.TotalCount;
            return tuple.SubCollection;
        }

        public Employee GetUserByCode(string id)
        {
            string sql = @"
                        SELECT employeeName,username,unitName,jobName,employeeStatus,employeeCode 
                            FROM dbo.V_employee where employeeCode='" + SafeQuote(id) + "'";

            return ExecuteQuery(sql).ToList().FirstOrDefault();
        }

        public List<Employee> GetBatchModelObjects(List<string> userIDs)
        {
            WhereSqlClauseBuilder where = new WhereSqlClauseBuilder();

            //string idField = "ID"; //TODO , 通过ORM 反射找到对应的Field; 默认暂时认为都是使用ID字段

            //ORMapping.GetMappingInfo<T>()[]

            string idsJoined = string.Format(" employeecode in  ('{0}') ", string.Join("','", userIDs.Select(p => SafeQuote(p))));



            string sqlString = ORMapping.GetSelectSql<Employee>(TSqlBuilder.Instance)
                + " where "
                + idsJoined
                + " AND " + NotDeleted;

            var listResult = this.ExecuteQuery(sqlString, null);


            return listResult;
        }

        public List<Employee> GetUserList(UserFilter filter, out int itemCount)
        {
            WhereSqlClauseBuilder where = filter.ConvertToWhereBuilder();
            where.AppendItem("employeeStatus", 3, "<>");

            QueryCondition qc = new QueryCondition(
                  filter.RowIndex,
                  filter.PageSize,
                  " * ",
                  ORMapping.GetTableName<Employee>(),
                  " orgName asc,username asc ",
                 where.ToSqlString(TSqlBuilder.Instance)
                );


            PartlyCollection<Employee> tuple = GetPageSplitedCollection(qc);

            itemCount = tuple.TotalCount;
            return tuple.SubCollection;
        }

    }
}
