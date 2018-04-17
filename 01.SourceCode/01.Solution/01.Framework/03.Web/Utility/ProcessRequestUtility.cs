using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using Framework.Core;

namespace Framework.Web
{
    /// <summary>
    /// 将Request映射到相应的ProcessRequest方法
    /// </summary>
    internal static class ProcessRequestUtility
    {
        public static readonly string MethodKey = "Method";

        public static object ExecuteProcess(IHttpHandler handler)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(handler != null, "handler");

            MethodInfo mi = GetMethodInfoByCurrentUri(handler.GetType());

            ExceptionHelper.FalseThrow(mi != null, "不能处理请求，无法从当前的Request找到匹配的ProcessRequest方法");

            return mi.Invoke(handler, PrepareMethodParamValues(handler, mi));
        }

        private static MethodInfo GetMethodInfoByCurrentUri(Type handlerType)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(handlerType != null, "handlerType");

            HandlerInfo handlerInfo = GetHandlerMethods(handlerType); //Cache

            MethodInfo mi = GetMatchedMethodInfoByCurrentUri(handlerInfo.HandlerMethods);

            if (mi == null)
                mi = handlerInfo.DefaultMethod;

            return mi;
        }

        private static HandlerInfo GetHandlerMethods(Type handlerType)
        {
            MethodInfo[] mis = handlerType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
 
            List<MethodInfo> methodList = new List<MethodInfo>();
            MethodInfo defaultMethod = null;

            foreach (MethodInfo mi in mis)
            {
                ProcessRequestAttribute attr = AttributeHelper.GetCustomAttribute<ProcessRequestAttribute>(mi);
                if (attr != null)
                {
                    methodList.Add(mi);
                    if (defaultMethod == null || attr.Default)
                        defaultMethod = mi;
                }
            }
            return new HandlerInfo(methodList.ToArray(), defaultMethod);
        }

        private static MethodInfo GetMatchedMethodInfoByCurrentUri(MethodInfo[] mis)
        {
            HttpRequest request = HttpContext.Current.Request;

            bool containMethod  = false;
            string methodName = string.Empty;
            if (request.QueryString[MethodKey] != null)
            {
                methodName = request.QueryString[MethodKey];
                containMethod = string.IsNullOrEmpty(methodName) || string.IsNullOrWhiteSpace(methodName) ? false : true;
            }

            NameValueCollection queryParams = new NameValueCollection(request.QueryString);
            queryParams.Remove(MethodKey);

            MethodInfo result = null;
            int maxMatchLevel = 0;
            foreach (MethodInfo mi in mis)
            {
                int level = 0;
                if (containMethod)
                {
                    if (string.Compare(mi.Name, methodName, false) == 0)
                        level++;
                }

                level += GetParamsMatchedLevel(mi, queryParams);
                if (level > maxMatchLevel)
                {
                    maxMatchLevel = level;
                    result = mi;
                }
            }
            return result;
        }
        
        private static int GetParamsMatchedLevel(MethodInfo mi, NameValueCollection queryString)
        {
            int result = 0;
            ParameterInfo[] parameters = mi.GetParameters();

            foreach (ParameterInfo param in parameters)
            {
                if (queryString[param.Name] != null)
                    result++;
            }

            result -= Math.Abs(parameters.Length - result);	//方法参数的个数减去匹配程度，形成新的匹配度

            return result;
        }

        private static object[] PrepareMethodParamValues(IHttpHandler handler, MethodInfo mi)
        {
            ParameterInfo[] parameters = mi.GetParameters();
            object[] paramValues = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                string queryValue = HttpContext.Current.Request.QueryString[parameters[i].Name];

                if (string.IsNullOrEmpty(queryValue))
                    paramValues[i] = null;
                else
                    paramValues[i] = DataConverter.ChangeType(queryValue, parameters[i].ParameterType);
            }
            return paramValues;
        }
    }

    public class HandlerInfo
    { 
        private MethodInfo[] handlerMethods = null;
		private MethodInfo defaultMethod = null;

        public HandlerInfo(MethodInfo[] mis, MethodInfo defMethod)
		{
			this.handlerMethods = mis;
			this.defaultMethod = defMethod;
		}

		public MethodInfo[] HandlerMethods
		{
			get
			{
				return handlerMethods;
			}
		}

		public MethodInfo DefaultMethod
		{
			get
			{
				return defaultMethod;
			}
		}
    }
}
