using HarmonyLib;
using Valheim.CustomRaids.ConfigurationTypes;
using Valheim.CustomRaids.Patches;

namespace Valheim.CustomRaids.Resetter
{
    [HarmonyPatch(typeof(FejdStartup))]
    public static class WorldStartupResetPatch
    {
        /// <summary>
        /// Singleplayer
        /// </summary>
        [HarmonyPatch("OnWorldStart")]
        [HarmonyPrefix]
        private static void ResetState()
        {
            Log.LogDebug("OnWorldStart: Resetting configurations");
            StateResetter.Reset();
            ConfigurationManager.LoadAllConfigurations();
            RandEventSystemWaitPatch.Wait = false;
        }

        /// <summary>
        /// Multiplayer
        /// </summary>
        [HarmonyPatch("JoinServer")]
        [HarmonyPrefix]
        private static void ResetStateMultiplayer()
        {
            Log.LogDebug("JoinServer: Resetting configurations");
            StateResetter.Reset();
        }

        /// <summary>
        /// Server
        /// </summary>
        [HarmonyPatch("ParseServerArguments")]
        [HarmonyPrefix]
        private static void ResetStateServer()
        {
            Log.LogDebug("ParseServerArguments: Resetting configurations");
            StateResetter.Reset();
            ConfigurationManager.LoadAllConfigurations();
            RandEventSystemWaitPatch.Wait = false;
        }
    }
}
