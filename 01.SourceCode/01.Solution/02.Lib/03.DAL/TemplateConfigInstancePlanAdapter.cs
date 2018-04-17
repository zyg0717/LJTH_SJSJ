
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;
using Lib.Model.Filter;

namespace Lib.DAL
{



    /// <summary>
    /// TemplateConfigInstance对象的数据访问适配器
    /// </summary>
    public class TemplateConfigInstancePlanAdapter : AppBaseAdapterT<TemplateConfigInstancePlan>
    {
        public IList<TemplateConfigInstancePlan> GetList(TemplateConfigInstancePlanFilter filter, out int totalCount)
        {
            var whereBuilder = filter.ConvertToWhereBuilder();
            whereBuilder.AppendItem("IsDeleted", false);
            QueryCondition qc = new QueryCondition(
                filter.RowIndex,
                filter.PageSize,
                " * ",
               ORMapping.GetTableName(typeof(TemplateConfigInstancePlan)),
               SqlTextHelper.SafeQuote("CreatorTime asc"),
               whereBuilder.ToSqlString(TSqlBuilder.Instance)
              );
            var result = GetPageSplitedCollection(qc);
            totalCount = result.TotalCount;
            return result.SubCollection;
        }

        public IList<TemplateConfigInstancePlan> GetPendingList(int topCount)
        {
            string sql =
                string.Format(@"SELECT TOP {0}
 TCIP.* FROM dbo.TemplateConfigInstancePlan TCIP
 INNER JOIN dbo.TemplateConfigInstance TCI
 ON TCIP.TemplateConfigInstanceID = TCI.ID


  WHERE
  TCIP.IsDeleted = 0 AND TCIP.Status = 1 AND DATEADD(DAY,-1, TCIP.SenderTime)<= GETDATE()
  AND
  TCI.IsDeleted = 0 AND TCI.ProcessStatus = 0
   ORDER BY TCIP.CreatorTime ASC ", topCount);
            return ExecuteQuery(sql);
        }

        public IList<TemplateConfigInstancePlan> GetPendingSubmitList(int topCount)
        {
            string sql =
                $"SELECT TOP {topCount} * FROM dbo.TemplateConfigInstancePlan WHERE IsDeleted=0 AND Status=2 AND SenderTime<=GETDATE()  ORDER BY CreatorTime ASC ";
            return ExecuteQuery(sql);
        }
    }
}

