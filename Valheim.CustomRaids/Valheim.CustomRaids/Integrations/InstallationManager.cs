namespace Valheim.CustomRaids.Integrations;

internal static class InstallationManager
{
    /// <summary>
    /// World Advancement Progression
    /// </summary>
    public static bool WAPInstalled { get; } = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.orianaventure.mod.WorldAdvancementProgression");
}
