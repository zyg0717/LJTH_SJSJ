
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
    /// OnlineTaskFileRelation对象的业务逻辑操作
    /// </summary>
    public class OnlineTaskFileRelationOperator : BizOperatorBase<OnlineTaskFileRelation>
    {

        #region Generate Code

        public static readonly OnlineTaskFileRelationOperator Instance = PolicyInjection.Create<OnlineTaskFileRelationOperator>();

        private static OnlineTaskFileRelationAdapter _Adapter = AdapterFactory.GetAdapter<OnlineTaskFileRelationAdapter>();

        public bool QueryTaskFileStatus(string businessid)
        {
            return _Adapter.QueryTaskFileStatus(businessid);
        }

        protected override BaseAdapterT<OnlineTaskFileRelation> GetAdapter()
        {
            return _Adapter;
        }

        public string AddModel(OnlineTaskFileRelation data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }

        public string UpdateModel(OnlineTaskFileRelation data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }

        public OnlineTaskFileRelation GetModel(string id)
        {
            return base.GetModelObject(id);
        }


        #endregion
    }
}

