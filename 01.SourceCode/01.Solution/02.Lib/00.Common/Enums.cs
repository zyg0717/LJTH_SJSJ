using Framework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lib.Common
{

    /// <summary>
    /// 流程审批状态
    /// </summary>
    public enum ProcessStatus
    {
        [EnumItemDescription("未上报")]
        Draft = 0,

        [EnumItemDescription("审批中")]
        Inprocess = 1,

        [EnumItemDescription("批准")]
        Approved = 2,

        [EnumItemDescription("退回")]
        Reject = 4
    }
    /// <summary>
    /// 消息类型 
    /// </summary>
    public enum MessageTypeEnum
    {
        /// <summary>
        /// 短信
        /// </summary>
        SMS = 1,
        /// <summary>
        /// 通知
        /// </summary>
        Alert = 2
    }
}
