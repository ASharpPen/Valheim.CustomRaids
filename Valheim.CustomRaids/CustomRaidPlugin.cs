using BepInEx;
using HarmonyLib;
using System;
using System.Linq;
using System.Collections.Generic;
using Valheim.CustomRaids.ConfigurationTypes;
using Valheim.CustomRaids.PreConfiguredRaids;

namespace Valheim.CustomRaids
{
    [BepInDependency("asharppen.valheim.enhanced_progress_tracker", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("asharppen.valheim.custom_raids", "Custom Raids", "1.3.2")]
    public class CustomRaidPlugin : BaseUnityPlugin
    {
        void Awake()
        {
            Log.Logger = Logger;

            Logger.LogInfo("Loading configurations...");

            ConfigurationManager.LoadGeneralConfigurations();

            if (ConfigurationManager.GeneralConfig.GeneratePresetRaids.Value)
            {
                new Ragnarok().CreateConfigIfMissing();
                new DeathsquitoSeason().CreateConfigIfMissing();
            }

            if(EnhancedProgressTrackerInstalled)
            {
                Log.LogDebug("Detected installation of Enhanced Progress Tracker.");
            }

            var harmony = new Harmony("mod.custom_raids");

            harmony.PatchAll();

            Log.Logger = Logger;
        }

        public static bool EnhancedProgressTrackerInstalled { get; } = Type.GetType("Valheim.EnhancedProgressTracker.EnhancedProgressTracker, Valheim.EnhancedProgressTracker") is not null;
    }
}
