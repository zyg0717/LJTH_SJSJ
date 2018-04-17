
using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using System.Linq;
using Lib.DAL;
using Framework.Core;

namespace Lib.BLL
{
    /// <summary>
    /// Enum对象的业务逻辑操作
    /// </summary>
    public class EnumOperator:BizOperatorBase<Model.Enum>
	{
    
        #region Generate Code
    
        public static readonly EnumOperator Instance = PolicyInjection.Create<EnumOperator>();    

        private static EnumAdapter _enumAdapter = AdapterFactory.GetAdapter<EnumAdapter>();

        protected override BaseAdapterT<Model.Enum> GetAdapter()
        {
            return  _enumAdapter;
        }


        public IList<Model.Enum> GetList()
        {
            IList<Model.Enum>  result = _enumAdapter.GetList();
            return result;
        }

        public IList<Model.Enum> GetList(string type)
        {
            IList<Model.Enum> result = _enumAdapter.GetList(type);
            return result;
        }

        public string AddModel(Model.Enum data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public Model.Enum GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id== Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(Model.Enum data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }

        public string RemoveModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id==Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            string result = base.RemoveObject(id);
            return result;
        }
        
        #endregion
    } 
}

