
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
using Lib.ViewModel;

namespace Lib.BLL
{
    /// <summary>
    /// DataCollectUser对象的业务逻辑操作
    /// </summary>
    public class DataCollectUserOperator : BizOperatorBase<DataCollectUser>
    {

        #region Generate Code

        public static readonly DataCollectUserOperator Instance = PolicyInjection.Create<DataCollectUserOperator>();

        private static DataCollectUserAdapter _datacollectuserAdapter = AdapterFactory.GetAdapter<DataCollectUserAdapter>();
        private static VCollectUserAdapter _VCollectUserAdapter = AdapterFactory.GetAdapter<VCollectUserAdapter>();

        protected override BaseAdapterT<DataCollectUser> GetAdapter()
        {
            return _datacollectuserAdapter;
        }

        public IList<DataCollectUser> GetList()
        {
            IList<DataCollectUser> result = _datacollectuserAdapter.GetList();
            return result;
        }
        public IList<DataCollectUser> GetList(string configInstanceID)
        {
            IList<DataCollectUser> result = _datacollectuserAdapter.GetList(configInstanceID);
            return result;
        }
        public PartlyCollection<VCollectUser> GetListWithLastTask(VCollectUserFilter filter)
        {
            return _VCollectUserAdapter.GetList(filter);

        }
        public string AddModel(DataCollectUser data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }
        public DataCollectUser TryGetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelWithDeletedObject(id);
        }
        public DataCollectUser GetModel(string id)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(id == Guid.Empty.ToString() || string.IsNullOrEmpty(id), "Argument id is Empty");
            return base.GetModelObject(id);
        }

        public string UpdateModel(DataCollectUser data)
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

        #endregion
    }
}

