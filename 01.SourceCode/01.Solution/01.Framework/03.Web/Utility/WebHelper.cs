using Framework.Core.Cache;
using Framework.Core.Config;
using Framework.Core.Log;
using System;
using System.Configuration;
using System.Web;
using System.Web.Hosting;
using Framework.Web.Security.Authentication;
using System.Threading;
using System.Collections.Generic;
using System.Text;

namespace Framework.Web.Utility
{
    /// <summary>
    /// 帮助类， 封装常用的Web后台调用的静态方法
    /// </summary>
    public class WebHelper
    {
        public static WebHelper Instance = new WebHelper();

        /// <summary>
        /// 需要在程序启动时注册的一个事件： 根据用户登陆名返回用户的的信息
        /// </summary>
        public event Func<string, Framework.Web.Security.Authentication.LoginUserInfo> GetUser;
        /// <summary>
        /// 获得当前的登陆用户信息
        /// </summary>
        /// <returns></returns>
        public Framework.Web.Security.Authentication.LoginUserInfo GetLoginUserInfo()
        {
            string cacheKey = null;
            Framework.Web.Security.Authentication.LoginUserInfo result = null;
            string IsUservirtualUser = ConfigurationManager.AppSettings["IsVirtualUser"];
            IsUservirtualUser = string.IsNullOrEmpty(IsUservirtualUser) ? "true" : IsUservirtualUser;


            if (HttpContext.Current == null || Convert.ToBoolean(IsUservirtualUser))
            {
                #region
                // 提供一种在单元测试环境下可以模拟的机制
                string mockCurrentUserLoginName = ConfigurationManager.AppSettings["VirtualUser"];
                if (!string.IsNullOrEmpty(mockCurrentUserLoginName))
                {
                    cacheKey = mockCurrentUserLoginName;
                    result = GetUser(cacheKey);
                }
                else
                {
                    throw new ApplicationException("必须是在Web环境下执行此方法。");
                }
                #endregion
            }
            else
            {
                string ssoUsername = GetMemberIdSSoCookies();

                string strUserName = !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name) ? HttpContext.Current.User.Identity.Name : ssoUsername;

                strUserName = HttpContext.Current.Items["LoginIdentity"] != null ? HttpContext.Current.Items["LoginIdentity"].ToString() : strUserName;
                cacheKey = strUserName;
                if (string.IsNullOrEmpty(cacheKey))
                {
                    return null;
                }
                if (!UserCache.Instance.TryGetValue(cacheKey, out result))
                {
                    #region
                    if (GetUser != null)
                    {
                        result = GetUser(cacheKey);
                        if (result == null)
                        {
                            //throw new Exception("该用户无权访问系统，请联系系统管理员。");
                            return null;
                        }
                        UserCache.Instance.Add(cacheKey, result);
                    }
                    #endregion
                }
            }
            return result;
        }

        private string GetMemberIdSSoCookies()
        {
            //return HttpContext.Current.Request.Cookies["AuthUser_LoginId"].Value;
            return HttpContext.Current.Items["LoginIdentity"].ToString();
        }
        /// <summary>
        /// 获得当前的登陆用户信息,如果当前登录用户失效， 则抛出异常
        /// </summary>
        /// <returns></returns>
        public static Framework.Web.Security.Authentication.LoginUserInfo GetCurrentUser() { return GetCurrentUser(true); }

        /// <summary>
        /// 中文显示名称
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLoginUserName() { return GetCurrentUser().CNName; }
        /// <summary>
        /// 登陆账号
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLoginUser() { return GetCurrentUser().LoginName; }

        /// 获得当前的登陆用户信息
        /// </summary>
        /// <param name="throwExceptionWhenNull">当true时，如果当前登录用户失效， 则抛出异常</param>
        /// <returns></returns>
        /// <exception cref="LoginUserNullException"></exception>
        public static Framework.Web.Security.Authentication.LoginUserInfo GetCurrentUser(bool Redeict = true)
        {
            Framework.Web.Security.Authentication.LoginUserInfo result = WebHelper.Instance.GetLoginUserInfo();
            if (result != null)
            {
                LogHelper.Instance.Info(string.Format("当前登录用户姓名：{0}", result.CNName));
            }
            //if (result == null && Redeict)
            //{
            //    HttpContext context = HttpContext.Current;
            //    context.Response.Redirect(string.Format("{0}?returnurl={1}", System.Web.Security.FormsAuthentication.LoginUrl, context.Request.RawUrl));
            //}
            return result;
        }

        public static bool IsAuthenticated
        {
            get
            {
                Framework.Web.Security.Authentication.LoginUserInfo result = WebHelper.Instance.GetLoginUserInfo();
                return result != null;
            }
        }

        public static string MapPath(string virtualPath)
        {
            return HostingEnvironment.MapPath(virtualPath);
        }


        private const string TimeSessionKey = "DateTimeOffset";
        public static DateTime DateTimeNow
        {
            get
            {
                if (AppSettingConfig.GetSetting("UseMockDate", "false").ToLower() != "true")
                {
                    return DateTime.Now;
                }
                else if (HttpContext.Current == null || HttpContext.Current.Session == null)
                {
                    return DateTime.Now;
                }
                else if (HttpContext.Current.Session[TimeSessionKey] != null)
                {
                    TimeSpan offset = (TimeSpan)HttpContext.Current.Session[TimeSessionKey];
                    return DateTime.Now.Add(offset);
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        public static DateTime GetTimeNow()
        {
            return DateTimeNow;
        }
        public static string ConvertPlainTextForShow(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            string textToFormat = plainText;
            textToFormat = textToFormat.Replace("\n", "<br/>");
            if (keepWhiteSpace)
            {
                textToFormat = textToFormat.Replace(" ", "&nbsp;");
            }

            return textToFormat;
        }

        public static string ConvertPlainTextForEdit(string plainText, bool keepWhiteSpace)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return plainText;
            }

            string textToFormat = plainText;
            textToFormat = textToFormat.Replace("<br/>", "\n");
            if (keepWhiteSpace)
            {
                textToFormat = textToFormat.Replace("&nbsp;", " ");
            }

            return HttpUtility.HtmlDecode(textToFormat);
        }

        public static void ResetAppTime(DateTime newDate)
        {

            if (AppSettingConfig.GetSetting("UseMockDate", "false").ToLower() == "true")
            {

                DateTime newAppTime = newDate.Add(DateTime.Now.Subtract(DateTime.Now.Date));

                TimeSpan offset = newAppTime.Subtract(DateTime.Now);
                HttpContext.Current.Session[TimeSessionKey] = offset;
            }
        }
    }
    public class UserCache : CacheQueue<String, Framework.Web.Security.Authentication.LoginUserInfo>
    {
        public static readonly UserCache Instance = CacheManager.GetInstance<UserCache>();

        private UserCache()
        {
        }
    }
}
