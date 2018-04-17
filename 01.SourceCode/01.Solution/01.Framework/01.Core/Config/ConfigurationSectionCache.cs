using System.Configuration;
using Framework.Core.Cache;

namespace Framework.Core.Config
{
    /// <summary>
    /// 用于存放ConfigurationSection的Cache
    /// </summary>
    sealed class ConfigurationSectionCache : PortableCacheQueue<string, ConfigurationSection>
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        public static readonly ConfigurationSectionCache Instance = CacheManager.GetInstance<ConfigurationSectionCache>();

        private ConfigurationSectionCache()
        {
        }
    } // class end
}
