using BepInEx;
using BepInEx.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.ConfigurationCore;
using Valheim.CustomRaids.ConfigurationTypes;

namespace Valheim.CustomRaids
{
    public static class ConfigurationManager
    {
        public static bool DebugOn = false;

        public static GeneralConfiguration GeneralConfig;

        public static List<RaidEventConfiguration> RaidConfig;

        public static void LoadAllConfigurations()
        {
            LoadGeneralConfigurations();

            LoadRaidConfigurations();
        }

        public static void LoadRaidConfigurations()
        {
            string configPath = Path.Combine(Paths.ConfigPath, "custom_raid.cfg");

            Debug.Log($"Loading raid event configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);

            if (GeneralConfig != null) configFile.SaveOnConfigSet = GeneralConfig.StopTouchingMyConfigs.Value;

            if (DebugOn) Debug.Log("Scanning bindings...");
            ConfigurationLoader.ScanBindings<RaidEventConfiguration, SpawnConfiguration>(configFile, DebugOn);

            if (DebugOn) Debug.Log("Creating configuration...");
            Dictionary<string, RaidEventConfiguration> raidConfiguration = ConfigurationLoader.LoadArrayConfigurations<RaidEventConfiguration, SpawnConfiguration>(configFile, DebugOn);
            RaidConfig = raidConfiguration.Values.ToList();

            if (DebugOn) Debug.Log("Finished loading raid event configurations");
        }

        public static void LoadGeneralConfigurations()
        {
            string generalConfig = Path.Combine(Paths.ConfigPath, "custom_raid.general.cfg");

            Debug.Log($"Loading general configuration from {generalConfig}.");

            GeneralConfig = new GeneralConfiguration();
            GeneralConfig.LoadConfig(new ConfigFile(generalConfig, true));

            DebugOn = GeneralConfig.DebugOn.Value;
        }
    }
}
