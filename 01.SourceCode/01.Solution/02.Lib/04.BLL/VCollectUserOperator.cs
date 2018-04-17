using Framework.Core;
using Framework.Data;
using Framework.Data.AppBase;
using Framework.Web.Security.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Lib.DAL;
using Lib.Model;
using Lib.Model.Filter;
using Lib.ViewModel;

namespace Lib.BLL
{
   public class VCollectUserOperator
    {
        #region Generate Code
        public static readonly VCollectUserOperator Instance = new VCollectUserOperator();

        private static VCollectUserAdapter _vCollectUserTaskAdapter = AdapterFactory.GetAdapter<VCollectUserAdapter>();

        #endregion

        public PartlyCollection<VCollectUser> GetViewList(VCollectUserFilter filter)
        {
            return _vCollectUserTaskAdapter.GetList(filter);
        }
    }
}
