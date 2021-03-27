using HarmonyLib;
using Valheim.CustomRaids.ConfigurationTypes;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(FejdStartup), "OnWorldStart")]
    public static class ResetConfigurations
    {
        private static void Postfix()
        {
            //Check for singleplayer.
            if (ZNet.instance == null)
            {
                Log.LogDebug("Resetting configurations");

                ConfigurationManager.LoadAllConfigurations();
            }
        }
    }
}
