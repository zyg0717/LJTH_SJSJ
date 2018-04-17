
using System;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using System.Collections.Generic;
using Framework.Data.AppBase;
using Lib.Model;
using Lib.DAL;
using Framework.Core;
using System.Web;
using Framework.Web.Security.Authentication;
using Lib.Common;
using Aspose.Cells;
using Framework.Web;
using Lib.ViewModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Lib.BLL
{
    /// <summary>
    /// PreviewRelation对象的业务逻辑操作
    /// </summary>
    public class PreviewRelationOperator : BizOperatorBase<PreviewRelation>
    {

        #region Generate Code

        public static readonly PreviewRelationOperator Instance = PolicyInjection.Create<PreviewRelationOperator>();

        private static PreviewRelationAdapter _Adapter = AdapterFactory.GetAdapter<PreviewRelationAdapter>();

        

        protected override BaseAdapterT<PreviewRelation> GetAdapter()
        {
            return _Adapter;
        }

        public string AddModel(PreviewRelation data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public string UpdateModel(PreviewRelation data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }

        public PreviewRelation GetModel(string id)
        {
            return base.GetModelObject(id);
        }


        #endregion
    }
}

