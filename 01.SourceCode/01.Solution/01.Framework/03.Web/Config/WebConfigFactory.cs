using System;
using Framework.Core.Config;
using System.Web.Configuration;

namespace Framework.Web
{
    public class WebConfigFactory
    {
        /// <summary>
        /// 获取WebControlsSection
        /// </summary>
        /// <returns>WebControlsSection</returns>
        /// <remarks>获取WebControlsSection</remarks>
        public static Framework.Web.Config.WebControlsSection GetWebControlsSection()
        {
            Framework.Web.Config.WebControlsSection section = (Framework.Web.Config.WebControlsSection)ConfigurationBroker.GetSection("Framework.web/webcontrols");

            if (section == null)
                section = new Framework.Web.Config.WebControlsSection();

            return section;
        }

        /// <summary>
        /// 获取JsonSerializationSection
        /// </summary>
        /// <returns>JsonSerializationSection</returns>
        /// <remarks>获取JsonSerializationSection</remarks>
        public static ScriptingJsonSerializationSection GetJsonSerializationSection()
        {
            ScriptingJsonSerializationSection section = (ScriptingJsonSerializationSection)ConfigurationBroker.GetSection("Framework.web/jsonConverter");

            if (section == null)
                section = new ScriptingJsonSerializationSection();

            return section;
        }


        /// <summary>
        /// 获取HttpModulesSection
        /// </summary>
        /// <returns></returns>
        public static HttpModulesSection GetHttpModulesSection()
        {
            HttpModulesSection section = (HttpModulesSection)ConfigurationBroker.GetSection("Framework.web/httpModules");

            if (section == null)
                section = new HttpModulesSection();

            return section;
        }


        /// <summary>
        /// 获取ContentTypesSection
        /// </summary>
        /// <returns>ContentTypesSection</returns>
        /// <remarks>获取ContentTypesSection</remarks>
        public static ContentTypesSection GetContentTypesSection()
        {
            ContentTypesSection section = (ContentTypesSection)ConfigurationBroker.GetSection("Framework.web/contentTypes");

            if (section == null)
                section = new ContentTypesSection();

            return section;
        }


        /// <summary>
        /// 获取PageExtensionSection
        /// </summary>
        /// <returns></returns>
        public static PageContentSection GetPageExtensionSection()
        {
            PageContentSection section = (PageContentSection)ConfigurationBroker.GetSection("Framework.web/pageContent");

            if (section == null)
                section = new PageContentSection();

            return section;
        }

        /// <summary>
        /// 获取PageModulesSection
        /// </summary>
        /// <returns></returns>
        public static PageModulesSection GetPageModulesSection()
        {
            PageModulesSection section = (PageModulesSection)ConfigurationBroker.GetSection("Framework.web/pageModules");

            if (section == null)
                section = new PageModulesSection();

            return section;
        }
    }
}