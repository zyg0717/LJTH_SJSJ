using Framework.Data.AppBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.DAL;
using Lib.ViewModel;

namespace Lib.BLL
{
   public  class TempOrgOperator
    {
        #region Generate Code
       public static readonly TempOrgOperator Instance = new TempOrgOperator();

        private static TempOrgAdapter _collectUserTaskAdapter = AdapterFactory.GetAdapter<TempOrgAdapter>();

        #endregion

        public List<TempTaskOrgView> GetList()
        {
            return _collectUserTaskAdapter.GetList();
        }
    }
}
