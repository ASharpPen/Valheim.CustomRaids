using HarmonyLib;
using System.Collections.Generic;
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
                        Log.LogTrace($"Raid {raidConfig.Name} disabled due to world not being old enough. {raidConfig.ConditionWorldAgeDaysMin} > {day}");
                        continue;
                    }
                    else if (raidConfig.ConditionWorldAgeDaysMax.Value > 0 && raidConfig.ConditionWorldAgeDaysMax.Value < day)
                    {
                        Log.LogTrace($"Raid {raidConfig.Name} disabled due to world being too old. {raidConfig.ConditionWorldAgeDaysMax.Value} < {day}");
                        continue;
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
