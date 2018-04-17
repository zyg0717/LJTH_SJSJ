using Framework.Core.Cache;
using Framework.Core;
using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Framework.Data.AppBase
{
    public class AppBaseCompositionAdapterT<T> : BaseCompositionAdapterT<T>
         where T : IBaseComposedModel, new()
    {
        protected override string ConnectionName
        {
            get { return "DBConnectionString"; }
        }
    }

    public class AppCommonAdapter : CommonAdapter
    {
        protected override string ConnectionName
        {
            get { return ""; }
        }
    }

    public class AppBaseAdapterT<T> : BaseAdapterT<T>
         where T : BaseModel, new()
    {
        protected override string ConnectionName
        {
            get { return "DBConnectionString"; }
        }

    }


}
