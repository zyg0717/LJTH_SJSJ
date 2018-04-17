using Framework.Core;
using Framework.Data;
using Framework.Core.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Framework.Data.AppBase;
using System.Diagnostics;

namespace Lib.Common
{
    public static class LogSqlAdapter
    {

        private static string InsertLogFormatString = @"
INSERT INTO  [APPLICATION_LOG]
           (
            [Source]
           ,[TraceEventType]
           ,[LogPriority]
           ,[EventID]
           ,[Title]
           ,[Message]
           ,[StackTrace]
           ,[MachineName]
           ,[ExtendedProperties]
           ,[CreatorName]
           ,[CreateTime])
     VALUES
           (  
                {0}
             '{1}'                   
           ,  {2}                    
           ,  {3}                    
           ,  {4}                    
           , '{5}'                   
           , '{6}'                   
           , '{7}'                   
           , '{8}'                   
           , '{9}'                   
           , '{10}'                  
           , '{11}'                  
           )
";

        //private static readonly string connectionString = "WandaKpiConnectionString";

        public static string AddLog(LogEntity log, string userName, string connectionString)
        {

            ExceptionHelper.TrueThrow(log == null, "LogEntity is null");
            ExceptionHelper.TrueThrow(string.IsNullOrEmpty(connectionString), "connectionString is empty");
            try
            {
                log.ActivityID = Guid.NewGuid();
                log.EventID = GetEventID(connectionString);
                string sql = string.Format(InsertLogFormatString,
                        "",//  log.ActivityID.ToString(),
                        SafeQuote(log.Source),
                         (int)log.LogEventType,
                         (int)log.Priority,
                        log.EventID,
                        SafeQuote(log.Title),
                        SafeQuote(log.Message),
                        SafeQuote(log.StackTrace),
                        SafeQuote(log.MachineName),
                        SafeQuote(GetJsonString(log.ExtendedProperties)),
                        SafeQuote(log.CreatorName),
                        log.TimeStamp);

                DbHelper.RunSql(sql, connectionString);
            }
            catch (Exception Exception)
            {

            }
            return log.ActivityID.ToString();

        }

        private static string SafeQuote(string data)
        {
            return TSqlBuilder.Instance.CheckQuotationMark(data, false);
        }
        private static int GetEventID(string connectionString)
        {
            string sql = "select top 1 eventID from      [APPLICATION_LOG] order by eventID desc";
            object currentMax = DbHelper.RunSqlReturnScalar(sql, connectionString);
            if (currentMax == null)
            {
                return 1;
            }

            return int.Parse(currentMax.ToString()) + 1;
        }

        /// <summary>
        /// 转换为json格式
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static string GetJsonString(IDictionary<string, object> dictionary)
        {

            if (dictionary == null)
            {
                return "{}";
            }
            StringBuilder result = new StringBuilder();

            result.Append("{ version:\"1.0\"");

            foreach (var item in dictionary)
            {
                result.AppendFormat(" ,{0}:\"{1}\"", item.Key, item.Value.ToString().Replace("\"", "&quot;")); //TODO， 待验证双引号转义
            }
            result.Append("}");
            return result.ToString();

        }

        public static PartlyCollection<LogEntity> GetLogsList(PagenationDataFilter filter, string connectionName)
        {
            WhereSqlClauseBuilder where = filter.ConvertToWhereBuilder();

            string sqlFormat = @"
With tmplLog as(
    SELECT row_number() over (order by CreateTime desc) AS ROW_INDEX, * 
    FROM  [APPLICATION_LOG] 
    WHERE 1=1 AND ({0})
) 
SELECT * FROM tmplLog
WHERE 
      ROW_INDEX BETWEEN {1} AND {2};

 
SELECT count (*) from APPLICATION_LOG WHERE 1=1 AND ({0})
";

            int rowIndex = filter.RowIndex;
            int pageSize = filter.PageSize;
            string whereStr= where.ToSqlString(TSqlBuilder.Instance);
            string sql = string.Format(sqlFormat,
                string.IsNullOrEmpty(whereStr) ? "1=1" : whereStr,
                                        rowIndex,
                                        rowIndex + pageSize);


            DataSet ds = DbHelper.RunSqlReturnDS(sql, connectionName);

            List<LogEntity> list = new List<LogEntity>();
            int count = 0;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string message = DataConverter.ChangeType<object, string>(row["Message"]);
                LogEntity item = new LogEntity(message);
                //item.ActivityID = DataConverter.ChangeType<object, Guid>(row["EventID"], typeof(Guid));
                item.EventID = DataConverter.ChangeType<object, int>(row["EventID"]);
                item.LogEventType = DataConverter.ChangeType<object, TraceEventType>(row["TraceEventType"]);
                item.MachineName = DataConverter.ChangeType<object, string>(row["MachineName"]);
                item.Priority = DataConverter.ChangeType<object, LogPriority>(row["LogPriority"]);
                item.Source = DataConverter.ChangeType<object, string>(row["Source"]);
                item.StackTrace = DataConverter.ChangeType<object, string>(row["StackTrace"]);
                item.TimeStamp = DataConverter.ChangeType<object, DateTime>(row["CreateTime"]);
                item.Title = DataConverter.ChangeType<object, string>(row["Title"]);
                item.CreatorName = DataConverter.ChangeType<object, string>(row["CreatorName"]);
                list.Add(item);
            }

            count = DataConverter.ChangeType<object, int>(ds.Tables[1].Rows[0][0]);

            PartlyCollection<LogEntity> result = PartlyCollection<LogEntity>.Create(list, count);
            return result;
        }
        public static PartlyCollection<LogEntity> GetLogsListII(PagenationDataFilter filter, string connectionName)
        {
            WhereSqlClauseBuilder where = filter.ConvertToWhereBuilder();

            string sqlFormat = @"With tmplLog as(
	                    SELECT row_number() over (order by CreateTime desc) AS ROW_INDEX, * FROM 
	                    (
		                    SELECT  * ,(Title + Source+ CONVERT( VARCHAR(MAX),[Message])) as KeyWord
		                    FROM  [APPLICATION_LOG]    
		                    )  AS T
	                    WHERE 1=1 AND ({0})
	                    )
            SELECT * FROM tmplLog
            WHERE 
              ROW_INDEX BETWEEN {1} AND {2} AND ({0}); 
             SELECT count (*) AS TOTALCOUNT FROM ( SELECT *,(Title + Source+ CONVERT( VARCHAR(MAX),[Message])) as KeyWord
               from [APPLICATION_LOG] 
               )T       
                 WHERE 1=1 AND ({0})";

            int rowIndex = filter.RowIndex;
            int pageSize = filter.PageSize;
            string whereStr = where.ToSqlString(TSqlBuilder.Instance);
            string sql = string.Format(sqlFormat,
                string.IsNullOrEmpty(whereStr) ? "1=1" : whereStr,
                                        rowIndex,
                                        rowIndex + pageSize);


            DataSet ds = DbHelper.RunSqlReturnDS(sql, connectionName);

            List<LogEntity> list = new List<LogEntity>();
            int count = 0;

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string message = DataConverter.ChangeType<object, string>(row["Message"]);
                LogEntity item = new LogEntity(message);
                //item.ActivityID = DataConverter.ChangeType<object, Guid>(row["EventID"], typeof(Guid));
                item.EventID = DataConverter.ChangeType<object, int>(row["EventID"]);
                item.LogEventType = DataConverter.ChangeType<object, TraceEventType>(row["TraceEventType"]);
                item.MachineName = DataConverter.ChangeType<object, string>(row["MachineName"]);
                item.Priority = DataConverter.ChangeType<object, LogPriority>(row["LogPriority"]);
                item.Source = DataConverter.ChangeType<object, string>(row["Source"]);
                item.StackTrace = DataConverter.ChangeType<object, string>(row["StackTrace"]);
                item.TimeStamp = DataConverter.ChangeType<object, DateTime>(row["CreateTime"]);
                item.Title = DataConverter.ChangeType<object, string>(row["Title"]);
                item.CreatorName = DataConverter.ChangeType<object, string>(row["CreatorName"]);
                list.Add(item);
            }

            count = DataConverter.ChangeType<object, int>(ds.Tables[1].Rows[0][0]);

            PartlyCollection<LogEntity> result = PartlyCollection<LogEntity>.Create(list, count);
            return result;
        }


        public static List<string> GetSources(string connectionName)
        {
            string sql = "select distinct [Source] from  [APPLICATION_LOG] ";
            DataSet ds = DbHelper.RunSqlReturnDS(sql, connectionName);

            return GetStringList(ds);
        }

        public static List<string> GetMachieNames(string connectionName)
        {
            string sql = "select distinct  machinename from  Application_Log ";
            DataSet ds = DbHelper.RunSqlReturnDS(sql, connectionName);

            return GetStringList(ds);
        }

        private static List<string> GetStringList(DataSet ds)
        {
            List<string> result = new List<string>();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                result.Add( DataConverter.ChangeType<object, string>(item[0]));
            }

            return result;
        }
    }


    [Serializable]
    public class LogEntityFilter : PagenationDataFilter
    {
        [FilterField("Title", "like")]
        public string Title { get; set; }
        [FilterField("LogPriority", DefaultV = -1)]
        public int Priority { get; set; }
        [FilterField("CreateTime", ">=")]
        public string CreateTime { get; set; }

        [FilterField("MachineName")]
        public string MachineName { get; set; }

        [FilterField("CreatorName","like")]
        public string CreatorName { get; set; }

        [FilterField("Message","like")]
        public string Message { get; set; }

        [FilterField("KeyWord", "like")]
        public string KeyWord { get; set; }
       
        [FilterField("StackTrace", "<>")]
        public string StackTrace { get; set; }
    }

}
