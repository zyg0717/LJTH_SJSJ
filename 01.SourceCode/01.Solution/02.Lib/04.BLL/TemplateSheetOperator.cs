
using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using System.Linq;
using Framework.Core;

namespace Lib.BLL
{
    /// <summary>
    /// TemplateSheet对象的业务逻辑操作
    /// </summary>
    public class TemplateSheetOperator:BizOperatorBase<TemplateSheet>
	{
    
        #region Generate Code
    
        public static readonly TemplateSheetOperator Instance = PolicyInjection.Create<TemplateSheetOperator>();    

        private static TemplateSheetAdapter _templatesheetAdapter = AdapterFactory.GetAdapter<TemplateSheetAdapter>();

        protected override BaseAdapterT<TemplateSheet> GetAdapter()
        {
            return  _templatesheetAdapter;
        }



        public IList<TemplateSheet> GetList()
        {
            IList<TemplateSheet>  result = _templatesheetAdapter.GetList();
            return result;
        }
        public IList<TemplateSheet> GetList(string templetedID)
        {
            IList<TemplateSheet> result = _templatesheetAdapter.GetList(templetedID);
            return result;
        }
        public string AddModel(TemplateSheet data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public TemplateSheet GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id==Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(TemplateSheet data)
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

