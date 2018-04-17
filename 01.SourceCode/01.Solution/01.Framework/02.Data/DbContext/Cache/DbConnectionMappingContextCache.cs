using System;
using Framework.Core.Cache;

namespace Framework.Data
{
    internal class DbConnectionMappingContextCache : ContextCacheQueueBase<string, DbConnectionMappingContext>
    {
        public static DbConnectionMappingContextCache Instance
        {
            get
            {
                return ContextCacheManager.GetInstance<DbConnectionMappingContextCache>();
            }
        }
    }
}
