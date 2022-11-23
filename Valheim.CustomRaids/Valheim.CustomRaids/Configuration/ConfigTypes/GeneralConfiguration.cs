using BepInEx.Configuration;
using System;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Configuration.ConfigTypes
{
    [Serializable]
    public class GeneralConfiguration
    {
        [NonSerialized]
        private ConfigFile Config;

        #region General

        public ConfigurationEntry<bool> StopTouchingMyConfigs = new ConfigurationEntry<bool>(false, "Disables automatic updating and saving of raid configurations.\nThis means no comments or missing options will be added, but.. allows you to keep things compact.\nNote: This also has a massive impact upon load time.");

        public ConfigurationEntry<bool> LoadSupplementalRaids = new ConfigurationEntry<bool>(true, "Loads raid configurations from supplemental files.\nEg. custom_raid.supplemental.my_raid.cfg will be included on load.");

        public ConfigurationEntry<bool> GeneratePresetRaids = new ConfigurationEntry<bool>(true, "Generates pre-defined supplemental raids. The generated raids are disabled by default.");

        public ConfigurationEntry<bool> PauseEventTimersWhileOffline = new(true, "Server option. If enabled, pauses the event timers when no players are online. \nThis means if a raid happened a minute before everyone logged out, and everyone logs in an hour later, the game will consider the last raid as having happened one minute before the login.");

        #endregion

        #region Debug

        public ConfigurationEntry<bool> DebugOn = new(false, "Enables debug logging.");

        public ConfigurationEntry<bool> TraceLogging = new(false, "Enables trace logging. Note, this will generate a LOT of log entries.");

        public ConfigurationEntry<bool> WriteDefaultEventDataToDisk = new(false, "If enabled, scans existing raid event data, and dumps to a file.");

        public ConfigurationEntry<bool> WritePostChangeEventDataToDisk = new(false, "If enabled, dumps raid event data after applying configuration to a file.");

        public ConfigurationEntry<bool> WriteEnvironmentDataToDisk = new(false, "If enabled, scans existing environment (weather) data, and dumps to a file.");

        public ConfigurationEntry<bool> WriteGlobalKeyDataToDisk = new(false, "If enabled, scans existing global keys, and dumps to a file.");

        public ConfigurationEntry<string> DebugFileFolder = new("Debug", "Folder path to write to. Root folder is BepInEx.");

        #endregion

        #region EventSystem

        public ConfigurationEntry<bool> RemoveAllExistingRaids = new(false, "If enabled, removes all existing raids and only allows configured. Will only remove non-random events, leaving boss events as is.");

        public ConfigurationEntry<bool> OverrideExisting = new(true, "Enable/disable override of existing events when event names match.");

        public ConfigurationEntry<float> EventCheckInterval = new(46f, "Minutes between checks for starting raids.\nWhen the interval has passed, all raids are checked for valid conditions and a random valid one is selected. Chance is then rolled for if it should start.");

        public ConfigurationEntry<float> EventTriggerChance = new(20f, "Chance of raid, per check interval. 100 is 100%.\nNote: Not used if UseIndividualRaidChecks is enabled, each raid will have their own chance in that case.");

        #endregion

        #region IndividualRaids

        public ConfigurationEntry<bool> UseIndividualRaidChecks = new(false, "If enabled, Custom Raids will overhaul the games way of checking for raids.\nEventTriggerChance will no longer be used, as the chance will be set per raid.\nThis allows for setting individual frequences and chances for each raid.\nThis overhaul gives each raid it's own timer, independent of each other and can therefore cause a LOT of raids.\nEventCheckInterval will still be used to indicate time between checks. \nMinTimeBetweenRaids can be used to ensure they don't happen too often.");

        public ConfigurationEntry<float> MinimumTimeBetweenRaids = new(46, "If overhaul is enabled, ensures a minimum amount of minutes between each raid.");

        #endregion

        public void LoadConfig(ConfigFile configFile)
        {
            Config = configFile;

            LoadSupplementalRaids.Bind(Config, "General", nameof(LoadSupplementalRaids));
            GeneratePresetRaids.Bind(Config, "General", nameof(GeneratePresetRaids));
            StopTouchingMyConfigs.Bind(Config, "General", "StopTouchingMyConfigs");
            PauseEventTimersWhileOffline.Bind(Config, "General", "PauseEventTimersWhileOffline");

            RemoveAllExistingRaids.Bind(Config, "EventSystem", nameof(RemoveAllExistingRaids));
            OverrideExisting.Bind(Config, "EventSystem", nameof(OverrideExisting));
            EventCheckInterval.Bind(Config, "EventSystem", "EventCheckInterval");
            EventTriggerChance.Bind(Config, "EventSystem", "EventTriggerChance");

            UseIndividualRaidChecks.Bind(Config, "IndividualRaids", nameof(UseIndividualRaidChecks));
            MinimumTimeBetweenRaids.Bind(Config, "IndividualRaids", nameof(MinimumTimeBetweenRaids));

            DebugOn.Bind(Config, "Debug", "DebugOn");
            TraceLogging.Bind(Config, "Debug", nameof(TraceLogging));
            WriteDefaultEventDataToDisk.Bind(Config, "Debug", nameof(WriteDefaultEventDataToDisk));
            WritePostChangeEventDataToDisk.Bind(Config, "Debug", nameof(WritePostChangeEventDataToDisk));
            WriteEnvironmentDataToDisk.Bind(Config, "Debug", nameof(WriteEnvironmentDataToDisk));
            WriteGlobalKeyDataToDisk.Bind(Config, "Debug", nameof(WriteGlobalKeyDataToDisk));
            DebugFileFolder.Bind(Config, "Debug", nameof(DebugFileFolder));
        }
    }
}
