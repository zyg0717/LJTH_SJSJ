
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
    /// TemplateConfig对象的业务逻辑操作
    /// </summary>
    public class TemplateConfigOperator:BizOperatorBase<TemplateConfig>
	{
    
        #region Generate Code
    
        public static readonly TemplateConfigOperator Instance = PolicyInjection.Create<TemplateConfigOperator>();    

        private static TemplateConfigAdapter _templateconfigAdapter = AdapterFactory.GetAdapter<TemplateConfigAdapter>();

        protected override BaseAdapterT<TemplateConfig> GetAdapter()
        {
            return  _templateconfigAdapter;
        }


        public IList<TemplateConfig> GetList()
        {
            IList<TemplateConfig>  result = _templateconfigAdapter.GetList();
            return result;
        }
		         
        public string AddModel(TemplateConfig data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public TemplateConfig GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id==Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(TemplateConfig data)
        {
            string result = base.UpdateModelObject(data);
            return result;
        }

        public string RemoveModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id==Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            string result = base.RemoveObject(id);
            return result;
        }

        public IList<TemplateConfig> GetList(string templateID, string templateSheetID)
        {
            IList<TemplateConfig> result = _templateconfigAdapter.GetList(templateID, templateSheetID);
            return result;

        }
        #endregion
    } 
}

