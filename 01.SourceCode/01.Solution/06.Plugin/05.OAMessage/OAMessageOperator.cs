
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

namespace Plugin.OAMessage
{
    /// <summary>
    /// TemplateAttachment对象的业务逻辑操作
    /// </summary>
    public class OAMessageOperator : BizOperatorBase<OAMessageEntity>
    {


        #region Generate Code

        public static readonly OAMessageOperator Instance = PolicyInjection.Create<OAMessageOperator>();

        private static OAMessageAdapter _adapter = AdapterFactory.GetAdapter<OAMessageAdapter>();

        protected override BaseAdapterT<OAMessageEntity> GetAdapter()
        {
            return _adapter;
        }

        public PartlyCollection<OAMessageEntity> GetList(OAMessageEntityFilter filter)
        {
            return _adapter.GetList(filter);
        }
        public List<OAMessageEntity> GetList(int flowType, string user)
        {
            return _adapter.GetList(flowType, user);
        }

        public string AddModel(OAMessageEntity data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.AddNewModel(data);
            return result;
        }
        public OAMessageEntity LoadOAMessage(string flowId, string nodename, string receiver)
        {
            return _adapter.LoadOAMessage(flowId, nodename, receiver);
        }


        #endregion
    }
}

