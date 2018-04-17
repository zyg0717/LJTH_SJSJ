
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;
using Lib.Common;
using Lib.Model.Filter;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;

namespace Lib.DAL
{



    /// <summary>
    /// TemplateTask对象的数据访问适配器
    /// </summary>
    public class TemplateTaskAdapter : AppBaseAdapterT<TemplateTask>
    {

        public IList<TemplateTask> GetList()
        {
            string sql = ORMapping.GetSelectSql<TemplateTask>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public TemplateTask GetModel(string userName, string templateConfigInstanceID)
        {
            return base.Load(p => { p.AppendItem("EmployeeLoginName", userName); p.AppendItem("templateConfigInstanceID", templateConfigInstanceID); p.AppendItem("isdeleted", 0); }).FirstOrDefault();
        }

        public List<TemplateTask> GetModel(string configInstanceID)
        {
            return base.Load(p =>
            {
                p.AppendItem("templateConfigInstanceID", configInstanceID);
                p.AppendItem("isdeleted", 0);
                p.AppendItem("ProcessStatus", 0);
            });
        }

        public TemplateTask GetTaskModel(string configInstanceID, string dataCollectUserID)
        {
            return base.Load(p => { p.AppendItem("templateConfigInstanceID", configInstanceID); p.AppendItem("dataCollectUserID", dataCollectUserID); p.AppendItem("isdeleted", 0); p.AppendItem("ProcessStatus", 0); }).FirstOrDefault();
        }
        public TemplateTask TryGetTaskModel(string configInstanceID, string dataCollectUserID)
        {
            return base.Load(p => { p.AppendItem("templateConfigInstanceID", configInstanceID); p.AppendItem("dataCollectUserID", dataCollectUserID); }).FirstOrDefault();
        }

        public List<TemplateTask> SearchUserTask(string taskID, string unitName, string employeeName, int feedBack)
        {
            string SQL = ORMapping.GetSelectSql<TemplateTask>(TSqlBuilder.Instance);
            SQL += string.Format("Where IsDeleted=0");
            if (!string.IsNullOrEmpty(unitName))
            {
                SQL += string.Format(" and OrgName like '%{0}%'", SafeQuote(unitName));
            }

            if (!string.IsNullOrEmpty(employeeName))
            {
                SQL += string.Format(" and EmployeeName like '%{0}%'", SafeQuote(employeeName));
            }
            if (!string.IsNullOrEmpty(taskID))
            {
                SQL += string.Format(" and TemplateConfigInstanceID like '{0}'", SafeQuote(taskID));
            }

            if (feedBack > -1)
            {
                SQL += string.Format(" and Status {1} {0} ", (int)ProcessStatus.Draft, feedBack == 1 ? "<>" : "=");
            }
            SQL += " and ProcessStatus=0";
            return ExecuteQuery(SQL);
        }

        public List<TemplateTask> GetDataUserIDByID(string TempConfigInstanceID)
        {
            string SQL = string.Format("select  DataCollectUserID from TemplateTask where TemplateConfigInstanceID ='{0}' and IsDeleted=0 and ProcessStatus=0", TempConfigInstanceID);
            return ExecuteQuery(SQL);
        }
        public TemplateTask GetByConfigInstaceID(string configInstanceID)
        {
            return base.Load(p => { p.AppendItem("templateConfigInstanceID", configInstanceID); p.AppendItem("isdeleted", 0); }).FirstOrDefault();

        }

        public void RemoveByDataCollectUserID(string dataCollectUserID)
        {
            string sql = @"UPDATE TemplateTask SET ISDELETED=1,MODIFYTIME=GETDATE(),MODIFIERNAME=@CurrentUserName WHERE ISDELETED=0 AND DataCollectUserID=@DataCollectUserID";
            ExecuteSql(sql
               , CreateSqlParameter("@CurrentUserName", System.Data.DbType.String, WebHelper.GetCurrentUser().LoginName)
               , CreateSqlParameter("@DataCollectUserID", System.Data.DbType.String, dataCollectUserID)

               );
        }

        public TemplateTask GetLastTask(string collectDataUserID)
        {
            string sql = "select top 1 * from TemplateTask where isdeleted=0 and ProcessStatus=0 and DataCollectUserID=@DataCollectUserID";
            return ExecuteQuery(sql, CreateSqlParameter("@DataCollectUserID", System.Data.DbType.String, collectDataUserID)).FirstOrDefault();
        }
    }
}

