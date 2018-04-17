using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Framework.Web.Controls;

[assembly: WebResource("Framework.Web.Resources.Animations.js", "application/x-javascript")]

namespace Framework.Web.Resources
{
    /// <summary>
    /// 与资源脚本Framework.Web.Resources.Animations.js相关联的类
    /// </summary>
    [ClientScriptResource(null, "Framework.Web.Resources.Animations.js")]
    [RequiredScript(typeof(ControlBaseScript))]
    public static class AnimationsScript
    {
    }
}
