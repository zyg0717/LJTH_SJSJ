using System;
using System.Web.UI;


#region Assembly Resource Attribute
[assembly: System.Web.UI.WebResource("Framework.Web.Resources.ControlBase.js", "text/javascript")]
[assembly: ScriptResource("Framework.Web.Resources.ControlBase.js", "Framework.Web.Resources.ScriptResources.resources", "Framework.Web.Resources")]
#endregion
namespace Framework.Web
{
    /// <summary>
    /// 与资源脚本Framework.Web.Resources.ControlBase.js相关联的类
    /// </summary>
    [ClientScriptResource(null, "Framework.Web.Resources.ControlBase.js")]
    public static class ControlBaseScript
    {

    }
}
