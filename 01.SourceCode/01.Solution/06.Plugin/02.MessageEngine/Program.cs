using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;

namespace Plugin.MessageEngine
{
    class Program
    {
        static void Main(string[] args)
        {


            WebHelper.Instance.GetUser += VirtualUser;
#if DEBUG
            //try
            //{

            //    var json = "{\"FromSys\":\"WCF\",\"MsgType\":\"IM.Alert\",\"Sender\":\"zhengguilong\",\"SenderName\":\"郑桂 \",\"Target\":\"zhengguilong\",\"Flag\":\"0\",\"MsgId\":\"WCF201801271847460939\",\"MSTitle\":\"\",\"MSContent\":\"【测试OA待办集成v5】已完成收集，请到系统查看,http://sjsj.wanda-dev.cn/Tasks/TaskInfo.aspx?taskid=53aa17a3-8133-ec86-a7e0-e0f4a03d042d\",\"TargetTime\":\"2018-01-27 18:30:30\",\"Priority\":\"3\"}";
            //    var data = MessageEngineService.PostUrl("http://192.168.50.94:7007/", json);
            //    LogMgnt.Instance["*"].Info("提交成功,返回数据为:{0}", data);
            //}
            //catch (Exception ex)
            //{
            //    LogMgnt.Instance["*"].Error("提交失败{0},{1}", ex.Message, ex.StackTrace);
            //}

            DebugRun();
#else
            ServiceBase[] ServicesToRun = new ServiceBase[] { new ScheduleService() };
            ServiceBase.Run(ServicesToRun);
#endif


        }

        public static LoginUserInfo VirtualUser(string loginName)
        {
            return new LoginUserInfo()
            {
                CNName = loginName,
                LoginName = loginName
            };
        }

        private static void DebugRun()
        {
            MainEntry objMain = new MainEntry();
            objMain.Run();
        }
    }
}
