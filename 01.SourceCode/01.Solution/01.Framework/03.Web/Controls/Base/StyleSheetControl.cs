using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Framework.Web.Controls
{
    internal enum StyleSheetPositionMode
    {
        Header = 1,
        BodyStart = 2,
        BodyEnd = 3
    }

    internal class StyleSheetControl : Control
    {
        private const string _C_CSS_LINK_FORMAT = "<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}\" />";
        private Dictionary<string, StyleSheetPositionMode> _UrlContainer = new Dictionary<string, StyleSheetPositionMode>();

        public void Add(string cssUrl, StyleSheetPositionMode mode)
        {
            //string url = cssUrl.Trim().ToLower();
            if (!_UrlContainer.ContainsKey(cssUrl))
            {
                _UrlContainer.Add(cssUrl, mode);
                this.BuildChildControl(cssUrl, mode);
            }
        }

        private void BuildChildControl(string cssUrl, StyleSheetPositionMode mode)
        {
            switch (mode)
            {
                case StyleSheetPositionMode.Header:
                    HtmlLink link = new HtmlLink();
                    link.Href = cssUrl;
                    link.Attributes.Add("type", "text/css");
                    link.Attributes.Add("rel", "stylesheet");
                    this.Controls.Add(link);
                    break;

                case StyleSheetPositionMode.BodyStart:
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(),
                        string.Format(_C_CSS_LINK_FORMAT, cssUrl), false);
                    break;

                case StyleSheetPositionMode.BodyEnd:
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(),
                        string.Format(_C_CSS_LINK_FORMAT, cssUrl), false);
                    break;

            }
        }
    }
}
