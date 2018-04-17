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
  public  class VTaskUserOperator
    {
        #region Generate Code
        public static readonly VTaskUserOperator Instance = new VTaskUserOperator();

        private static VTaskUserAdapter _vtaskUserAdapter = AdapterFactory.GetAdapter<VTaskUserAdapter>();

        #endregion

        public PartlyCollection<VTaskUser> GetViewList(VTaskUserFilter filter)
        {
            return _vtaskUserAdapter.GetList(filter);
        }
    }
}
