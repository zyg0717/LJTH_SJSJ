using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;

namespace Lib.DAL
{
    /// <summary>
    /// Interface对象的数据访问适配器
    /// </summary>
    public class TSM_MessagesAdapter : AppBaseAdapterT<TSM_Messages>
    {


        public IList<TSM_Messages> GetMessagesList()
        {
            string sql = ORMapping.GetSelectSql<TSM_Messages>(TSqlBuilder.Instance);
            return ExecuteQuery(sql);
        }


        public override int Delete(TSM_Messages data)
        {
            string sql = "DELETE FROM TSM_Messages WHERE ID=@ID";
            SqlParameter pa = CreateSqlParameter("@ID", System.Data.DbType.String, data.ID);
            return ExecuteSql(sql, new SqlParameter[] { pa });
        }

        public List<TSM_Messages> LoadPendingList(int topCount, int maxTryCount)
        {
            string sql = string.Format("SELECT TOP {0} * FROM dbo.TSM_Messages WHERE (Status=0 OR (TryTimes<{1} AND Status=-1)) ORDER BY Priority ASC ,TargetTime DESC ", topCount, maxTryCount);
            return ExecuteQuery(sql);
        }
    }
}

