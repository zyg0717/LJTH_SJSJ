using Framework.Data.AppBase;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.DAL;
using Lib.ViewModel;

namespace Lib.BLL
{
    public class CollectUserTaskOperator
    {
        #region Generate Code
        public static readonly CollectUserTaskOperator Instance = new CollectUserTaskOperator();

        private static CollectUserTaskAdapter _collectUserTaskAdapter = AdapterFactory.GetAdapter<CollectUserTaskAdapter>();

        #endregion

        public List<CollectUserTaskView> GetList(string templateID, string taskID)
        {
            List<CollectUserTaskView> result = _collectUserTaskAdapter.GetList(templateID, taskID);
            return result;
        }
        public List<CollectUserTaskView> GetProcessList()
        {
            List<CollectUserTaskView> result = _collectUserTaskAdapter.GetProcessList();
            return result;
        }
        public CollectUserTaskView LoadByID(string id)
        {
            CollectUserTaskView result = _collectUserTaskAdapter.LoadByID(id);
            return result;
        }
        public List<CollectUserTaskView> GetMyTaskList(string LoginName)
        {
            List<CollectUserTaskView> result = _collectUserTaskAdapter.GetMyTaskList(LoginName);
            return result;
        }
        public List<CollectUserTaskView> GetListT(string taskName, string tempDate, string creatorName, string tempName)
        {
            List<CollectUserTaskView> result = _collectUserTaskAdapter.GetListT(taskName, tempDate, creatorName, tempName);
            return result;
        }
        public List<CollectUserTaskView> GetMoreMyTaskList(string LoginName, string name, string time)
        {
            return _collectUserTaskAdapter.GetMoreMyTaskList(LoginName, name, time);
        }
        public List<CollectUserTaskView> SearchUserTask(string taskID, string UnitName, string EmployeeName, int FeedBack)
        {
            return _collectUserTaskAdapter.SearchUserTask(taskID, UnitName, EmployeeName, FeedBack);
        }
    }
}
