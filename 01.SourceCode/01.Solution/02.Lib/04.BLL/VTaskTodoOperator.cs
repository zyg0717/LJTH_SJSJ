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
  public  class VTaskTodoOperator
    {
        #region Generate Code
        public static readonly VTaskTodoOperator Instance = new VTaskTodoOperator();

        private static VTaskTodoAdapter _vtaskUserAdapter = AdapterFactory.GetAdapter<VTaskTodoAdapter>();

        #endregion

        public PartlyCollection<VTaskTodo> GetViewList(VTaskTodoFilter filter)
        {
            return _vtaskUserAdapter.GetList(filter);
        }
    }
}
