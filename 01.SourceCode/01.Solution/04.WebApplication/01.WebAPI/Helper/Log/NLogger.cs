using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Tracing;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace WebApplication.WebAPI.Helper.Log
{
    /// <summary>
    /// 日志处理类
    /// </summary>
    public class NLogTraceHelper
    {
        private static Logger _Instance;
        /// <summary>
        /// 日志实例
        /// </summary>
        public static Logger Instace
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = LogManager.GetCurrentClassLogger();
                }
                return _Instance;
            }
        }
    }

    /// <summary>
    /// NLogger实例
    /// </summary>
    public sealed class NLogger : ITraceWriter
    {
        private static readonly Logger classLogger = LogManager.GetCurrentClassLogger();

        private static readonly Lazy<Dictionary<TraceLevel, Action<string>>> loggingMap =
            new Lazy<Dictionary<TraceLevel, Action<string>>>(() => new Dictionary<TraceLevel, Action<string>>
                {
                    {TraceLevel.Info, classLogger.Info},
                    {TraceLevel.Debug, classLogger.Debug},
                    {TraceLevel.Error, classLogger.Error},
                    {TraceLevel.Fatal, classLogger.Fatal},
                    {TraceLevel.Warn, classLogger.Warn}
                });

        private Dictionary<TraceLevel, Action<string>> Logger
        {
            get { return loggingMap.Value; }
        }
        /// <summary>
        /// 日志跟踪
        /// </summary>
        /// <param name="request"></param>
        /// <param name="category"></param>
        /// <param name="level"></param>
        /// <param name="traceAction"></param>
        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level != TraceLevel.Off)
            {
                var record = new TraceRecord(request, category, level);
                traceAction(record);
                Log(record);
            }
        }
        private void Log(TraceRecord record)
        {
            var message = new StringBuilder();

            if (record.Request != null)
            {
                if (record.Request.Method != null)
                    message.Append(record.Request.Method);

                if (record.Request.RequestUri != null)
                    message.Append("\n").Append(record.Request.RequestUri.PathAndQuery);
            }

            if (!string.IsNullOrWhiteSpace(record.Category))
                message.Append("\n").Append(record.Category);

            //if (!string.IsNullOrWhiteSpace(record.Operator))
            //    message.Append(" ").Append(record.Operator).Append(" ").Append(record.Operation);

            if (!string.IsNullOrWhiteSpace(record.Message))
                message.Append("\n").Append(record.Message);

            if (record.Exception != null && !string.IsNullOrWhiteSpace(record.Exception.GetBaseException().Message))
            {
                message.Append("\n").Append(record.Exception.GetBaseException().Message);
                message.Append("\n").Append(record.Exception.GetBaseException().StackTrace);
            }
            Logger[record.Level](message.ToString());
        }
    }
}