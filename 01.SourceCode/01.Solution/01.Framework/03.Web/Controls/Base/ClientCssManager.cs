using System.Web.UI;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 在页面中注册Css文件帮助函数
    /// </summary>
    /// <remarks>在页面中注册Css文件帮助函数</remarks>
    public static class ClientCssManager
    {
        private const string _C_CSS_CONTROL_ID = "__StyleSheetControlID";
        private const string _C_HEADER_END_CSS_CONTROL_ID = "__HeaderEndStyleSheetControlID";
        private static StyleSheetControl GetStyleSheetControl(Page page)
        {
            StyleSheetControl ctr = (StyleSheetControl)page.Header.FindControl(_C_CSS_CONTROL_ID);
            if (ctr == null)
            {
                ctr = new StyleSheetControl();
                ctr.ID = _C_CSS_CONTROL_ID;
                page.Header.Controls.AddAt(0, ctr);
            }

            return ctr;
        }

        private static StyleSheetControl GetHeaderEndStyleSheetControl(Page page)
        {
            StyleSheetControl ctr = (StyleSheetControl)page.Header.FindControl(_C_HEADER_END_CSS_CONTROL_ID);
            if (ctr == null)
            {
                ctr = new StyleSheetControl();
                ctr.ID = _C_HEADER_END_CSS_CONTROL_ID;
                page.Header.Controls.Add(ctr);
            }

            return ctr;
        }

        /// <summary>
        /// 在Head之间注册Css文件
        /// </summary>
        /// <param name="page">当前页面</param>
        /// <param name="cssUrl">css文件url路径</param>
        /// <remarks>在Head之间注册Css</remarks>
        public static void RegisterHeaderCss(Page page, string cssUrl)
        {
            StyleSheetControl ctr = GetStyleSheetControl(page);
            ctr.Add(cssUrl, StyleSheetPositionMode.Header);
        }

        /// <summary>
        /// 在Head结束前注册Css文件
        /// </summary>
        /// <param name="page">当前页面</param>
        /// <param name="cssUrl">css文件url路径</param>
        /// <remarks>在Head之间注册Css</remarks>
        public static void RegisterHeaderEndCss(Page page, string cssUrl)
        {
            StyleSheetControl ctr = GetHeaderEndStyleSheetControl(page);
            ctr.Add(cssUrl, StyleSheetPositionMode.Header);
        }

        /// <summary>
        /// 在body开始后注册Css文件
        /// </summary>
        /// <param name="page">当前页面</param>
        /// <param name="cssUrl">css文件url路径</param>
        /// <remarks>在body开始后注册Css</remarks>
        public static void RegisterBodyStartCss(Page page, string cssUrl)
        {
            StyleSheetControl ctr = GetStyleSheetControl(page);
            ctr.Add(cssUrl, StyleSheetPositionMode.BodyStart);
        }

        /// <summary>
        /// 在body结束前注册Css文件
        /// </summary>
        /// <param name="page">当前页面</param>
        /// <param name="cssUrl">css文件url路径</param>
        /// <remarks>在body结束前注册Css</remarks>
        public static void RegisterBodyEndCss(Page page, string cssUrl)
        {
            StyleSheetControl ctr = GetStyleSheetControl(page);
            ctr.Add(cssUrl, StyleSheetPositionMode.BodyEnd);
        }
    }

}
