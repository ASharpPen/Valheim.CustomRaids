using BepInEx;
using BepInEx.Configuration;
using System;
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
        private static string DefaultRaidFile = "custom_raids.raids.cfg";
        private static string DefaultConfigFile = "custom_raids.cfg";

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

            string configPath = Path.Combine(Paths.ConfigPath, DefaultRaidFile);

            var configs = LoadRaidConfigFile(configPath);

            if (GeneralConfig.LoadSupplementalRaids.Value)
            {
                if (DebugOn) Debug.Log("Loading supplemental raids...");

                configs.AddRange(LoadSupplemental());
            }

            RaidConfig = configs;

            if (DebugOn) Debug.Log("Finished loading raid event configurations");
        }

        public static void LoadGeneralConfigurations()
        {
            string generalConfig = Path.Combine(Paths.ConfigPath, DefaultConfigFile);

            Debug.Log($"Loading general configuration from {generalConfig}.");

            GeneralConfig = new GeneralConfiguration();
            GeneralConfig.LoadConfig(new ConfigFile(generalConfig, true));

            DebugOn = GeneralConfig.DebugOn.Value;
        }

        public static List<RaidEventConfiguration> LoadSupplemental()
        {
            var supplementalFiles = Directory.GetFiles(Paths.ConfigPath, "custom_raids.supplemental.*");
            var supplementalConfigurations = new List<RaidEventConfiguration>(supplementalFiles.Length);

            if (DebugOn) Debug.Log($"Found {supplementalFiles.Length} supplemental files");

            foreach(var file in supplementalFiles)
            {
                try
                {
                    supplementalConfigurations.AddRange(LoadRaidConfigFile(file));
                }
                catch(Exception e)
                {
                    Debug.LogWarning($"Failed to load supplemental raid {file}: {e.Message}");
                }
            }

            return supplementalConfigurations;
        }

        private static List<RaidEventConfiguration> LoadRaidConfigFile(string configPath)
        {
            Debug.Log($"Loading raid event configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);

            if (GeneralConfig != null) configFile.SaveOnConfigSet = !GeneralConfig.StopTouchingMyConfigs.Value;

            if (DebugOn) CustomLog.LogTrace("Scanning bindings...");
            ConfigurationLoader.ScanBindings<RaidEventConfiguration, SpawnConfiguration>(configFile, false);

            if (DebugOn) CustomLog.LogTrace("Creating configuration...");
            Dictionary<string, RaidEventConfiguration> raidConfiguration = ConfigurationLoader.LoadArrayConfigurations<RaidEventConfiguration, SpawnConfiguration>(configFile, false);
            return raidConfiguration.Values.ToList();
        }
    }
}
