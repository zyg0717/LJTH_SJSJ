using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Web.Controls
{
    /// <summary>
    /// pager设计模式类
    /// </summary>
    /// <remarks>
    ///  pager设计模式类
    /// </remarks>
    internal class DeluxePagerDesigner : DesignerBase
    {
        /// <summary>
        /// GetEmptyDesignTimeHtml
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///  GetEmptyDesignTimeHtml
        /// </remarks>
        protected override string GetEmptyDesignTimeHtml()
        {
            return CreatePlaceHolderDesignTimeHtml("To configure and style this GridView, please switch to HTML view.");
        }
    }
}
