


using Framework.Core;
using Framework.Data.AppBase;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lib.DAL;
using Lib.Model;

namespace Lib.BLL
{
    /// <summary>
    /// AutoProcess对象的业务逻辑操作
    /// </summary>
    public class AutoProcessOperator:BizOperatorBase<AutoProcess>
	{
    
        #region Generate Code
    
        public static readonly AutoProcessOperator Instance = PolicyInjection.Create<AutoProcessOperator>();    

        private static AutoProcessAdapter _autoprocesAdapter = AdapterFactory.GetAdapter<AutoProcessAdapter>();

        protected override BaseAdapterT<AutoProcess> GetAdapter()
        {
            return  _autoprocesAdapter;
        }

        public IList<AutoProcess> GetAutoprocesList()
        {
            IList<AutoProcess>  result = _autoprocesAdapter.GetAutoProcessList();
            return result;
        }
		         
        public string AddAutoproces(AutoProcess data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public AutoProcess GetAutoProcess(string autoprocesID)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(autoprocesID == Guid.Empty.ToString() || string.IsNullOrEmpty(autoprocesID), "Argument id is Empty");
            return base.GetModelObject(autoprocesID);
        }

        public string UpdateAutoproces(AutoProcess data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }

        public string RemoveAutoproces(string autoprocesID)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(autoprocesID == Guid.Empty.ToString() || string.IsNullOrEmpty(autoprocesID), "Argument id is Empty");
            string result = base.RemoveObject(autoprocesID);
            return result;
        }
        
        #endregion

        public IList<AutoProcess> GetAutoprocesWaittingList(string BusinessType, int ErrorCount = 5)
        {
            IList<AutoProcess> result = _autoprocesAdapter.GetAutoprocesWaittingList(BusinessType, ErrorCount);
            return result;
        }
        public IList<AutoProcess> GetWaitingStartWorkflowList(int ErrorCount = 5)
        {
            IList<AutoProcess> result = _autoprocesAdapter.GetWaitingStartWorkflowList(ErrorCount);
            return result;
        }


    }
}

