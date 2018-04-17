
using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using System.Linq;
using Framework.Core;
using Lib.Model.Filter;

namespace Lib.BLL
{
    /// <summary>
    /// TemplateConfigInstance对象的业务逻辑操作
    /// </summary>
    public class TemplateConfigInstancePlanOperator : BizOperatorBase<TemplateConfigInstancePlan>
    {

        #region Generate Code

        public static readonly TemplateConfigInstancePlanOperator Instance = PolicyInjection.Create<TemplateConfigInstancePlanOperator>();

        private static TemplateConfigInstancePlanAdapter _templateconfiginstancePlanAdapter = AdapterFactory.GetAdapter<TemplateConfigInstancePlanAdapter>();

        protected override BaseAdapterT<TemplateConfigInstancePlan> GetAdapter()
        {
            return _templateconfiginstancePlanAdapter;
        }

        public string AddModel(TemplateConfigInstancePlan data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public TemplateConfigInstancePlan GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }
        public TemplateConfigInstancePlan GetModelBySubTask(string subTaskId)
        {
            var totalCount = 0;
            return _templateconfiginstancePlanAdapter.GetList(new TemplateConfigInstancePlanFilter()
            {
                SubTemplateConfigInstanceID = subTaskId
            }, out totalCount).FirstOrDefault();
        }
        public string UpdateModel(TemplateConfigInstancePlan data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }

        public string RemoveModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            string result = base.RemoveObject(id);
            return result;
        }

        public IList<TemplateConfigInstancePlan> GetList(TemplateConfigInstancePlanFilter filter, out int totalCount)
        {
            return _templateconfiginstancePlanAdapter.GetList(filter, out totalCount);
        }

        public IList<TemplateConfigInstancePlan> GetPendingList(int topCount)
        {
            return _templateconfiginstancePlanAdapter.GetPendingList(topCount);
        }
        public IList<TemplateConfigInstancePlan> GetPendingSubmitList(int topCount)
        {
            return _templateconfiginstancePlanAdapter.GetPendingSubmitList(topCount);
        }
        #endregion

    }
}

