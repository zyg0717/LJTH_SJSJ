using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Common
{
    public static class ConstSet
    {
        //public static string DataPrivilegeGroupKey
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["DataPrivilegeGroupKey"];
        //    }
        //}
        //public static string AdminRoleCode
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["AdminRoleCode"];
        //    }
        //}

        //public static string AuditRoleCode
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["AuditRoleCode"];
        //    }
        //}


        public static string OWAServerUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["owa.serverurl"];
            }
        }
        public static string AdminUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminUserName"];
            }
        }

        public static string NormalEmployeeStatus
        {
            get
            {
                return ConfigurationManager.AppSettings["NormalEmployeeStatus"];
            }
        }
        public static string CtxNotifyContentFormat
        {
            get
            {
                return ConfigurationManager.AppSettings["CtxNotifyContentFormat"];
            }
        }
        public static string SiteBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["SiteBaseUrl"];
            }
        }
        //public static string AdminRoleID
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["AdminRoleID"];
        //    }
        //}

        //public static string AuditRoleID
        //{
        //    get
        //    {
        //        return ConfigurationManager.AppSettings["AuditRoleID"];
        //    }
        //}
        public static string TemplateBasePath
        {
            get
            {
                return ConfigurationManager.AppSettings["TemplateBasePath"];
            }
        }

        public static string ReportBasePath
        {
            get
            {
                return ConfigurationManager.AppSettings["ReportBasePath"];
            }
        }

        public static string AttachtBasePath
        {
            get
            {
                return ConfigurationManager.AppSettings["AttachtBasePath"];
            }
        }
    }
}
