



using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using System.Linq;

namespace Plugin.UserSelect
{
    /// <summary>
    /// AutoProcess对象的数据访问适配器
    /// </summary>
    public class VEmployeeAdapter : AppBaseCompositionAdapterT<VEmployeeEntity>
    {
        public List<VEmployeeEntity> LoadUserList(int deptID)
        {
            string sql = @"SELECT *
FROM V_Employee
WHERE  EmployeeStatus = 2
      AND unitID = @DeptId
ORDER BY orderLevel ASC; ";
            return ExecuteQuery(sql
                , CreateSqlParameter("@DeptId", System.Data.DbType.Int32, deptID)
                );
        }

        internal List<VEmployeeEntity> LoadUserList(string employeeName, string dept, string job, string userName, string keyWord)
        {
            string sql = @"
SELECT TOP 200 *
FROM V_Employee
WHERE EmployeeStatus = 2
      AND 1 = (CASE
                   WHEN LEN(@employeeName) = 0
                        OR @employeeName IS NULL THEN
                       1
                   WHEN CHARINDEX(@employeeName, EmployeeName) > 0 THEN
                       1
                   ELSE
                       0
               END
              )
      AND 1 = (CASE
                   WHEN LEN(@dept) = 0
                        OR @dept IS NULL THEN
                       1
                   WHEN CHARINDEX(@dept, unitName) > 0 THEN
                       1
                   ELSE
                       0
               END
              )
      AND 1 = (CASE
                   WHEN LEN(@job) = 0
                        OR @job IS NULL THEN
                       1
                   WHEN CHARINDEX(@job, jobName) > 0 THEN
                       1
                   ELSE
                       0
               END
              )
      AND 1 = (CASE
                   WHEN LEN(@userName) = 0
                        OR @userName IS NULL THEN
                       1
                   WHEN CHARINDEX(@userName, username) > 0 THEN
                       1
                   ELSE
                       0
               END
              )
      AND 1 = (CASE
                   WHEN LEN(@keyword) = 0
                        OR @keyword IS NULL THEN
                       1
                   WHEN
                   (
                       CHARINDEX(@keyword, Tel) > 0
                       OR CHARINDEX(@keyword, Mobile) > 0
                       OR CHARINDEX(@keyword, Email) > 0
                   ) THEN
                       1
                   ELSE
                       0
               END
              )
ORDER BY OrderLevel ASC; ";
            return ExecuteQuery(sql
                , CreateSqlParameter("@employeeName", System.Data.DbType.String, employeeName)
                , CreateSqlParameter("@dept", System.Data.DbType.String, dept)
                , CreateSqlParameter("@job", System.Data.DbType.String, job)
                , CreateSqlParameter("@userName", System.Data.DbType.String, userName)
                , CreateSqlParameter("@keyword", System.Data.DbType.String, keyWord)
                );
        }

        internal List<VEmployeeEntity> LoadUserList(string key)
        {
            string sql = @"

SELECT TOP 200 *
FROM V_Employee
WHERE EmployeeStatus = 2
      AND (1 = (CASE
                   WHEN LEN(@key) = 0
                        OR @key IS NULL THEN
                       1
                   WHEN CHARINDEX(@key, EmployeeName) > 0 THEN
                       1
                   ELSE
                       0
               END
              )
   
      OR 1 = (CASE
                   WHEN LEN(@key) = 0
                        OR @key IS NULL THEN
                       1
                   WHEN CHARINDEX(@key, username) > 0 THEN
                       1
                   ELSE
                       0
               END
              ))
    
ORDER BY OrderLevel ASC;";
            return ExecuteQuery(sql
                , CreateSqlParameter("@key", System.Data.DbType.String, key)
                );
        }

        internal VEmployeeEntity GetEmployeeByID(int userId)
        {
            string sql = "SELECT * FROM dbo.V_Employee WHERE employeeCode=@EmployeeCode";
            return ExecuteQuery(sql
                , CreateSqlParameter("@EmployeeCode", System.Data.DbType.Int32, userId)
                ).FirstOrDefault();
        }
    }
}

