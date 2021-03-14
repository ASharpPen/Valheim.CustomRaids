using System;
using System.Collections.Generic;
using Valheim.CustomRaids.ConfigurationTypes;

namespace Valheim.CustomRaids.Multiplayer
{
    [Serializable]
    internal class ConfigurationPackage
    {
        public GeneralConfiguration GeneralConfig;

        public List<RaidEventConfiguration> RaidConfigs;

        public ConfigurationPackage(){ }

        public ConfigurationPackage(
            GeneralConfiguration generalConfig,
            List<RaidEventConfiguration> raidConfigs)
        {
            GeneralConfig = generalConfig;
            RaidConfigs = raidConfigs;
        }
    }
}
