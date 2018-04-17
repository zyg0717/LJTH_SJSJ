using System;
using System.Collections.Specialized;
using Framework.Core.Cache;

namespace Framework.Data.XORMapping
{
    public class XORMappingCache
    {
    }

    public class DataModelCache : CacheQueue<string, DataModel>
    {

    }

    public class DataPropertyCache : CacheQueue<string, DataProperty>
    {

    }
}
