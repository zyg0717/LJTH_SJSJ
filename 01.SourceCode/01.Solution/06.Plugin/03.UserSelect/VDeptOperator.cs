
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
    public class VDeptOperator : BizOperatorBase<VDeptEntity>
    {


        #region Generate Code

        public static readonly VDeptOperator Instance = PolicyInjection.Create<VDeptOperator>();

        private static VDeptAdapter _adapter = AdapterFactory.GetAdapter<VDeptAdapter>();

        protected override BaseAdapterT<VDeptEntity> GetAdapter()
        {
            return _adapter;
        }


        public string AddModel(VDeptEntity data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }
        public List<VDeptEntity> LoaDeptList(int pid)
        {
            return _adapter.LoaDeptList(pid);
        }

        internal object LoadPreDeptList(int oid)
        {
            return _adapter.LoadPreDeptList(oid);
        }


        #endregion
    }
}

