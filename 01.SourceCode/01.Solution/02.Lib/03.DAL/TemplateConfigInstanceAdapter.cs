
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;

namespace Lib.DAL
{



    /// <summary>
    /// TemplateConfigInstance对象的数据访问适配器
    /// </summary>
    public class TemplateConfigInstanceAdapter : AppBaseAdapterT<TemplateConfigInstance>
    {
        public IList<TemplateConfigInstance> GetList()
        {
            string sql = ORMapping.GetSelectSql<TemplateConfigInstance>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public List<TemplateConfigInstance> GetList(string loginName, string name, string time, string endTime)
        {
            string SQL = ORMapping.GetSelectSql<TemplateConfigInstance>(TSqlBuilder.Instance);
            SQL += string.Format("Where {0} ", NotDeleted);
            if (!string.IsNullOrEmpty(loginName))
            {
                SQL += string.Format(" and UserName = '{0}'", SafeQuote(loginName));
            }
            if (!string.IsNullOrEmpty(name))
            {
                SQL += string.Format(" and TemplateConfigInstanceName like '%{0}%'", SafeQuote(name));
            }
            DateTime beginTime = DateTime.Parse("1990-01-01");

            DateTime endtime = DateTime.Parse("2990-01-01");

            if (!string.IsNullOrEmpty(time))
            {
                beginTime = DateTime.Parse(time);
            }
            if (!string.IsNullOrEmpty(endTime))
            {
                endtime = DateTime.Parse(endTime);
            }

            if (!string.IsNullOrEmpty(time))
            {
                SQL += string.Format(" and CreatorTime between '{0} 00:00:00' and '{1} 23:59:59'", beginTime.ToString("yyyy-MM-dd"), endtime.ToString("yyyy-MM-dd"));
            }

            return ExecuteQuery(SQL);
        }

        public bool IsStartToUse(string userID)
        {
            string sql = @"SELECT 
 (CASE  (SELECT COUNT(1)  FROM dbo.Template WHERE CreatorLoginName=@UserID)
  WHEN 0  THEN 0 ELSE 1 END
  )
+
 (CASE  (SELECT COUNT(1)  FROM dbo.TemplateConfigInstance WHERE CreatorLoginName=@UserID)
  WHEN 0  THEN 0 ELSE 1 END
  )";
            return Convert.ToInt32(DbHelper.RunSqlReturnScalar(sql, this.ConnectionName, CreateSqlParameter("@UserID", System.Data.DbType.String, userID))) > 0;
        }

        public void ScheduleTask(int topCount)
        {
            string spName = "P_ScheduleTask";
            DbHelper.RunSPReturnScalar(spName, ConnectionName);
        }
        public void DoneTask()
        {
            string spName = "P_DoneTask";
            DbHelper.RunSPReturnScalar(spName, ConnectionName);
        }

        public List<TemplateConfigInstance> LoadClearTaskList(int topCount)
        {
            string sql = string.Format(@"
SELECT TOP {0} TCI.* FROM dbo.TemplateConfigInstance TCI

LEFT JOIN v_Employee U

ON TCI.UserName = U.username
 WHERE
 TCI.IsDeleted = 0
 --周期任务 及子任务
 AND TCI.TaskType IN (2,3)
 --正在运行的任务
 AND (
 CASE 
 --周期任务没有发起状态 
 WHEN TaskType=2 AND  ProcessStatus=0 THEN 1
 --子任务有发起状态
 WHEN TaskType=3 AND ProcessStatus=1 THEN 1
 ELSE 0 END 
 )=1
 --不在在职状态中
 AND U.employeeStatus NOT IN(2, 7, 9, 11)
", topCount);
            return ExecuteQuery(sql);
        }

        public List<TemplateConfigInstance> LoadPendingDoneTaskNotifyList(int topCount)
        {
            string sql = string.Format(@"SELECT top {0} * FROM dbo.TemplateConfigInstance
WHERE ProcessStatus = 3 AND ISNULL(NotifyStatus,0)= 0 AND IsDeleted = 0", topCount);
            return ExecuteQuery(sql);
        }
    }
}

