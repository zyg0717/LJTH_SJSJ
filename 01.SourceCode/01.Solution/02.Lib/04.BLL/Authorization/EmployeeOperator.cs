using Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Lib.Model;
using Framework.Data.AppBase;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Lib.DAL;
using Lib.Model.Filter;

namespace Lib.BLL
{
    public class EmployeeOperator : BizOperatorBase<Employee>
    {
        public static EmployeeOperator Instance = PolicyInjection.Create<EmployeeOperator>();
        private static EmployeeAdapter _userinfoAdapter = EmployeeAdapter.Instance;

        
        public Employee GetUserByCode(string id)
        {
            return _userinfoAdapter.GetUserByCode(id);
        }

        public List<Employee> FindUser(string userName)
        {
            return _userinfoAdapter.FindUser(userName);
        }

        internal List<Employee> GetEmployeeList(List<string> userIDs)
        {
            List<Employee> result = _userinfoAdapter.GetBatchModelObjects(userIDs).ToList();
            return result;
        }
        
        protected override BaseAdapterT<Employee> GetAdapter()
        {
           return _userinfoAdapter;
        }
    }
}
