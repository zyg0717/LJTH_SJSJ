using System;
using System.Collections.Generic;
using System.Configuration;
using Framework.Core;
using Framework.Data.AppBase;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using Lib.Common;
using Lib.DAL;
using Lib.Model;

namespace Lib.BLL
{
    public class TSM_MessagesOperator : BizOperatorBase<TSM_Messages>
    {
        public static readonly TSM_MessagesOperator Instance = PolicyInjection.Create<TSM_MessagesOperator>();
        private static TSM_MessagesAdapter _TSM_MessagesAdapter = AdapterFactory.GetAdapter<TSM_MessagesAdapter>();

        protected override BaseAdapterT<TSM_Messages> GetAdapter()
        {
            return _TSM_MessagesAdapter;
        }
        internal IList<TSM_Messages> GetTSM_MessagesList()
        {
            IList<TSM_Messages> result = _TSM_MessagesAdapter.GetMessagesList();
            return result;
        }
        //public TSM_Messages GetTSM_Messages(string Mid)
        //{
        //    ExceptionHelper.TrueThrow<ArgumentNullException>(Mid == null, "Argument Mid is Empty");
        //    return base.GetModelObject(Mid);
        //}
        public void AddTSM_Messages(TSM_Messages data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");

            //if (data.MessageType == 2)
            //{
            //    var flag = ConfigurationManager.AppSettings["NotifyCtxFlag"];
            //    if (string.IsNullOrEmpty(flag) || flag.ToLower() != "true")
            //    {
            //        data.Target = ConstSet.AdminUserName;
            //    }
            //}
            if (data.MessageType == 1)
            {
                string content = data.Content;
                int total = content.Length;
                int step = 2000;
                int i = 0;
                int max = (total - 1) / step + 1;
                for (i = 0; i < max; i++)
                {
                    string msg = "";
                    if (i == (max - 1))
                    {
                        msg = content.Substring(i * step);
                    }
                    else
                    {
                        msg = content.Substring(i * step, step - 1);
                    }
                    TSM_Messages newData = data;
                    newData.ID = Guid.NewGuid().ToString();
                    newData.Content = msg;
                    base.AddNewModel(newData);
                }
            }
            else
            {
                base.AddNewModel(data);
            }

        }
        public string UpdateTSM_Messages(TSM_Messages data)
        {
            ExceptionHelper.TrueThrow<ArgumentNullException>(data == null, "Argument data is Empty");
            string result = base.UpdateModelObject(data);
            return result;
        }
        //public int RemoveTSM_Messages(TSM_Messages data)
        //{
        //    int i = _TSM_MessagesAdapter.Remove(data);
        //    return i;
        //}

        public List<TSM_Messages> LoadPendingList(int topCount,int maxTryCount)
        {
            return _TSM_MessagesAdapter.LoadPendingList(topCount, maxTryCount);
        }
    }
}
