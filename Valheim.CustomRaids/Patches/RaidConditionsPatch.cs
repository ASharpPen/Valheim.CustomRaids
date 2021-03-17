using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

                //Do we have a config for it?
                var raidConfig = RandomEventCache.GetConfig(randomEvent);

                if (raidConfig != null)
                {
                    //Lets check conditions.
                    //Get time
                    var seconds = ZNet.instance.GetTimeSeconds();
                    var day = EnvMan.instance.GetDay(seconds);

                    Log.LogTrace($"Checking raid conditionals at time {day}");

                    if (raidConfig.ConditionWorldAgeDaysMin.Value > day)
                    {
                        Log.LogDebug($"Raid {raidConfig.Name} disabled due to world not being old enough. {raidConfig.ConditionWorldAgeDaysMin} > {day}");
                        continue;
                    }
                    else if (raidConfig.ConditionWorldAgeDaysMax.Value > 0 && raidConfig.ConditionWorldAgeDaysMax.Value < day)
                    {
                        Log.LogDebug($"Raid {raidConfig.Name} disabled due to world being too old. {raidConfig.ConditionWorldAgeDaysMax.Value} < {day}");
                        continue;
                    }

                    //Check key conditions.
                    if(raidConfig.RequireOneOfGlobalKeys.Value.Length > 0)
                    {
                        var keys = raidConfig.RequireOneOfGlobalKeys.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        HashSet<string> globalKeys = ZoneSystem.instance
                            .GetGlobalKeys()
                            .Select(x => x.Trim().ToUpperInvariant())
                            .ToHashSet();

                        bool foundRequiredKey = false;
                        foreach(var key in keys)
                        {
                            if(globalKeys.Contains(key.Trim().ToUpperInvariant()))
                            {
                                foundRequiredKey = true;
                                break;
                            }
                        }

                        if(foundRequiredKey == false)
                        {
#if DEBUG
                            Log.LogDebug($"Unable to find any of the keys {raidConfig.RequireOneOfGlobalKeys.Value}");
#endif
                            continue;
                        }
                    }
                }
                else
                {
                    Log.LogTrace($"No config for event {randomEvent.m_name}");
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
