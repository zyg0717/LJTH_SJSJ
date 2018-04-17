using Framework.Data;
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
    public class TaskUserViewOperator
    {
        #region Generate Code
        public static readonly TaskUserViewOperator Instance = new TaskUserViewOperator();

        private static TaskUserViewAdapter _taskUserViewAdapter = AdapterFactory.GetAdapter<TaskUserViewAdapter>();

        #endregion

        public List<TaskUserView> GetList()
        {
            List<TaskUserView> result = _taskUserViewAdapter.GetList();
            return result;
        }
        public List<TaskUserView> GetAgencyList(List<string> businessids)
        {
            List<TaskUserView> result = _taskUserViewAdapter.GetAgencyList(businessids);
            return result;
        }
        public List<TaskUserView> SearchAgencyList(List<string> businessids)
        {
            List<TaskUserView> result = _taskUserViewAdapter.SearchAgencyList(businessids);
            return result;
        }
    }
}
