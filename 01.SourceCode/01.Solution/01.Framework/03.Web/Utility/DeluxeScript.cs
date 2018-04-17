using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Web.Resources
{
    /// <summary>
    /// 
    /// </summary>
    [ClientScriptResource(null, "Framework.Web.Resources.Deluxe.js")]
    public sealed class DeluxeScript
    {
        /// <summary>
        /// 接收命令的客户端Input控件ID
        /// </summary>
        public const string C_CommandIputClientID = "__commandInput";
    }
}
