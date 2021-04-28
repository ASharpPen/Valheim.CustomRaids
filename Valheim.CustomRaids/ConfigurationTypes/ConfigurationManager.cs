using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Valheim.CustomRaids.ConfigurationCore;

namespace Valheim.CustomRaids.ConfigurationTypes
{
    public static class ConfigurationManager
    {
        private const string DefaultRaidFile = "custom_raids.raids.cfg";
        private const string DefaultConfigFile = "custom_raids.cfg";

        public static bool DebugOn => GeneralConfig?.DebugOn?.Value ?? false;

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
                Log.LogDebug("Loading supplemental raids...");

                configs.AddRange(LoadSupplemental());
            }

            RaidConfig = configs;

            Log.LogDebug("Finished loading raid event configurations");
        }

        public static void LoadGeneralConfigurations()
        {
            string generalConfig = Path.Combine(Paths.ConfigPath, DefaultConfigFile);

            Log.LogDebug($"Loading general configuration from {generalConfig}.");

            GeneralConfig = new GeneralConfiguration();
            GeneralConfig.LoadConfig(new ConfigFile(generalConfig, true));
        }

        public static List<RaidEventConfiguration> LoadSupplemental()
        {
            var supplementalFiles = Directory.GetFiles(Paths.ConfigPath, "custom_raids.supplemental.*", SearchOption.AllDirectories);
            var supplementalConfigurations = new List<RaidEventConfiguration>(supplementalFiles.Length);

            Log.LogDebug($"Found {supplementalFiles.Length} supplemental files");

            foreach(var file in supplementalFiles)
            {
                try
                {
                    supplementalConfigurations.AddRange(LoadRaidConfigFile(file));
                }
                catch(Exception e)
                {
                    Log.LogWarning($"Failed to load supplemental raid {file}: {e.Message}");
                }
            }

            return supplementalConfigurations;
        }

        private static List<RaidEventConfiguration> LoadRaidConfigFile(string configPath)
        {
            Log.LogDebug($"Loading raid event configurations from {configPath}.");

            var configFile = new ConfigFile(configPath, true);

            if (GeneralConfig != null) configFile.SaveOnConfigSet = !GeneralConfig.StopTouchingMyConfigs.Value;

            Dictionary<string, RaidEventConfiguration> raidConfiguration = ConfigurationLoader.LoadConfigurationGroup<RaidEventConfiguration, SpawnConfiguration>(configFile);
            return raidConfiguration.Values.ToList();
        }
    }
}
