using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace Framework.Web.Controls
{
    /// <summary>
    /// 翻页绑定控件属性对象
    /// </summary>
    /// <remarks>
    /// 翻页绑定控件属性对象
    /// </remarks>
    public class PagerBoundControlStatus
    {
        private DataListControlType controlMode;
        private bool isPagedControl;
        private bool isDataSourceControl;

        /// <summary>
        /// 控件模型，如：GridView
        /// </summary>
        /// <remarks>
        /// 控件模型，如：GridView
        /// </remarks>
        public DataListControlType DataListControlType
        {
            get { return this.controlMode; }
            set { this.controlMode = value; }
        }
        /// <summary>
        /// 绑定的控件是否为翻页控件
        /// </summary>
        /// <remarks>
        /// 绑定的控件是否为翻页控件
        /// </remarks>
        public bool IsPagedControl
        {
            get { return this.isPagedControl; }
            set { this.isPagedControl = value; }
        }
        /// <summary>
        /// 是否支持数据源控件
        /// </summary>
        /// <remarks>
        /// 是否支持数据源控件
        /// </remarks>
        public bool IsDataSourceControl
        {
            get { return this.isDataSourceControl; }
            set { this.isDataSourceControl = value; }
        }
    }


}
