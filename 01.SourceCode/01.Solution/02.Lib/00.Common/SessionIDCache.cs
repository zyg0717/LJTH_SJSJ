using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Core.Cache;
namespace Lib.Common
{
    public class SessionIDCache : CacheQueue<string,string>
    {
        public static readonly SessionIDCache Instance = CacheManager.GetInstance<SessionIDCache>();

        private SessionIDCache()
        {
        }
    }
}
