
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;

namespace Lib.DAL
{



    /// <summary>
    /// Enum对象的数据访问适配器
    /// </summary>
    public class EnumAdapter : AppBaseAdapterT<Model.Enum>
    {
        public IList<Model.Enum> GetList()
        {
            string sql = ORMapping.GetSelectSql<Model.Enum>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }


        public IList<Model.Enum> GetList(string type)
        {
            string sql = ORMapping.GetSelectSql<Model.Enum>(TSqlBuilder.Instance);

            sql += "WHERE EnumType='" + SafeQuote(type) + "' and " + base.NotDeleted + " ORDER BY DisplayOrder ";

            return ExecuteQuery(sql);
        }
    }
}

