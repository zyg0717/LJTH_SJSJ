using System;
using Framework.Core.Cache;

namespace Framework.Data
{
    internal sealed class ORMappingsCache : CacheQueue<System.Type, ORMappingItemCollection>
    {
        public static readonly ORMappingsCache Instance = CacheManager.GetInstance<ORMappingsCache>();

        private ORMappingsCache()
        {
        }

        internal static object syncRoot = new object();
    }
}
