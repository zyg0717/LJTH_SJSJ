using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Framework.Core.Config;
namespace Wanda.RCSJSJ.Common.Web
{
    public class SSOConfigSettings : ConfigurationSection
    {
         /// <summary>
        /// 获取配置文件中的单点登录信息
        /// </summary>
        /// <returns></returns>
        public static SSOConfigSettings GetConfig()
        {
            SSOConfigSettings section =
                (SSOConfigSettings)ConfigurationBroker.GetSection("ssOConfigSettings");

            ConfigurationExceptionHelper.CheckSectionNotNull(section, "ssOConfigSettings");
            return section;
        }


        private SSOConfigSettings()
		{
		}

        [ConfigurationProperty("informations", IsRequired = true)]
        public SSOConfigurationElementCollection Informations
		{
			get
			{
                object obj = this["informations"];
                return (SSOConfigurationElementCollection)this["informations"];
			}
		}
    }
    [ConfigurationCollection(typeof(SSOConfigurationElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class SSOConfigurationElementCollection : NamedConfigurationElementCollection<SSOConfigurationElement>
    {

    }
    public class SSOConfigurationElement : NamedConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public override string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("value", IsKey = true, IsRequired = true)]
        public string Value
        {
            get
            {
                return (string)this["value"];
            }
        }
    }
}
