using System;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Network;

namespace Valheim.CustomRaids.Configuration.Multiplayer
{
    [Serializable]
    internal class ConfigPackage : CompressedPackage
    {
        public GeneralConfiguration GeneralConfig;
        public RaidEventConfigurationFile RaidConfig;

        protected override void BeforePack()
        {
            GeneralConfig = ConfigurationManager.GeneralConfig;
            RaidConfig = ConfigurationManager.RaidConfig;

            Log.LogDebug($"Packaged general configuration.");
            Log.LogDebug($"Packaged raid configurations: {RaidConfig?.Subsections?.Count ?? 0}");
        }

        protected override void AfterUnpack(object obj)
        {
            if (obj is ConfigPackage configPackage)
            {
                Log.LogDebug("Received and deserialized config package");

                ConfigurationManager.GeneralConfig = configPackage.GeneralConfig;
                ConfigurationManager.RaidConfig = configPackage.RaidConfig;

                Log.LogDebug($"Unpacked general configuration.");
                Log.LogDebug($"Unpacked raid configurations: {ConfigurationManager.RaidConfig?.Subsections?.Count ?? 0}");

                Log.LogInfo("Successfully unpacked configs.");
            }
            else
            {
                Log.LogWarning("Received bad config package. Unable to load.");
            }
        }
    }
}
