using System;
using System.Collections.Generic;
using Framework.Core.Cache;

namespace Framework.Web.MVC
{
    internal class ControllerInfoCache : CacheQueue<System.Type, ControllerInfo>
    {
        public static readonly ControllerInfoCache Instance = CacheManager.GetInstance<ControllerInfoCache>();

        private ControllerInfoCache()
        {
        }
    }
}
