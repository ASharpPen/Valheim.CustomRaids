using BepInEx;
using HarmonyLib;
using Valheim.CustomRaids.Configuration;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.PreConfiguredRaids;
using Valheim.CustomRaids.TerminalCommands;

namespace Valheim.CustomRaids;

[BepInDependency("asharppen.valheim.spawn_that", BepInDependency.DependencyFlags.SoftDependency)]
[BepInPlugin(ModId, PluginName, Version)]
public class CustomRaidPlugin : BaseUnityPlugin
{
    public const string ModId = "asharppen.valheim.custom_raids";
    public const string PluginName = "Custom Raids";
    public const string Version = "1.7.9";

    void Awake()
    {
        Log.Logger = Logger;

        Logger.LogInfo("Loading configurations...");

        ConfigurationManager.GeneralConfig =  ConfigurationManager.LoadGeneralConfigurations();

        if (ConfigurationManager.GeneralConfig.GeneratePresetRaids.Value)
        {
            new Ragnarok().CreateConfigIfMissing();
            new DeathsquitoSeason().CreateConfigIfMissing();
        }

        var harmony = new Harmony(ModId);

        harmony.PatchAll();

        LoadCommands();
    }

    private static void LoadCommands()
    {
        ListPlayerReadyEventsCommand.Register();
        ExplainPlayerReadyEventCommand.Register();

#if DEBUG
        ListPlayerKnownItemsCommand.Register();
        ListPlayerKeysCommand.Register();
        UpdatePlayerReadyEventsCommand.Register();
#endif
    }
}
