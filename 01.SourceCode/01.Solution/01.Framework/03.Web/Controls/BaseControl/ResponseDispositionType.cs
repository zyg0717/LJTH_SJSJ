using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Web.Controls
{
    /// <summary>
    /// HttpResponse 返回流 客户端打开类型
    /// </summary>
    public enum ResponseDispositionType
    {
        #region Define Conent
        /// <summary>
        /// 未定义
        /// </summary>
        Undefine = 0,

        /// <summary>
        /// 在浏览器内打开
        /// </summary>
        InnerBrowser = 1,

        /// <summary>
        /// 不提示用户直接打开文件
        /// </summary>
        Inline = 2,

        /// <summary>
        /// 提示用户，用户选择打开文件、保存文件或取消
        /// </summary>
        Attachment = 3
        #endregion
    }
}
