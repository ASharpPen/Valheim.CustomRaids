using HarmonyLib;
using System.Collections.Generic;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Resetter;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public static class RandEventSystemWaitPatch
    {
        internal static bool Wait = true;

        static RandEventSystemWaitPatch()
        {
            StateResetter.Subscribe(() =>
            {
                Wait = true;
            });
        }

        [HarmonyPatch(nameof(RandEventSystem.GetCurrentSpawners))]
        [HarmonyPrefix]
        private static bool GetSpawnersWait(ref List<SpawnSystem.SpawnData> __result)
        {
            if(Wait)
            {
                Log.LogTrace("Waiting for green light.");

                __result = new List<SpawnSystem.SpawnData>();
                return false;
            }

            return true;
        }

        [HarmonyPatch("RPC_SetEvent")]
        [HarmonyPrefix]
        private static bool ActiveEventWait()
        {
            if(Wait)
            {
                Log.LogTrace("Skipping raid message from server. Waiting for green light.");
                return false;
            }

            return true;
        }
    }
}
