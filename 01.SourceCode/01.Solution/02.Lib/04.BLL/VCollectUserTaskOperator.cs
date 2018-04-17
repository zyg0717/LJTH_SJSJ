using Framework.Data;
using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.DAL;
using Lib.Model.Filter;
using Lib.ViewModel;

namespace Lib.BLL
{
   public class VCollectUserTaskOperator
    {
        #region Generate Code
        public static readonly VCollectUserTaskOperator Instance = new VCollectUserTaskOperator();

        private static VCollectUserTaskAdapter _vCollectUserTaskAdapter = AdapterFactory.GetAdapter<VCollectUserTaskAdapter>();

        #endregion

        public PartlyCollection<VCollectUserTask> GetViewList(VCollectUserTaskFilter filter)
        {
            return _vCollectUserTaskAdapter.GetList(filter);
        }
    }
}
