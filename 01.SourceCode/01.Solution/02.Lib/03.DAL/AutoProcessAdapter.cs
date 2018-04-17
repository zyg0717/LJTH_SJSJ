



using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;


namespace Lib.DAL
{
    /// <summary>
    /// AutoProcess对象的数据访问适配器
    /// </summary>
   public class AutoProcessAdapter:AppBaseAdapterT<AutoProcess>
	{

       public IList<AutoProcess> GetAutoProcessList()
       {
           string sql = ORMapping.GetSelectSql<AutoProcess>(TSqlBuilder.Instance);

           sql += "WHERE " + base.NotDeleted;

           return ExecuteQuery(sql);
       }

        

            public IList<AutoProcess> GetWaitingStartWorkflowList( int ErrorCount = 5)
        {
            string sql = string.Format("SELECT TOP 30 * FROM {0} WHERE {1}", ORMapping.GetTableName<AutoProcess>(), base.NotDeleted);
            sql += " AND (BusinessType='StartProcess' OR BusinessType='CancelProcess') AND (Status=0 OR (Status=-1 AND ErrorCount<@ErrorCount)) ORDER BY CreatorTime";

            return ExecuteQuery(sql, CreateSqlParameter("@ErrorCount", System.Data.DbType.Int64, ErrorCount));
        }
        public IList<AutoProcess> GetAutoprocesWaittingList(string BusinessType, int ErrorCount=5)
       {
           string sql = string.Format("SELECT TOP 30 * FROM {0} WHERE {1}", ORMapping.GetTableName<AutoProcess>(), base.NotDeleted);
           sql += " AND BusinessType=@BusinessType AND (Status=0 OR (Status=-1 AND ErrorCount<@ErrorCount)) ORDER BY CreatorTime";

           return ExecuteQuery(sql, CreateSqlParameter("@BusinessType", System.Data.DbType.String, BusinessType), CreateSqlParameter("@ErrorCount", System.Data.DbType.Int64, ErrorCount));
       }
         
		
		 
	} 
}

