using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids.Managers;

namespace Valheim.CustomRaids.Patches;

[HarmonyPatch(typeof(RandEventSystem))]
internal static class RaidConditionsPatch
{
    [HarmonyPatch(nameof(RandEventSystem.GetPossibleRandomEvents))]
    [HarmonyPostfix]
    private static void FilterPossibleEvents(List<KeyValuePair<RandomEvent, Vector3>> __result)
    {
        List<KeyValuePair<RandomEvent, Vector3>> filtered = new List<KeyValuePair<RandomEvent, Vector3>>(__result.Count);

        Log.LogTrace($"Checking {__result.Count} raids for conditionals");

        for (int i = 0; i < __result.Count; ++i)
        {
            var randomEvent = __result[i].Key;
            var raidPosition = __result[i].Value;

            if (randomEvent is null)
            {
#if DEBUG
                    Log.LogWarning($"RandEventSystem.GetPossibleRandomEvents had a null event at index '{i}'");
#endif
                continue;
            }

            if (!RaidConditionManager.HasValidConditions(randomEvent, raidPosition))
            {
                continue;
            }

            filtered.Add(__result[i]);
        }

        if (__result.Count != filtered.Count)
        {
            __result.Clear();
            __result.AddRange(filtered);
        }
    }
}
