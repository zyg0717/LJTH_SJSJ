
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;
using System.Data.SqlClient;
using System.Data;
using Lib.DAL;
using Lib.Common;
using Lib.ViewModel;
using Lib.Model.Filter;

namespace Lib.DAL
{



    /// <summary>
    /// Template对象的数据访问适配器
    /// </summary>
    public class VTemplateAdapter : AppBaseCompositionAdapterT<VTemplate>
    {

        public PartlyCollection<VTemplate> GetList(VTemplateFilter filter)
        {


            var where = filter.ConvertToWhereBuilder();
            where.AppendItem("Status", 0);
            //string sql = ORMapping.GetSelectSql<VTemplate>(TSqlBuilder.Instance);

            //sql += " WHERE  1=1";
            //if (!where.IsEmpty)
            //{
            //    sql += " AND " + where.ToSqlString(TSqlBuilder.Instance);
            //}
            //var qc = new QueryCondition(
            //   filter.RowIndex,
            //   filter.PageSize,
            //   "*",
            //   sql,
            //   " CreatorTime desc",
            //   where.ToSqlString(TSqlBuilder.Instance));
            var tuple = GetPageSplitedCollection(filter.RowIndex, filter.PageSize, where.ToSqlString(TSqlBuilder.Instance));
            return tuple;
        }

    }
}

