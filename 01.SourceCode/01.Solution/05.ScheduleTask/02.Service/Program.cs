using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Framework.Web.Security.Authentication;
using Framework.Web.Utility;

namespace ScheduleTask.TaskService
{
    class Program
    {
        static void Main(string[] args)
        {
            WebHelper.Instance.GetUser += VirtualUser;
#if DEBUG
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
