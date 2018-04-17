using Framework.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Framework.Data.AppBase
{
    /// <summary>
    /// 通用的数据查询 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CommonAdapter : DataAdapterBase
    {
        protected override string ConnectionName
        {
            get { return "DBConnectionString"; }
        }

    }
}
