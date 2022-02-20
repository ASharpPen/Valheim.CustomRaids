using HarmonyLib;
using System.Collections;
using UnityEngine;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids.Managers;
using Valheim.CustomRaids.Resetter;

namespace Valheim.CustomRaids.Patches
{

    [HarmonyPatch(typeof(Game))]
    internal static class ConfigLoadingPatch
    {
        private static bool FirstTime = true;

        static ConfigLoadingPatch()
        {
            StateResetter.Subscribe(() =>
            {
                FirstTime = true;
            });
        }

        [HarmonyPatch("FindSpawnPoint")]
        [HarmonyPostfix]
        private static void LoadConfigs(Game __instance)
        {
            if (FirstTime)
            {
                FirstTime = false;
                _ = __instance.StartCoroutine(ReleaseConfigs());
            }
        }

        public static IEnumerator ReleaseConfigs()
        {
            Log.LogDebug("Starting early delay for config application.");

            yield return new WaitForSeconds(2);

            Log.LogDebug("Finished early delay for config application.");
            RaidConfigManager.ApplyConfigs();
            RandEventSystemWaitPatch.Wait = false;
        }
    }
}
