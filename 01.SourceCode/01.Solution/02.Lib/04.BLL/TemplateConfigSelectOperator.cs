
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
    /// TemplateConfigSelect对象的业务逻辑操作
    /// </summary>
    public class TemplateConfigSelectOperator:BizOperatorBase<TemplateConfigSelect>
	{
    
        #region Generate Code
    
        public static readonly TemplateConfigSelectOperator Instance = PolicyInjection.Create<TemplateConfigSelectOperator>();    

        private static TemplateConfigSelectAdapter _templateconfigselectAdapter = AdapterFactory.GetAdapter<TemplateConfigSelectAdapter>();

        protected override BaseAdapterT<TemplateConfigSelect> GetAdapter()
        {
            return  _templateconfigselectAdapter;
        }


        public IList<TemplateConfigSelect> GetList()
        {
            IList<TemplateConfigSelect>  result = _templateconfigselectAdapter.GetList();
            return result;
        }
		         
        public string AddModel(TemplateConfigSelect data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public TemplateConfigSelect GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id==Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(TemplateConfigSelect data)
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

        public IList<TemplateConfigSelect> GetList(string templateID, string templateSheetID, string configID)
        {
            IList<TemplateConfigSelect> result = _templateconfigselectAdapter.GetList(templateID, templateSheetID, configID);
            return result;
        }
        public IList<TemplateConfigSelect> GetList(string templateID)
        {
            IList<TemplateConfigSelect> result = _templateconfigselectAdapter.GetList(templateID);
            return result;
        }
        #endregion
    } 
}

