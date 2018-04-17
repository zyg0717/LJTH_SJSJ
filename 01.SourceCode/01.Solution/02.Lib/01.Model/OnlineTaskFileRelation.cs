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
    [ORTableMapping("dbo.OnlineTaskFileRelation")]
    public class OnlineTaskFileRelation : BaseModel
    {

        #region Public Properties


        [ORFieldMapping("BusinessID")]
        public string BusinessID { get; set; }

        [ORFieldMapping("AccessToken")]
        public string AccessToken { get; set; }

        /// <summary>
        /// 1为发起在线填报 2为待验证 3为上传完成 4为验证失败
        /// </summary>
        [ORFieldMapping("TaskStatus")]
        public int TaskStatus { get; set; }

        [ORFieldMapping("ResultMsg")]
        public string ResultMsg { get; set; }

        [ORFieldMapping("TaskLink")]
        public string TaskLink { get; set; }

        [ORFieldMapping("TaskInfoFileName")]
        public string TaskInfoFileName { get; set; }

        [ORFieldMapping("TaskInfoFileByteLength")]
        public long TaskInfoFileByteLength { get; set; }

        [ORFieldMapping("TaskInfoFileCode")]
        public string TaskInfoFileCode { get; set; }

        [ORFieldMapping("TaskFileName")]
        public string TaskFileName { get; set; }

        [ORFieldMapping("TaskFileByteLength")]
        public long TaskFileByteLength { get; set; }

        [ORFieldMapping("TaskFileCode")]
        public string TaskFileCode { get; set; }


        #endregion


    }
}

