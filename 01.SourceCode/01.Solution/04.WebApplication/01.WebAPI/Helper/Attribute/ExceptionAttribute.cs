using WebApplication.WebAPI.Helper.Log;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Tracing;
using Lib.Common;

namespace WebApplication.WebAPI.Models.Helper
{
    /// <summary>
    /// 统一异常处理
    /// </summary>
    public class ExceptionAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// 异常信息捕获
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            //ex.StackTrace
            var ex = actionExecutedContext.Exception;
            //先log一下 在处理
            if (!(ex is BizException))
            {
                var code = Guid.NewGuid().ToString("X");
                NLogTraceHelper.Instace.Error(ex, "异常代码：{0}", code);
                throw new BizException(string.Format("数据处理异常，异常代码：\r\n{0}", code));
            }
            base.OnException(actionExecutedContext);
        }
    }
}