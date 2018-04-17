
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using System.Linq;
using Framework.Core;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Lib.Common;
using System.Web;
using System.IO;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;

namespace Plugin.UserSelect
{
    /// <summary>
    /// TemplateAttachment对象的业务逻辑操作
    /// </summary>
    public class VEmployeeOperator
    {


        #region Generate Code

        public static readonly VEmployeeOperator Instance = new VEmployeeOperator();

        private static VEmployeeAdapter _adapter = AdapterFactory.GetAdapter<VEmployeeAdapter>();



        internal VEmployeeEntity GetEmployeeByID(int userId)
        {
            return _adapter.GetEmployeeByID(userId);
        }

        public List<VEmployeeEntity> LoadUserList(int deptID)
        {
            return _adapter.LoadUserList(deptID);
        }

        internal List<VEmployeeEntity> LoadUserList(string employeeName, string dept, string job, string userName, string keyWord)
        {
            return _adapter.LoadUserList(employeeName, dept, job, userName, keyWord);
        }

        internal List<VEmployeeEntity> LoadUserList(string key)
        {
            return _adapter.LoadUserList(key);
        }


        #endregion
    }
}

