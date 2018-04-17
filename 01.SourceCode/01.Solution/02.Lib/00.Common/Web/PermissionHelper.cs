using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Wanda.RCSJSJ.Common
{
    public static class PermissionHelper
    {
        public static void GetPermission()
        {
            if (EnablePermission)
                //未获取，或者不是同一个人，或者不是同一个应用，则从权限中心获取
                if (Wanda.Platform.Permission.ClientComponent.PermissionCenterProxy.GetPCDataStatus() == Wanda.Platform.Permission.ClientComponent.PermissionCenterDataStatus.NotReady
                    || GetCurrentUser != PermissionCenterProxy.GetUserLoginName()
                    || PCClientIdentity.CurrentApplicationCode != PermissionCenterProxy.GetApplicationCode()
                    )
                {
                    try
                    {
                        PermissionCenterProxy.GetPermission();
                        NLogHelper.Log.Info(string.Format("{0} {1} GetPermission", PermissionCenterProxy.GetUserLoginName(), DateTime.Now.ToString()),
        NLogHelper.MakeLogObj(NLogHelper.BusinessID_Const, PermissionCenterProxy.GetUserLoginName()),
        NLogHelper.MakeLogObj(NLogHelper.MethodName_Const, "GetPermission"),
        NLogHelper.MakeLogObj(NLogHelper.BizAppCode_Const, "ZCPT")
        );
                    }
                    catch (Exception e)
                    {
                        NLogHelper.Log.Error(e.ToString(),
                            NLogHelper.MakeLogObj(NLogHelper.BusinessID_Const, PermissionCenterProxy.GetUserLoginName()),
                            NLogHelper.MakeLogObj(NLogHelper.MethodName_Const, "GetPermission"),
                            NLogHelper.MakeLogObj(NLogHelper.BizAppCode_Const, "ZCPT")
                            );
                    }
                }
        }

        public static List<string> GetStartProcessList()
        {
            //如果要搬走请把GetPermission（） 方法也拷走
            if (EnablePermission)
                return PermissionCenterProxy.GetUserProperties().Where(p => p.PropertyCode == "SystemName").Select(p => p.PropertyValue).ToList();
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 用于获取操作日志
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMonthReportLogList()
        {
            //如果要搬走请把GetPermission（） 方法也拷走
            if (EnablePermission)
                return PermissionCenterProxy.GetUserProperties().Where(p => p.PropertyCode == "MonthReportLog").Select(p => p.PropertyValue).ToList();
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 用于获取操作日志
        /// </summary>
        /// <returns></returns>
        public static List<string> Getsubmanage()
        {
            //如果要搬走请把GetPermission（） 方法也拷走
            if (EnablePermission)
                return PermissionCenterProxy.GetUserProperties().Where(p => p.PropertyCode == "submanage").Select(p => p.PropertyValue).ToList();
            else
            {
                return null;
            }
        }

        public static List<FunctionalAuthorityResult> GetFuncPermission()
        {
            if (EnablePermission)
                return PermissionCenterProxy.GetFuncPermission();
            else
            {
                return null;
            }
        }

        public static bool EnablePermission
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["EnablePermission"] == "true";
            }
        }



        /// <summary>
        /// 获取人(请使用这处代码)
        /// </summary>
        /// <returns></returns>
        internal static string GetCurrentUser
        {
            get { return Wanda.Workflow.Client.SDKHelper.GetUserName(HttpContext.Current); }
        }
    }
}
