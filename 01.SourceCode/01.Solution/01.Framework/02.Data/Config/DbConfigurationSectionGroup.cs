﻿using System.Configuration;

namespace Framework.Data
{
    sealed class DbConfigurationSectionGroup : ConfigurationSectionGroup
    {
        public DbConfigurationSectionGroup()
            : base()
        {
        }

        [ConfigurationProperty("connectionManager")]
        public ConnectionManagerConfigurationSection ConnectionManager
        {
            get
            {
                return base.Sections["connectionManager"] as ConnectionManagerConfigurationSection;
            }
        }

    }
}
