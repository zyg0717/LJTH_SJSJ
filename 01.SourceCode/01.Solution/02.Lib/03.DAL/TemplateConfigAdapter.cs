
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;

namespace Lib.DAL
{



    /// <summary>
    /// TemplateConfig对象的数据访问适配器
    /// </summary>
    public class TemplateConfigAdapter : AppBaseAdapterT<TemplateConfig>
    {
        public IList<TemplateConfig> GetList()
        {
            string sql = ORMapping.GetSelectSql<TemplateConfig>(TSqlBuilder.Instance);

            sql += "WHERE " + base.NotDeleted;

            return ExecuteQuery(sql);
        }

        public IList<TemplateConfig> GetList(string templateID, string templateSheetID)
        {
            if (string.IsNullOrEmpty(templateSheetID))
            {
                return base.Load(p => { p.AppendItem("templateID", templateID); p.AppendItem("isdeleted", 0); });
            }
            else
            {
                return base.Load(p => { p.AppendItem("templateID", templateID); p.AppendItem("templateSheetID", templateSheetID); p.AppendItem("isdeleted", 0); });
            }

        }
    }
}

