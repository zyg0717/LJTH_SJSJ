using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.LayoutRenderers;
using System.Web;
using Newtonsoft.Json;
using System.Collections;

namespace Lib.Common
{
    /// <summary>
    /// 日志工具
    /// 使用nlog
    /// </summary>
    public class NLogHelper
    {
        /// <summary>
        /// Nlog日志对象实例
        /// </summary>
        public static NLogHelper Log = new NLogHelper("Portal");
        public const string WorkflowApp_Const = "WorkflowApp";
        public const string BizAppCode_Const = "BizAppCode";
        public const string BusinessID_Const = "BusinessID";
        public const string MethodName_Const = "MethodName";

        private Logger logger = null;
        private string WorkflowApp = String.Empty;

        public NLogHelper(string workflowapp)
        {
            logger = LogManager.GetCurrentClassLogger();
            WorkflowApp = workflowapp;
        }

        private void WriteLog(Exception ex, LogLevel level, string message, params object[] args)
        {
            LogEventInfo ei = new LogEventInfo();
            ei.Properties[WorkflowApp_Const] = WorkflowApp;
            ei.Properties[BusinessID_Const] = string.Empty;
            ei.Properties[MethodName_Const] = string.Empty;
            ei.Properties[BizAppCode_Const] = string.Empty;
            ei.Level = level;
            //生成message内容
            Dictionary<string, object> dict = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(message))
                dict.Add("Message", message);
            foreach (object obj in args)
            {
                if (obj != null)
                {
                    if (obj is LogObj)
                    {
                        LogObj o = (LogObj)obj;
                        dict.Add(o.ObjName, o.Obj);
                    }
                    else
                        dict.Add(obj.GetType().Name, obj);
                }
            }
            if (!dict.ContainsKey("ClientIP"))
                dict.Add("ClientIP", GetClientIPAddress());
            if (!dict.ContainsKey("BrowserInfo"))
                dict.Add("BrowserInfo", GetBrowserType());

            if ((string.IsNullOrEmpty(message) && dict.Count > 0) || (!string.IsNullOrEmpty(message) && dict.Count > 1))
            {
                object outStr = null; ;
                if (dict.TryGetValue(BusinessID_Const, out outStr) && outStr is string)
                    ei.Properties[BusinessID_Const] = (string)outStr;
                outStr = null;
                if (dict.TryGetValue(MethodName_Const, out outStr) && outStr is string)
                    ei.Properties[MethodName_Const] = (string)outStr;
                outStr = null;
                if (dict.TryGetValue(BizAppCode_Const, out outStr) && outStr is string)
                    ei.Properties[BizAppCode_Const] = (string)outStr;
                ei.Message = JsonConvert.SerializeObject(dict);
            }
            else
                ei.Message = message;
            ei.Exception = ex;
            logger.Log(ei);
        }

        public static LogObj MakeLogObj(string objName, object obj)
        {
            LogObj result = new LogObj();
            result.ObjName = objName;
            result.Obj = obj;
            return result;
        }

        public void Trace(string message, params object[] args)
        {
            Trace(null, message, args);
        }

        public void Trace(Exception ex, string message, params object[] args)
        {
            WriteLog(ex, LogLevel.Trace, message, args);
        }

        public void Debug(string message, params object[] args)
        {
            Debug(null, message, args);
        }

        public void Debug(Exception ex, string message, params object[] args)
        {
            WriteLog(ex, LogLevel.Debug, message, args);
        }

        public void Info(string message, params object[] args)
        {
            Info(null, message, args);
        }

        public void Info(Exception ex, string message, params object[] args)
        {
            WriteLog(ex, LogLevel.Info, message, args);
        }

        public void Warn(string message, params object[] args)
        {
            Warn(null, message, args);
        }

        public void Warn(Exception ex, string message, params object[] args)
        {
            WriteLog(ex, LogLevel.Warn, message, args);
        }

        public void Error(string message, params object[] args)
        {
            Error(null, message, args);
        }

        public void Error(Exception ex, string message, params object[] args)
        {
            WriteLog(ex, LogLevel.Error, message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            Fatal(null, message, args);
        }

        public void Fatal(Exception ex, string message, params object[] args)
        {
            WriteLog(ex, LogLevel.Fatal, message, args);
        }
        /// <summary>
        /// 获取当前IP
        /// </summary>
        protected string GetClientIPAddress()
        {
            try
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                else
                {
                    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
            }
            catch { return "unknown"; }
        }


        /// <summary>
        /// 获取浏览了类型
        /// </summary>
        /// <returns></returns>
        protected string GetBrowserType()
        {
            try
            {
                HttpBrowserCapabilities hbc = HttpContext.Current.Request.Browser;
                return hbc.Browser.ToString();     //获取浏览器类型

            }
            catch { return "unknown"; }
        }
    }

    public class LogObj
    {
        public string ObjName;
        public object Obj;
    }

    /// <summary>
    /// 应用系统
    /// </summary>
    [LayoutRenderer("workflowapp")]
    public class BizAppLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Properties[NLogHelper.WorkflowApp_Const].ToString());
        }
    }

    /// <summary>
    /// 系统环境Context
    /// </summary>
    [LayoutRenderer("context")]
    public class ContextRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (HttpContext.Current != null)
            {
                //todo
                builder.Append("Web Mode");
                //builder.Append(HttpContext.Current.Request.UserHostAddress);
                //builder.Append(HttpContext.Current.Request.UserAgent);
            }
            else
            {
                builder.Append("Console Mode");
            }
        }
    }

    [LayoutRenderer("exception")]
    public class ExceptionRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            if (logEvent.Exception == null)
                builder.Append("");
            else
                builder.Append(logEvent.Exception.ToString());
        }
    }

    [LayoutRenderer("businessid")]
    public class BusinessIDLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Properties[NLogHelper.BusinessID_Const].ToString());
        }
    }

    [LayoutRenderer("bizappcode")]
    public class BizAppCodeLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Properties[NLogHelper.BizAppCode_Const].ToString());
        }
    }

    [LayoutRenderer("methodname")]
    public class MethodNameLayoutRenderer : LayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            builder.Append(logEvent.Properties[NLogHelper.MethodName_Const].ToString());
        }
    }

}
