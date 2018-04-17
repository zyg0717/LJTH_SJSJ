using WebApplication.WebAPI.Models.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;


namespace WebApplication.WebAPI.Controllers.Base
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*", SupportsCredentials = true, PreflightMaxAge = 1728000)]
    [UserAuthorize]
    public class BaseController : ApiController
    {
        /// <summary>
        /// 生成返回数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal BizResult BizResult(object data)
        {
            return new BizResult(data);
        }
    }
}