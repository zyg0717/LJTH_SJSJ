
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;

namespace Lib.DAL
{



    /// <summary>
    /// TemplateConfigSelect对象的数据访问适配器
    /// </summary>
    public class TemplateConfigSelectAdapter : AppBaseAdapterT<TemplateConfigSelect>
    {
        public IList<TemplateConfigSelect> GetList()
        {
            string sql = ORMapping.GetSelectSql<TemplateConfigSelect>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public IList<TemplateConfigSelect> GetList(string templateID, string templateSheetID, string configID)
        {
            return base.Load(p => {
                p.AppendItem("templateID", templateID);
                p.AppendItem("templateSheetID", templateSheetID);
                p.AppendItem("TemplateConfigID", configID);
                p.AppendItem("isdeleted", 0); });
        }
        public IList<TemplateConfigSelect> GetList(string templateID)
        {
            return base.Load(p => {
                p.AppendItem("templateID", templateID);
                p.AppendItem("isdeleted", 0);
            });
        }
    }
}

