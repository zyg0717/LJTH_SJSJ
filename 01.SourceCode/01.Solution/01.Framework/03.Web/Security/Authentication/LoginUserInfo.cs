﻿using Framework.Core.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Framework.Web.Security.Authentication
{
    public class LoginUserInfo
    {
        public bool IsNormalEmployeeStatus
        {
            get
            {
                var normalEmployeeStatusList = ConfigurationManager.AppSettings["NormalEmployeeStatus"].Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();
                return normalEmployeeStatusList.Any(x => x == EmployeeStatus);
            }
        }


        public int EmployeeStatus { get; set; }
        public string UnitFullPath { get; set; }
        public string EmployeeID { get; set; }

        public string EmployeeCode { get; set; }

        public string LoginName { get; set; }

        public string CNName { get; set; }

        public string UnitName { get; set; }

        public int UnitID { get; set; }

        public string ActualUnitName { get; set; }

        public int actualUnitID { get; set; }

        public string OrgName { get; set; }

        public int OrgID { get; set; }
        public string jobName { get; set; }

    }

    public class LoginUserInfoCache : CacheQueue<String, LoginUserInfo>
    {
        public static readonly LoginUserInfoCache Instance = CacheManager.GetInstance<LoginUserInfoCache>();

        private LoginUserInfoCache()
        {
        }
    }
}
