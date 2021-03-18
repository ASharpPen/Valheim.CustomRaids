using BepInEx;
using HarmonyLib;
using System;
using System.Linq;
using System.Collections.Generic;
using Valheim.CustomRaids.ConfigurationTypes;
using Valheim.CustomRaids.PreConfiguredRaids;
using Valheim.CustomRaids.Compatibility;

namespace Valheim.CustomRaids
{
    [BepInDependency("org.bepinex.plugins.creaturelevelcontrol", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin("asharppen.valheim.custom_raids", "Custom Raids", "1.2.0")]
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

            var harmony = new Harmony("mod.custom_raids");

            CreatureLevelAndLootControlCompatibility.MakeCompatible(harmony);

            harmony.PatchAll();

            Log.Logger = Logger;
        }
    }
}
