
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;

namespace Lib.DAL
{



    /// <summary>
    /// TemplateSheet对象的数据访问适配器
    /// </summary>
    public class TemplateSheetAdapter : AppBaseAdapterT<TemplateSheet>
    {
        public IList<TemplateSheet> GetList()
        {
            string sql = ORMapping.GetSelectSql<TemplateSheet>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public IList<TemplateSheet> GetList(string templetedID)
        {
            return base.Load(p => { p.AppendItem("TemplateID", templetedID); p.AppendItem("ISDELETED", 0); });
        }
    }
}

