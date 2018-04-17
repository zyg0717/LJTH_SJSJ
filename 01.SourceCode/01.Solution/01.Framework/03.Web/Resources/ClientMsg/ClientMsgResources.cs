using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.Web.Controls;
using System.Web.UI;

namespace Framework.Web.Resources.ClientMsg
{
    /// <summary>
    /// 与资源脚本Framework.Web.Resources.ClientMsg.ClientMsg.js"相关联的类
    /// </summary>
    [RequiredScript(typeof(DeluxeScript))]
    [ClientScriptResource(null, "Framework.Web.Resources.ClientMsg.ClientMsg.js")]
    public sealed class ClientMsgResources
    {
        /// <summary>
        /// 脚本文件名
        /// </summary>
        public static readonly string ScriptFileName = "ClientMsg.js";

        /// <summary>
        /// 获取资源文件url
        /// </summary>
        /// <param name="clientScript"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetClientMsgResourceUrl(ClientScriptManager clientScript, string fileName)
        {
            return clientScript.GetWebResourceUrl(typeof(ClientMsgResources), GetFileFullName(fileName));
        }

        /// <summary>
        /// 获取资源文件全名
        /// </summary>
        /// <param name="fileName">资源文件</param>
        /// <returns>资源文件全名</returns>
        public static string GetFileFullName(string fileName)
        {
            return string.Format("Framework.Web.Resources.ClientMsg.{0}", fileName);
        }
    }
}
