using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Framework.Core;
using System.Web;
using System.Reflection;

namespace Framework.Core.Log
{
    public class LogHelper
    {
        static LogHelper _instance;
        public static LogHelper Instance
        {
            get
            {

                if (_instance == null)
                {
                    _instance = new LogHelper();
                    _instance._formatterName = DateTime.Now.ToString("yyyyMMdd");
                    _instance.infor.Listeners.Add(new FlatFileTraceListener(AppDomain.CurrentDomain.BaseDirectory + "\\log\\Information_" + _instance._formatterName + ".log", new TextLogFormatter("default", @"[{timestamp}]{title} : {message}")));
                    _instance.error.Listeners.Add(new FlatFileTraceListener(AppDomain.CurrentDomain.BaseDirectory + "\\log\\Error_" + _instance._formatterName + ".log", new TextLogFormatter("default", @"[{timestamp}]{title} : {message}")));
                }

                if (_instance._formatterName != DateTime.Now.ToString("yyyyMMdd"))
                {
                    _instance.infor.Listeners.Clear();
                    _instance.error.Listeners.Clear();
                    _instance._formatterName = DateTime.Now.ToString("yyyyMMdd");

                    _instance.infor.Listeners.Add(new FlatFileTraceListener(AppDomain.CurrentDomain.BaseDirectory + "\\log\\Information_" + _instance._formatterName + ".log", new TextLogFormatter("default", @"[{timestamp}]{title} : {message}")));
                    _instance.error.Listeners.Add(new FlatFileTraceListener(AppDomain.CurrentDomain.BaseDirectory + "\\log\\Error_" + _instance._formatterName + ".log", new TextLogFormatter("default", @"[{timestamp}]{title} : {message}")));
                }


                return _instance;
            }
        }
        Logger infor = LoggerFactory.Create("infor");
        Logger error = LoggerFactory.Create("Error");
        string _formatterName = null;

        public void InfoFlag(string flag, string message)
        {
            infor.Write(string.Format("标识：{0},信息：{1}", flag, message), null, LogPriority.Normal, 0, TraceEventType.Information, "Infomation");
        }
        public void Info(string message)
        {
            infor.Write(message, null, LogPriority.Normal, 0, TraceEventType.Information, "Infomation");
        }
        public void Info(string message, string sourceName)
        {
            infor.Write(message, sourceName, LogPriority.Normal, 0, TraceEventType.Information, sourceName);
        }
        public void Info(string message, string sourceName, LogPriority priority)
        {
            infor.Write(message, sourceName, priority, 0, TraceEventType.Information, sourceName);
        }
        public void Info(string message, string sourceName, string Title)
        {
            infor.Write(message, sourceName, LogPriority.Normal, 0, TraceEventType.Information, Title);
        }
        public void Info(string message, string sourceName, LogPriority priority, string Title)
        {
            infor.Write(message, sourceName, priority, 0, TraceEventType.Information, Title);
        }

        public void Write(string message, string sourceName, LogPriority priority, int eventId,
                                TraceEventType logEventType, string title)
        {
            infor.Write(message, sourceName, priority, eventId, logEventType, title);
        }
        public void Error(Exception exp)
        {
            error.Write(exp.ToString() + "\r\n StaskTrace:" + exp.StackTrace + "\r\n", null, LogPriority.Highest, 0, TraceEventType.Error, "Error");
        }
        public void Error(Exception exp, string sourceName)
        {
            error.Write(exp.ToString(), sourceName, LogPriority.Highest, 0, TraceEventType.Error, "Error");
        }
    }

    /// <summary>
    /// 日志处理类
    /// </summary>
    /// <remarks>
    /// 包含Filters、Listeners的日志处理类
    /// </remarks>
    public sealed class Logger : IDisposable
    {
        private string loggerName = string.Empty;
        private List<FormattedTraceListenerBase> listeners = null;
        private LogFilterPipeline filters = null;
        private bool enableLog = true;

        private ReaderWriterLock rwLock = new ReaderWriterLock();
        private const int DefaultLockTimeout = 3000;

        private Action<LogEntity> ProcessLogHandler = null;
        /// <summary>
        /// Logger的名称
        /// </summary>
        /// <remarks>
        /// Logger的名称，一般从配置文件读取
        /// </remarks>
        public string Name
        {
            get
            {
                return this.loggerName;
            }
            set
            {
                this.loggerName = value;
            }
        }

        /// <summary>
        /// 表明Logger是否可用
        /// </summary>
        /// <remarks>
        /// 设置该Logger是否可用的布尔值
        /// </remarks>
        public bool EnableLog
        {
            get
            {
                return this.enableLog;
            }
            set
            {
                this.enableLog = value;
            }
        }

        /// <summary>
        /// Listener集合
        /// </summary>
        /// <remarks>
        /// 从配置文件中读取创建对象；如果没有，则返回初始List&lt;FormattedTraceListenerBase&gt;对象
        /// </remarks>
        public List<FormattedTraceListenerBase> Listeners
        {
            get
            {
                if (string.IsNullOrEmpty(this.loggerName) == false && LoggingSection.GetConfig().Loggers[Name] != null)
                    this.listeners = LoggingSection.GetConfig().Loggers[Name].LogListeners;
                else
                {
                    if (this.listeners == null)
                    {
                        this.listeners = new List<FormattedTraceListenerBase>();
                    }
                }

                return this.listeners;
            }
        }

        /// <summary>
        /// Filter集合
        /// </summary>
        /// <remarks>
        /// 从配置文件中读取、创建对象；如果没有，则返回初始LogFilterPipeline对象
        /// </remarks>
        internal LogFilterPipeline FilterPipeline
        {
            get
            {
                if (string.IsNullOrEmpty(this.loggerName) == false && LoggingSection.GetConfig().Loggers != null && LoggingSection.GetConfig().Loggers[Name] != null)
                    this.filters = LoggingSection.GetConfig().Loggers[Name].LogFilters;
                else
                {
                    if (this.filters == null)
                        this.filters = new LogFilterPipeline();
                }

                return this.filters;
            }
        }

        internal Logger()
        {
            ProcessLogHandler = new Action<LogEntity>(this.ProcessLog);
        }

        //internal Logger(string loggerName, LogFilterPipeline filters, List<FormattedTraceListenerBase> listeners)
        //{
        //    ExceptionHelper.CheckStringIsNullOrEmpty(loggerName, "LoggerName不能为空");

        //    this._loggerName = loggerName;
        //    this._listeners = listeners;
        //    this._filters = filters;
        //}
        internal Logger(string loggerAliasName, bool enabled)
            : this()
        {
            ExceptionHelper.CheckStringIsNullOrEmpty(loggerAliasName, "LoggerName不能为空");

            this.loggerName = loggerAliasName;
            this.enableLog = enabled;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            foreach (FormattedTraceListenerBase listener in Listeners)
                listener.Dispose();
        }

        #region Process Log

        public string CurrentUserName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    // Wanda SSO 
                    string ssoUsername = HttpContext.Current.Items["WD_SSO_UserName"] != null ? HttpContext.Current.Items["WD_SSO_UserName"].ToString() : string.Empty;

                    string result = HttpContext.Current.User.Identity != null ? HttpContext.Current.User.Identity.Name : ssoUsername;
                    return result;
                }
                else
                {
                    return "anonymous";
                }
            }
        }

        public void LogOperation(params object[] parameterInputs)
        {
            StackFrame sf = new StackFrame(1);
            var method = sf.GetMethod();

            string methodName = method.Name;
            string parameters = string.Join("\r\n", method.GetParameters().Select(
                (p, i) =>
                    string.Format(" {0}({1})={2}", p.Name, p.ParameterType.Name, GetInput(parameterInputs, i)))
                    .ToArray());

            var genericArgs = method.GetGenericArguments();

            string msg = methodName + "\r\n" + parameters;

            if (HttpContext.Current != null)
            {
                string[] args = { "__1osInfo", "__2browserInfo" };
                foreach (var item in args)
                {
                    msg += "\r\n:" + item + ":" + HttpContext.Current.Items.GetValue(item, "Not registered");
                }

            }


            LogEntity log = new LogEntity(msg)
            {
                Source = "操作日志",
                Priority = LogPriority.Lowest,
                EventID = 0,
                LogEventType = TraceEventType.Information,
                CreatorName = this.CurrentUserName,
                Title = "操作日志"

            };

            Write(log);
        }


        private string GetInput(object[] parameterInputs, int i)
        {
            if (parameterInputs == null)
            {
                return "null";
            }
            if (i >= parameterInputs.Length)
            {
                return "null";
            }

            if (parameterInputs[i] == null)
            {
                return "null";
            }
            return parameterInputs[i].ToString();
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="log">待写的日志记录</param>
        /// <remarks>
        /// 写日志信息的方法
        /// <code source="..\Framework\TestProjects\DeluxeWorks.Library.Test\\Logging\LoggerTest.cs"
        /// lang="cs" region="Logger Write Test" tittle="写日志信息"></code>
        /// </remarks>
        public void Write(LogEntity log)
        {
            try
            {
                this.rwLock.AcquireReaderLock(Logger.DefaultLockTimeout);

                if (this.enableLog && this.FilterPipeline.IsMatch(log) && ProcessLogHandler != null)
                {
                    ProcessLogHandler.BeginInvoke(log, null, null);
                }
            }
            catch (LogException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new LogException("写日志信息时出错：" + ex.Message, ex);
            }
            finally
            {
                this.rwLock.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="priority">日志优先级</param>
        /// <param name="eventId">日志事件ID</param>
        /// <param name="logEventType">日志事件类型</param>
        /// <param name="title">日志标题</param>
        /// <remarks>
        /// 根据传递的参数，构建LogEntity对象，并写入媒介
        /// <code source="..\Framework\TestProjects\DeluxeWorks.Library.Test\\Logging\LoggerTest.cs"
        /// lang="cs" region="Logger Write Test" tittle="写日志信息"></code>
        /// </remarks>
        public void Write(string message, string sourceName, LogPriority priority, int eventId,
                                TraceEventType logEventType, string title)
        {
            LogEntity log = new LogEntity(message);
            log.Source = sourceName;
            log.Priority = priority;
            log.EventID = eventId;
            log.LogEventType = logEventType;
            log.Title = title;

            Write(log);
        }

        private void ProcessLog(LogEntity log)
        {
            //if (!ShouldTrace(log.LogEventType)) 
            //    return;

            TraceEventCache cache = new TraceEventCache();

            //bool isTransfer = logEntry.Severity == TraceEventType.Transfer && logEntry.RelatedActivityId != null;

            foreach (TraceListener listener in this.Listeners)
            {
                try
                {
                    if (false == listener.IsThreadSafe)
                    {
                        Monitor.Enter(listener);//Monitor.Enter(sychronRoot);
                    }

                    listener.TraceData(cache, log.Source, log.LogEventType, log.EventID, log);

                    listener.Flush();
                }
                catch (Exception ex)
                {
                    if (listener is FormattedEventLogTraceListener)
                    {
                        try
                        {
                            string msg = string.Format("{1}[{0:yyyy-MM-dd HH:mm:ss}] \n 错误堆栈为：{2}", DateTime.Now, ex.Message, ex.StackTrace);

                            EventLog.WriteEntry("Application", "写事件查看器异常：" + msg, EventLogEntryType.Warning);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                        throw;
                }
                finally
                {
                    if (false == listener.IsThreadSafe)
                    {
                        Monitor.Exit(listener); //Monitor.Exit(sychronRoot);
                    }
                }
            }
        }

        #endregion

    }
}
