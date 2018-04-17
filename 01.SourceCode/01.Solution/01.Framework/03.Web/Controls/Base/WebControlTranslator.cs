﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class WebControlTranslator : ControlTranslatorGenericBase<WebControl>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="control"></param>
        public WebControlTranslator(WebControl control)
            : base(control)
        {
        }

        /// <summary>
        ///
        /// </summary>
        public override void Translate()
        {
            this.Control.ToolTip = Translate(this.Control.ToolTip);
        }
    }
}
