using HarmonyLib;
using System.Collections.Generic;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.Raids.Serverside.Schedulers.Default
{
    [HarmonyPatch(typeof(RandEventSystem))]
    internal static class Patch_RandEventSystem_GlobalKeys
    {
        [HarmonyPatch("HaveGlobalKeys")]
        [HarmonyPostfix]
        private static void IgnoreIfEnhancedKeysIsInstalled(RandomEvent ev, ref bool __result)
        {
            __result = true;
        }

        [HarmonyPatch("GetValidEventPoints")]
        [HarmonyPrefix]
        private static void FilterCharactersWithInvalidGlobalKeys(RandomEvent ev, ref List<ZDO> characters)
        {
            List<ZDO> filtered = new List<ZDO>();

            for (int i = 0; i < characters.Count; ++i)
            {
                var playerName = characters[i].GetPlayerName();

                if (playerName is not null)
                {
                    if (GlobalKeyConditionChecker.ShouldFilter(ev, playerName))
                    {
#if DEBUG
                        Log.LogDebug($"Filtering GetValidEventPoints for {ev.m_name}");
#endif

                        continue;
                    }
                }

                filtered.Add(characters[i]);
            }

            characters = filtered;
        }
    }
}
