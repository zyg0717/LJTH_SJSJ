using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using System.Data;
using System.Net;
using System.IO;
using Lib.Model;
using Lib.BLL;
using System.Configuration;
using Framework.Web.Json;
using System.Net.Http;

namespace Plugin.MessageEngine
{
    /// <summary>
    /// 消息发送服务
    /// </summary>
    [Quartz.DisallowConcurrentExecution]
    [Quartz.PersistJobDataAfterExecution]
    class MessageEngineService : Quartz.IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            LogMgnt.Instance["MessageEngineService"].Info("------------消息发送服务开始-------------");
            try
            {
                var pendingList = TSM_MessagesOperator.Instance.LoadPendingList(20, Convert.ToInt32(ConfigurationManager.AppSettings["MaxTryCount"]));
                pendingList.ForEach(x =>
                {
                    bool status = false;
                    string errorMessage = "";
                    try
                    {
                        //转换实体
                        var json = ConvertToJson(x);
                        var postData = JsonHelper.Serialize(json);
                        LogMgnt.Instance["MessageEngineService"].Info("当前要提交的json为:{0}", postData);
                        //发送消息 调用接口
                        string url = ConfigurationManager.AppSettings["MessageService.Url"];
                        LogMgnt.Instance["MessageEngineService"].Info("当前提交url:{0}", url);
                        var reuslt = PostToServer(url, postData);
                        var data = JsonHelper.Deserialize<MessageEntityResult>(reuslt);
                        status = data.Result.Equals("ok", StringComparison.CurrentCultureIgnoreCase);
                        errorMessage = reuslt;
                    }
                    catch (Exception ex)
                    {
                        errorMessage += ex.Message;
                        errorMessage += "\r\n";
                        errorMessage += ex.StackTrace.ToString();
                        status = false;
                    }
                    x.Status = status ? 1 : -1;
                    x.TryTimes += status ? 0 : 1;
                    if (errorMessage.Length > 500)
                        errorMessage = errorMessage.Substring(0, 500);
                    x.ErrorInfo = errorMessage;
                    TSM_MessagesOperator.Instance.UpdateTSM_Messages(x);
                });
            }
            catch (Exception ex)
            {
                LogMgnt.Instance["MessageEngineService"].Error("任务执行异常，错误信息{0}，错误堆栈{1}", ex.Message, ex.StackTrace);
            }
            LogMgnt.Instance["MessageEngineService"].Info("------------消息发送服务完成-------------");
        }
        public class MessageEntityResult
        {
            public string FromSys { get; set; }
            public string Result { get; set; }
            public string MsgId { get; set; }
            public string SendTime { get; set; }
            public string ResultDesc { get; set; }
        }
        private static string ConvertMessageType(int messageType)
        {
            string returnValue = "";
            switch (messageType)
            {
                case 1:
                    returnValue = "IM.MSG";
                    break;
                case 2:
                    returnValue = "IM.Alert";
                    break;
                case 3:
                    returnValue = "IM.MSG";
                    break;
            }
            return returnValue;
        }
        public static MessageEntity ConvertToJson(TSM_Messages message)
        {
            var target = message.Target;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MessageService.DebugUser"]))
            {
                target = ConfigurationManager.AppSettings["MessageService.DebugUser"];
            }
            var senderList = ConfigurationManager.AppSettings["MessageService.Sender"];

            var sender = senderList.IndexOf(",") > 0 ? senderList.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries)[0] : message.Sender;
            var senderName = senderList.IndexOf(",") > 0 ? senderList.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries)[1] : message.SenderName;


            MessageEntity json = new MessageEntity();
            json.FromSys = ConfigurationManager.AppSettings["MessageService.FromSys"];
            json.MSContent = message.Content;
            json.MsgId = json.FromSys + DateTime.Now.ToString("yyyyMMddHHmmssffff");
            json.MsgType = ConvertMessageType(message.MessageType);
            json.Priority = message.Priority.ToString();
            json.Sender = sender;
            json.SenderName = senderName;
            json.Targets = target;
            json.MSTitle = message.Title;
            json.TargetTime = message.TargetTime.ToString("yyyy-MM-dd HH:mm:ss");
            json.Flag = "0";
            var prefix = ConfigurationManager.AppSettings["MessageService.Prefix"];
            if (!string.IsNullOrEmpty(prefix))
            {
                json.MSTitle = prefix + json.MSTitle;
            }
            return json;
        }
        public class MessageEntity
        {
            public string FromSys { get; set; }
            public string MsgType { get; set; }
            public string Sender { get; set; }
            public string SenderName { get; set; }
            public string Targets { get; set; }
            public string Flag { get; set; }
            public string MsgId { get; set; }
            public string MSTitle { get; set; }
            public string MSContent { get; set; }
            public string TargetTime { get; set; }
            public string Priority { get; set; }
        }
        /// <summary>
        /// post数据到消息服务接口中
        /// </summary>
        /// <param name="url">消息服务地址 目前为:http://192.168.50.94:7007</param>
        /// <param name="param">post提交的json参数  示例数据:{"FromSys":"WCF","MsgType":"IM.Alert","Sender":"zhengguilong","SenderName":"郑桂 ","Target":"zhengguilong","Flag":"0","MsgId":"WCF201801280559300052","MSTitle":"数据收集任务完成通知","MSContent":"【测试新增任务】已完成收集，请到系统查看,http://192.168.50.72/Tasks/TaskInfo.aspx?taskid=cbfc8758-f133-ec8a-1346-552c2915a3d0","TargetTime":"2018-01-28 05:59:25","Priority":"3"}</param>
        /// <returns></returns>
        public static string PostToServer(string url, string param)
        {
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 10 * 1000;
            request.Method = "POST";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(param);
            //*******************这几行很关键
            request.ContentLength = bytes.Length;
            request.KeepAlive = false;
            request.ServicePoint.Expect100Continue = false;
            //*******************
            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();
            System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream stream = response.GetResponseStream();
            string line = "";
            string result = "";
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            while ((line = reader.ReadLine()) != null)
            {
                result += line + "\r\n";
            }
            return result;
        }
    }
}
