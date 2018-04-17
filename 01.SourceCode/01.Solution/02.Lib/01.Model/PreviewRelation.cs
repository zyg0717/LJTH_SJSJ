using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Data;
using Framework.Data.AppBase;


namespace Lib.Model
{
    /// <summary>
    /// This object represents the properties and methods of a TemplateTask.
    /// </summary>
    [ORTableMapping("dbo.PreviewRelation")]
    public class PreviewRelation : BaseModel
    {

        #region Public Properties


        [ORFieldMapping("BusinessID")]
        public string BusinessID { get; set; }

        [ORFieldMapping("AccessToken")]
        public string AccessToken { get; set; }

        [ORFieldMapping("DocumentLink")]
        public string DocumentLink { get; set; }

        [ORFieldMapping("DocumentFileName")]
        public string DocumentFileName { get; set; }

        [ORFieldMapping("DocumentByteLength")]
        public long DocumentByteLength { get; set; }

        [NoMapping]
        public string DocumentLength { get; set; }

        [ORFieldMapping("DocumentFileCode")]
        public string DocumentFileCode { get; set; }


        #endregion


    }
}

