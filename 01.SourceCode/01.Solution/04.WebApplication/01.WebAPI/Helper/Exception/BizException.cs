using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebApplication.WebAPI.Models.Helper
{
    /// <summary>
    /// 业务异常处理
    /// </summary>
    public class BizException : HttpResponseException
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msg"></param>
        public BizException(string msg) : base(new HttpResponseMessage())
        {
            this.Response.Content = new StringContent(msg);
            this.Response.StatusCode = System.Net.HttpStatusCode.Conflict;
        }
        /// <summary>
        /// 构造函数重载
        /// </summary>
        /// <param name="httpCode"></param>
        /// <param name="msg"></param>
        public BizException(System.Net.HttpStatusCode httpCode, string msg) : base(new HttpResponseMessage())
        {
            this.Response.Content = new StringContent(msg);
            this.Response.StatusCode = httpCode;
        }
    }
}