﻿
using System;
using System.Collections.Generic;
using Framework.Data;
using Framework.Data.AppBase;
using Lib.Model;
using System.Linq;
using Lib.Common;
using Lib.Model.Filter;

namespace Lib.DAL
{



    /// <summary>
    /// PreviewRelation对象的数据访问适配器
    /// </summary>
    public class PreviewRelationAdapter : AppBaseAdapterT<PreviewRelation>
    {
        //public TemplateTask GetModel(string userName, string templateConfigInstanceID)
        //{
        //    return base.Load(p => { p.AppendItem("EmployeeLoginName", userName); p.AppendItem("templateConfigInstanceID", templateConfigInstanceID); p.AppendItem("isdeleted", 0); }).FirstOrDefault();
        //}
    }
}

