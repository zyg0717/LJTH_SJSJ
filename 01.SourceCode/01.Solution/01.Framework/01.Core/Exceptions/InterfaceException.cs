
using System;
namespace Framework.Core
{

    /// <summary>
    /// 数据验证失败异常
    /// </summary>
    public class InterfaceException : SystemSupportException
    {
        public InterfaceException():base()
        {
            
        }

        public InterfaceException(string message):base(message)
        {
            
        }
        public InterfaceException(string message, Exception innerException)
            : base(message,innerException)
        {
              
        }

        public override string Message
        {
            get
            {
                return "数据验证失败。"+base.Message;
            }
        }

     
    }
}
