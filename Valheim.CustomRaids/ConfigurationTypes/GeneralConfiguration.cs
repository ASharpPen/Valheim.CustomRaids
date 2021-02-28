using BepInEx.Configuration;

namespace Valheim.CustomRaids.ConfigurationTypes
{
    public class GeneralConfiguration
    {
        private ConfigFile Config;

        #region General

        public ConfigEntry<bool> StopTouchingMyConfigs;

        public ConfigEntry<bool> LoadRaidConfigsOnWorldStart;

        public ConfigEntry<bool> LoadSupplementalRaids;

        public ConfigEntry<bool> GeneratePresetRaids;

        #endregion

        #region Debug

        public ConfigEntry<bool> DebugOn;

        public ConfigEntry<bool> WriteDefaultEventDataToDisk;

        public ConfigEntry<bool> WritePostChangeEventDataToDisk;

        public ConfigEntry<bool> WriteEnvironmentDataToDisk;

        public ConfigEntry<bool> WriteGlobalKeyDataToDisk;

        #endregion

        #region EventSystem

        public ConfigEntry<bool> RemoveAllExistingRaids;

        public ConfigEntry<bool> OverrideExisting;

        public ConfigEntry<float> EventCheckInterval;

        public ConfigEntry<float> EventTriggerChance;

        #endregion

        public void LoadConfig(ConfigFile configFile)
        {
            Config = configFile;

            StopTouchingMyConfigs = configFile.Bind<bool>("General", "StopTouchingMyConfigs", false, "Disables automatic updating and saving of raid configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.");
            LoadRaidConfigsOnWorldStart = configFile.Bind<bool>("General", nameof(LoadRaidConfigsOnWorldStart), false, "Reloads raid configurations when a game world starts.\nThis means if you are playing solo, you can edit the raid files while logged out, without exiting the game completely.");
            LoadSupplementalRaids = configFile.Bind<bool>("General", nameof(LoadSupplementalRaids), true, "Loads raid configurations from supplemental files.\nEg. custom_raid.supplemental.my_raid.cfg will be included on load.");
            GeneratePresetRaids = configFile.Bind<bool>("General", nameof(GeneratePresetRaids), true, "Generates pre-defined supplemental raids. The generated raids are disabled by default.");

            DebugOn = configFile.Bind<bool>("Debug", "DebugOn", false, "Enables debug logging.");
            WriteDefaultEventDataToDisk = configFile.Bind<bool>("Debug", nameof(WriteDefaultEventDataToDisk), false, "If enabled, scans existing raid event data, and dumps to a file on disk.");
            WritePostChangeEventDataToDisk = configFile.Bind<bool>("Debug", nameof(WritePostChangeEventDataToDisk), false, "If enabled, dumps raid event data after applying configuration to a file on disk.");
            WriteEnvironmentDataToDisk = configFile.Bind<bool>("Debug", nameof(WriteEnvironmentDataToDisk), false, "If enabled, scans existing environment data, and dumps to a file on disk.");
            WriteGlobalKeyDataToDisk = configFile.Bind<bool>("Debug", nameof(WriteGlobalKeyDataToDisk), false, "If enabled, scans existing global keys, and dumps to a file on disk.");

            RemoveAllExistingRaids = configFile.Bind<bool>("EventSystem", nameof(RemoveAllExistingRaids), false, "If enabled, removes all existing raids and only allows configured.");
            OverrideExisting = configFile.Bind<bool>("EventSystem", nameof(OverrideExisting), true, "Enable/disable override of existing events when event names match.");
            EventCheckInterval = configFile.Bind<float>("EventSystem", "EventCheckInterval", 1f, "Frequency between checks for new raids. Value is in hours");
            EventTriggerChance = configFile.Bind<float>("EventSystem", "EventTriggerChance", 1f, "Chance of raid, per check interval. 1 is 100%.");
        }
    }
}
