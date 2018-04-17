using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Lib.Common;

namespace WebApplication.WebAPI.Models.Helper
{
    /// <summary>
    /// 业务数据返回对象
    /// </summary>
    public class BizResult : IHttpActionResult
    {
        HttpRequestMessage _request;
        object _data;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="data"></param>
        public BizResult(object data)
        {
            _data = data;
        }
        /// <summary>
        /// 处理函数
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var result = new
            {
                Result = _data
            };
            var response = new HttpResponseMessage()
            {
                Content = new ObjectContent(result.GetType(), result, new JsonMediaTypeFormatter(), "text/plain"),
                RequestMessage = _request
            };
            return System.Threading.Tasks.Task.FromResult(response);
        }
    }
}