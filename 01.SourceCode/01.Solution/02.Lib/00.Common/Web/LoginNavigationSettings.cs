using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Framework.Core.Config;
namespace Wanda.RCSJSJ.Common.Web
{
    public class LoginNavigationSettings : ConfigurationSection
    {
         /// <summary>
        /// 获取配置文件中的单点登录信息
        /// </summary>
        /// <returns></returns>
        public static LoginNavigationSettings GetConfig()
        {
            LoginNavigationSettings section =
                (LoginNavigationSettings)ConfigurationBroker.GetSection("loginNavigationSettings");

            ConfigurationExceptionHelper.CheckSectionNotNull(section, "loginNavigationSettings");
            return section;
        }


        private LoginNavigationSettings()
		{
		}

        [ConfigurationProperty("items", IsRequired = true)]
        public SSOConfigurationElementCollection Items
		{
			get
			{
                object obj = this["items"];
                return (SSOConfigurationElementCollection)this["items"];
			}
		}
    }
    [ConfigurationCollection(typeof(NavigationConfigurationElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class NavigationConfigurationElementCollection : NamedConfigurationElementCollection<NavigationConfigurationElement>
    {

    }
    public class NavigationConfigurationElement : NamedConfigurationElement
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
