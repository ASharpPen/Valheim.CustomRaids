using HarmonyLib;
using Valheim.CustomRaids.Configuration;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Patches;

namespace Valheim.CustomRaids.Resetter;

[HarmonyPatch(typeof(FejdStartup))]
internal static class WorldStartupResetPatch
{
    internal static GameState State { get; set; }

    /// <summary>
    /// Singleplayer
    /// </summary>
    [HarmonyPatch(nameof(FejdStartup.OnWorldStart))]
    [HarmonyPrefix]
    private static void ResetState()
    {
        State = GameState.Singleplayer;

        Log.LogDebug("OnWorldStart: Resetting configurations");
        StateResetter.Reset();
        ConfigurationManager.LoadAllConfigurations();
        RandEventSystemWaitPatch.Wait = false;
    }

    /// <summary>
    /// Multiplayer
    /// </summary>
    [HarmonyPatch(nameof(FejdStartup.JoinServer))]
    [HarmonyPrefix]
    private static void ResetStateMultiplayer()
    {
        State = GameState.Multiplayer;

        Log.LogDebug("JoinServer: Resetting configurations");
        StateResetter.Reset();
    }

    /// <summary>
    /// Server
    /// </summary>
    [HarmonyPatch(nameof(FejdStartup.ParseServerArguments))]
    [HarmonyPrefix]
    private static void ResetStateServer()
    {
        State = GameState.Dedicated;

        Log.LogDebug("ParseServerArguments: Resetting configurations");
        StateResetter.Reset();
        ConfigurationManager.LoadAllConfigurations();
        RandEventSystemWaitPatch.Wait = false;
    }
}

internal enum GameState
{
    Singleplayer,
    Multiplayer,
    Dedicated
}