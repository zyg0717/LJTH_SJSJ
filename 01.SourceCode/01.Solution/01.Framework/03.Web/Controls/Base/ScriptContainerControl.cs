using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Framework.Core;

namespace Framework.Web.Controls
{
    internal enum ScriptPositionMode
    {
        Header = 1,
        BodyStart = 2,
        ScriptManager = 3,
        BodyEnd = 4
    }


    internal class ScriptContainerControl : Control
    {
        private const string _C_SCRIPT_FORMAT = "<script type=\"text/javascript\" src=\"{0}\"></script>";
        private Dictionary<string, ScriptPositionMode> _UrlContainer = new Dictionary<string, ScriptPositionMode>();

        public void Add(string scriptUrl, ScriptPositionMode mode)
        {
            //string url = cssUrl.Trim().ToLower();
            if (!_UrlContainer.ContainsKey(scriptUrl))
            {
                _UrlContainer.Add(scriptUrl, mode);
                this.BuildChildControl(scriptUrl, mode);
            }
        }

        private void BuildChildControl(string scriptUrl, ScriptPositionMode mode)
        {
            switch (mode)
            {
                case ScriptPositionMode.Header:
                    HtmlGenericControl ctr = new HtmlGenericControl("script");
                    ctr.Attributes.Add("language", "javascript");
                    ctr.Attributes.Add("type", "text/javascript");
                    ctr.Attributes.Add("src", scriptUrl);
                    this.Controls.Add(ctr);
                    break;

                case ScriptPositionMode.ScriptManager:
                    ScriptManager sm = ScriptManager.GetCurrent(this.Page);
                    ExceptionHelper.TrueThrow(sm == null, Resources.DeluxeWebResource.E_NoScriptManager);
                    sm.Scripts.Add(new ScriptReference(scriptUrl));
                    break;

                case ScriptPositionMode.BodyStart:
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(),
                        string.Format(_C_SCRIPT_FORMAT, scriptUrl), false);
                    break;

                case ScriptPositionMode.BodyEnd:
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(),
                        string.Format(_C_SCRIPT_FORMAT, scriptUrl), false);
                    break;

            }
        }
    }
}
