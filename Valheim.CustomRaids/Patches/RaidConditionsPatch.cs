using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.Conditions;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public static class RaidConditionsPatch
    {
        [HarmonyPatch("GetPossibleRandomEvents")]
        [HarmonyPostfix]
        private static void FilterPossibleEvents(List<KeyValuePair<RandomEvent, Vector3>> __result)
        {
            List<KeyValuePair<RandomEvent, Vector3>> filtered = new List<KeyValuePair<RandomEvent, Vector3>>(__result.Count);

            Log.LogTrace($"Checking {__result.Count} raids for conditionals");

            for (int i = 0; i < __result.Count; ++i)
            {
                var randomEvent = __result[i].Key;
                var raidPosition = __result[i].Value;

                if(ConditionChecker.ShouldFilter(randomEvent, raidPosition))
                {
                    continue;
                }

                filtered.Add(__result[i]);
            }

            if(__result.Count != filtered.Count)
            {
                __result.Clear();
                __result.AddRange(filtered);
            }
        }
    }
}
