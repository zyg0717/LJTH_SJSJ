using System;
using System.Configuration;
using Framework.Core.Config;

namespace Framework.Data.XORMapping
{
    public class XORMappingConifgSectionGroup : ConfigurationSectionGroup
    {
        public XORMappingConifgSectionGroup()
            : base()
        { }

        [ConfigurationProperty("mappingFileSettings")]
        public MappingFileSection MappingFiles
        {
            get
            {
                return base.Sections["mappingFileSettings"] as MappingFileSection;
            }
        }
    }

    public class MappingFileSection : ConfigurationSection
    {
        private static string sectionName = "mappingFileSettings";

        public static MappingFileSection GetConfig()
        {
            MappingFileSection section = (MappingFileSection)ConfigurationBroker.GetSection(sectionName);

            ConfigurationExceptionHelper.CheckSectionNotNull(section, sectionName);

            return section;
        }

        [ConfigurationProperty("baseDir")]
        public string BaseDir
        {
            get
            {
                if (this["baseDir"] == null || string.IsNullOrEmpty((string)this["baseDir"]))
                {
                    return AppDomain.CurrentDomain.BaseDirectory;
                }
                return (string)this["baseDir"];
            }
        }

        [ConfigurationProperty("fileLocations")]
        public MappingFileElementCollection FileLocations
        {
            get
            {
                return this["fileLocations"] as MappingFileElementCollection;
            }
        }
    }

    public class MappingFileElement : NamedConfigurationElement
    {
        [ConfigurationProperty("filename")]
        public string FileName
        {
            get
            {
                return (string)this["filename"];
            }
        }
    }

    public class MappingFileElementCollection : NamedConfigurationElementCollection<MappingFileElement>
    {
        
    }
}
