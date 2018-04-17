
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
    public class VTaskTodoAdapter : AppBaseCompositionAdapterT<VTaskTodo>
    {

        public PartlyCollection<VTaskTodo> GetList(VTaskTodoFilter filter)
        {
            var where = filter.ConvertToWhereBuilder();
            var tuple = GetPageSplitedCollection(filter.RowIndex, filter.PageSize, where.ToSqlString(TSqlBuilder.Instance));
            return tuple;
        }

    }
}

