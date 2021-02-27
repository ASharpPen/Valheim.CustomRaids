using BepInEx;
using HarmonyLib;

namespace Valheim.CustomRaids
{
    [BepInPlugin("asharppen.valheim.custom_raids", "Custom Raids", "0.1.0")]
    public class CustomRaidPlugin : BaseUnityPlugin
    {
        void Awake()
        {
            Logger.LogInfo("Loading configurations...");

            ConfigurationManager.LoadGeneralConfigurations();

            if(!ConfigurationManager.GeneralConfig.LoadRaidConfigsOnWorldStart.Value)
            {
                ConfigurationManager.LoadRaidConfigurations();
            }

            new Harmony("mod.custom_raids").PatchAll();
        }
    }
}
