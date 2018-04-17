using System;
using System.Configuration;
using Framework.Core.Config;

namespace Framework.Data
{
    sealed class DataProviderConfigurationElement : TypeConfigurationElement
    {

    }

    [ConfigurationCollection(typeof(DataProviderConfigurationElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    sealed class DataProviderConfigurationElementCollection : NamedConfigurationElementCollection<DataProviderConfigurationElement>
    {

    }
}
