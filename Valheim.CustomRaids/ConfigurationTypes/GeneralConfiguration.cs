using BepInEx.Configuration;

namespace Valheim.CustomRaids.ConfigurationTypes
{
    public class GeneralConfiguration
    {
        private ConfigFile Config;

        #region General

        public ConfigEntry<bool> StopTouchingMyConfigs;

        public ConfigEntry<bool> LoadRaidConfigsOnWorldStart;

        #endregion

        #region Debug

        public ConfigEntry<bool> DebugOn;

        public ConfigEntry<bool> WriteDefaultEventDataToDisk;

        public ConfigEntry<bool> WriteEnvironmentDataToDisk;

        public ConfigEntry<bool> WriteGlobalKeyDataToDisk;

        #endregion

        #region EventSystem

        public ConfigEntry<bool> RemoveAllExistingRaids;

        public ConfigEntry<float> EventCheckInterval;

        public ConfigEntry<float> EventTriggerChance;

        #endregion

        public void LoadConfig(ConfigFile configFile)
        {
            Config = configFile;

            StopTouchingMyConfigs = configFile.Bind<bool>("General", "StopTouchingMyConfigs", false, "Disables automatic updating and saving of raid configurations.\nThis means no helpers will be added, but.. allows you to keep things compact.");
            LoadRaidConfigsOnWorldStart = configFile.Bind<bool>("General", nameof(LoadRaidConfigsOnWorldStart), false, "Reloads raid configurations when a game world starts.\n This means if you are playing solo, you can edit the file while logged out, without exiting the game completely.");

            DebugOn = configFile.Bind<bool>("Debug", "DebugOn", false, "Enables debug logging.");
            WriteDefaultEventDataToDisk = configFile.Bind<bool>("Debug", nameof(WriteDefaultEventDataToDisk), false, "If enabled, scans existing raid event data, and dumps to a file on disk.");
            WriteEnvironmentDataToDisk = configFile.Bind<bool>("Debug", nameof(WriteEnvironmentDataToDisk), false, "If enabled, scans existing environment data, and dumps to a file on disk.");
            WriteGlobalKeyDataToDisk = configFile.Bind<bool>("Debug", nameof(WriteGlobalKeyDataToDisk), false, "If enabled, scans existing global keys, and dumps to a file on disk.");

            RemoveAllExistingRaids = configFile.Bind<bool>("EventSystem", nameof(RemoveAllExistingRaids), false, "If enabled, removes all existing raids and only allows configured.");
            EventCheckInterval = configFile.Bind<float>("EventSystem", "EventCheckInterval", 0.001f, "Frequency between checks for new raids. Value is in hours");
            EventTriggerChance = configFile.Bind<float>("EventSystem", "EventTriggerChance", 1f, "Chance of raid, per check interval.");
        }
    }
}
