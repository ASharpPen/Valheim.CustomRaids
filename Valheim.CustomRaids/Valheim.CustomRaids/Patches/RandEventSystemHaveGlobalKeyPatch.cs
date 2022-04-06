using HarmonyLib;
using System.Collections.Generic;
using Valheim.CustomRaids.Compatibilities;
using Valheim.CustomRaids.Conditions;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Patches;

[HarmonyPatch(typeof(RandEventSystem))]
internal static class RandEventSystemHaveGlobalKeyPatch
{
    [HarmonyPatch(nameof(RandEventSystem.HaveGlobalKeys))]
    [HarmonyPostfix]
    private static void IgnoreIfEnhancedKeysIsInstalled(RandomEvent ev, ref bool __result)
    {
        __result = true;
    }

    [HarmonyPatch(nameof(RandEventSystem.GetValidEventPoints))]
    [HarmonyPrefix]
    private static void FilterCharactersWithInvalidGlobalKeys(RandomEvent ev, ref List<ZDO> characters)
    {
        List<ZDO> filtered = new List<ZDO>();

        for (int i = 0; i < characters.Count; ++i)
        {

            var playerName = EnhancedProgressTrackerCompatibilities.GetPlayerName(characters[i]);

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
