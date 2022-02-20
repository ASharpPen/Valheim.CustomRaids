using BepInEx;
using BepInEx.Configuration;
using System;
using System.IO;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Configuration
{
    public static class ConfigurationManager
    {
        private const string DefaultRaidFile = "custom_raids.raids.cfg";
        private const string DefaultConfigFile = "custom_raids.cfg";
        private const string ConfigSupplemental = "custom_raids.supplemental.*.cfg";

        public static bool DebugOn => GeneralConfig?.DebugOn?.Value ?? false;

        public static GeneralConfiguration GeneralConfig;
        public static RaidEventConfigurationFile RaidConfig;

        public static void LoadAllConfigurations()
        {
            GeneralConfig = LoadGeneralConfigurations();

            RaidConfig = LoadRaidConfigurations();
        }

        public static RaidEventConfigurationFile LoadRaidConfigurations()
        {

            string configPath = Path.Combine(Paths.ConfigPath, DefaultRaidFile);

            var configs = LoadRaidConfigFile(configPath);

            if (GeneralConfig.LoadSupplementalRaids.Value)
            {
                var supplementalFiles = Directory.GetFiles(Paths.ConfigPath, ConfigSupplemental, SearchOption.AllDirectories);
                Log.LogDebug($"Found {supplementalFiles.Length} supplemental raid config files");

                foreach (var file in supplementalFiles)
                {
                    try
                    {
                        var supplementalConfig = LoadRaidConfigFile(file);
                        supplementalConfig.MergeInto(configs);
                    }
                    catch(Exception e)
                    {
                        Log.LogError($"Failed to load supplemental config '{file}'.", e);
                    }
                }
            }

            Log.LogDebug("Finished loading raid event configurations");

            return configs;
        }

        public static GeneralConfiguration LoadGeneralConfigurations()
        {
            string configPath = Path.Combine(Paths.ConfigPath, DefaultConfigFile);

            Log.LogDebug($"Loading general configuration from {configPath}.");

            ConfigurationLoader.SanitizeSectionHeaders(configPath);
            ConfigFile configFile = new ConfigFile(configPath, true);

            var generalConfig = new GeneralConfiguration();
            generalConfig.LoadConfig(configFile);

            return generalConfig;
        }

        private static RaidEventConfigurationFile LoadRaidConfigFile(string configPath)
        {
            Log.LogDebug($"Loading raids from {configPath}.");

            ConfigurationLoader.SanitizeSectionHeaders(configPath);
            var configFile = new ConfigFile(configPath, true);

            if (GeneralConfig?.StopTouchingMyConfigs?.Value == true) configFile.SaveOnConfigSet = !GeneralConfig.StopTouchingMyConfigs.Value;

            return ConfigurationLoader.LoadConfiguration<RaidEventConfigurationFile>(configFile);
        }
    }
}
