
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;
using Lib.Model.Filter;

namespace Lib.DAL
{



    /// <summary>
    /// DataCollectUser对象的数据访问适配器
    /// </summary>
    public class DataCollectUserAdapter : AppBaseAdapterT<DataCollectUser>
    {

        public IList<DataCollectUser> GetList()
        {
            string sql = ORMapping.GetSelectSql<DataCollectUser>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public List<DataCollectUser> GetList(string configInstanceID, DataCollectUserFilter filter = null)
        {
            return base.Load(p =>
            {
                p.AppendItem("templateConfigInstanceID", configInstanceID);
                p.AppendItem("isdeleted", 0);
                p.AppendItem("ProcessStatus", 0);

                if (filter != null)
                {
                    if (filter.FeedBack != -1)
                    {
                        p.AppendItem("(select count(1) from TemplateTask where DataCollectUserID=DataCollectUser.ID and ProcessStatus=0 and isdeleted=0 and Status=2)", filter.FeedBack);
                    }
                    if (!string.IsNullOrEmpty(filter.UnitName))
                    {
                        p.AppendItem("UnitName", filter.UnitName);
                    }
                    if (!string.IsNullOrEmpty(filter.EmployeeName))
                    {
                        p.AppendItem("EmployeeName", filter.EmployeeName);
                    }
                }
            });
        }

    }
}

