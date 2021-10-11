using HarmonyLib;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Patches;
using Valheim.CustomRaids.Raids.Managers;

namespace Valheim.CustomRaids
{
    [HarmonyPatch(typeof(RandEventSystem), "Start")]
    public static class RandEventSystemPatch
    {
        [HarmonyPostfix]
        private static void RandEventSystemStart(RandEventSystem __instance)
        {
            Log.LogDebug("Starting RandEventSystem");

            //If singleplayer, ZNet will not be initialized here.
            if (ZNet.instance == null)
            {
                RaidConfigManager.ApplyConfigs();
                RandEventSystemWaitPatch.Wait = false;
            }
            else if(ZNet.instance.IsServer())
            {
                RaidConfigManager.ApplyConfigs();
                RandEventSystemWaitPatch.Wait = false;
            }
        }
    }
}
