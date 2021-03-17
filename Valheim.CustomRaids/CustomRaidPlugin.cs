using BepInEx;
using HarmonyLib;
using Valheim.CustomRaids.ConfigurationTypes;
using Valheim.CustomRaids.PreConfiguredRaids;

namespace Valheim.CustomRaids
{
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

            new Harmony("mod.custom_raids").PatchAll();

            Log.Logger = Logger;
        }
    }
}
