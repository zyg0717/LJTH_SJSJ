using WebApplication.WebAPI.Helper.Log;
using Framework.Web.Security.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Lib.BLL;
using Lib.Common;
using NLog;
using Framework.Web.Utility;

namespace WebApplication.WebAPI.Models.Helper
{
    /// <summary>
    /// 登陆状态权限验证
    /// </summary>
    public class UserAuthorizeAttribute : ActionFilterAttribute
    {
        private bool _IsExcept { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public UserAuthorizeAttribute() : base()
        {

        }
        private static readonly ILogger logger = LogManager.GetLogger("UserAuthorizeAttribute", typeof(UserAuthorizeAttribute));
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isExcept"></param>
        public UserAuthorizeAttribute(bool isExcept) : base()
        {
            _IsExcept = isExcept;
        }
        /// <summary>
        /// 登陆状态验证拦截
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var requestQuery = actionContext.Request.RequestUri.PathAndQuery;
            var content = string.Empty;
            var readTask = actionContext.Request.Content.ReadAsStreamAsync();
            using (System.IO.Stream sm = readTask.Result)
            {
                if (sm != null && sm.Length > 0)
                {
                    sm.Seek(0, SeekOrigin.Begin);
                    int len = (int)sm.Length;
                    byte[] inputByts = new byte[len];
                    sm.Read(inputByts, 0, len);
                    sm.Close();
                    content = Encoding.UTF8.GetString(inputByts);
                }
            }
            NLogTraceHelper.Instace.Debug("请求地址：{0},提交参数：{1}", requestQuery, content);

            var isAuth = WebHelper.GetCurrentUser() != null;
            logger.Info(string.Format("请求地址：{0},提交参数：{1}", requestQuery, content));
            logger.Info(string.Format("登陆状态验证：{0}", isAuth));
            if (!isAuth && !_IsExcept)
            {
                throw new BizException(System.Net.HttpStatusCode.Forbidden, "登陆状态缺失");
            }
            base.OnActionExecuting(actionContext);
        }
    }
}