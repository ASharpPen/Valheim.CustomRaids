using BepInEx.Configuration;
using System;
using Valheim.CustomRaids.ConfigurationCore;

namespace Valheim.CustomRaids.ConfigurationTypes
{
    [Serializable]
    public class GeneralConfiguration
    {
        [NonSerialized]
        private ConfigFile Config;

        #region General

        public ConfigurationEntry<bool> StopTouchingMyConfigs = new ConfigurationEntry<bool>(false, "Disables automatic updating and saving of raid configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.");

        public ConfigurationEntry<bool> LoadSupplementalRaids = new ConfigurationEntry<bool>(true, "Loads raid configurations from supplemental files.\nEg. custom_raid.supplemental.my_raid.cfg will be included on load.");

        public ConfigurationEntry<bool> GeneratePresetRaids = new ConfigurationEntry<bool>(true, "Generates pre-defined supplemental raids. The generated raids are disabled by default.");

        #endregion

        #region Debug

        public ConfigurationEntry<bool> DebugOn = new ConfigurationEntry<bool>(false, "Enables debug logging.");

        public ConfigurationEntry<bool> TraceLogging = new ConfigurationEntry<bool>(false, "Enables trace logging. Note, this will generate a LOT of log entries.");

        public ConfigurationEntry<bool> WriteDefaultEventDataToDisk = new ConfigurationEntry<bool>(false, "If enabled, scans existing raid event data, and dumps to a file on disk.");

        public ConfigurationEntry<bool> WritePostChangeEventDataToDisk = new ConfigurationEntry<bool>(false, "If enabled, dumps raid event data after applying configuration to a file on disk.");

        public ConfigurationEntry<bool> WriteEnvironmentDataToDisk = new ConfigurationEntry<bool>(false, "If enabled, scans existing environment data, and dumps to a file on disk.");

        public ConfigurationEntry<bool> WriteGlobalKeyDataToDisk = new ConfigurationEntry<bool>(false, "If enabled, scans existing global keys, and dumps to a file on disk.");

        #endregion

        #region EventSystem

        public ConfigurationEntry<bool> RemoveAllExistingRaids = new ConfigurationEntry<bool>(false, "If enabled, removes all existing raids and only allows configured.");

        public ConfigurationEntry<bool> OverrideExisting = new ConfigurationEntry<bool>(true, "Enable/disable override of existing events when event names match.");

        public ConfigurationEntry<float> EventCheckInterval = new ConfigurationEntry<float>(46f, "Frequency between checks for new raids. Value is in minutes");

        public ConfigurationEntry<float> EventTriggerChance = new ConfigurationEntry<float>(20f, "Chance of raid, per check interval. 100 is 100%.");

        #endregion

        public void LoadConfig(ConfigFile configFile)
        {
            Config = configFile;

            LoadSupplementalRaids.Bind(Config, "General", nameof(LoadSupplementalRaids));
            GeneratePresetRaids.Bind(Config, "General", nameof(GeneratePresetRaids));
            StopTouchingMyConfigs.Bind(Config, "General", "StopTouchingMyConfigs");

            RemoveAllExistingRaids.Bind(Config, "EventSystem", nameof(RemoveAllExistingRaids));
            OverrideExisting.Bind(Config, "EventSystem", nameof(OverrideExisting));
            EventCheckInterval.Bind(Config, "EventSystem", "EventCheckInterval");
            EventTriggerChance.Bind(Config, "EventSystem", "EventTriggerChance");

            DebugOn.Bind(Config, "Debug", "DebugOn");
            TraceLogging.Bind(Config, "Debug", nameof(TraceLogging));
            WriteDefaultEventDataToDisk.Bind(Config, "Debug", nameof(WriteDefaultEventDataToDisk));
            WritePostChangeEventDataToDisk.Bind(Config, "Debug", nameof(WritePostChangeEventDataToDisk));
            WriteEnvironmentDataToDisk.Bind(Config, "Debug", nameof(WriteEnvironmentDataToDisk));
            WriteGlobalKeyDataToDisk.Bind(Config, "Debug", nameof(WriteGlobalKeyDataToDisk));
        }
    }
}
