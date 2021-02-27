using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.ConfigurationCore;
using Valheim.CustomRaids.ConfigurationTypes;

namespace Valheim.CustomRaids
{
    [BepInPlugin("asharppen.valheim.custom_raids", "Custom Raids", "0.1.0")]
    public class CustomRaidPlugin : BaseUnityPlugin
    {
        public static bool DebugOn = true;

        public static EventSystemConfiguration EventSystemConfig;

        public static List<RaidEventConfiguration> RaidConfigurations;

        void Awake()
        {
            Logger.LogInfo("Loading configurations...");

            string configPath = Path.Combine(Paths.ConfigPath, "custom_raid.cfg");

            var configFile = new ConfigFile(configPath, true);

            if (DebugOn) Logger.LogInfo("Scanning bindings...");

            ConfigurationLoader.ScanBindings<RaidEventConfiguration, SpawnConfiguration>(configFile, DebugOn);

            if (DebugOn) Logger.LogInfo("Creating configuration...");
            Dictionary<string, RaidEventConfiguration> raidConfiguration = ConfigurationLoader.LoadArrayConfigurations<RaidEventConfiguration, SpawnConfiguration>(configFile, DebugOn);
            RaidConfigurations = raidConfiguration.Values.ToList();

            if (DebugOn) Logger.LogInfo("Loading general event system configuration.");
            EventSystemConfig = new EventSystemConfiguration();

            string generalConfig = Path.Combine(Paths.ConfigPath, "custom_raid.general.cfg");
            EventSystemConfig.LoadConfig(new ConfigFile(generalConfig, true));

            Logger.LogInfo("Configuration loading complete." + raidConfiguration);

            new Harmony("mod.custom_raids").PatchAll();
        }
    }
}
