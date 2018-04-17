using System;
using System.Globalization;
using Framework.Core.Cache;

namespace Framework.Core.Globalization
{
    internal sealed class CultureInfoContextCache : ContextCacheQueueBase<string, CultureInfo>
    {
        public static CultureInfoContextCache Instance
        {
            get
            {
                return ContextCacheManager.GetInstance<CultureInfoContextCache>();
            }
        }

        private CultureInfoContextCache()
        {
        }
    }
}
