using HarmonyLib;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids;
using Valheim.CustomRaids.Raids.Managers;
using Valheim.CustomRaids.Spawns.Caches;

namespace Valheim.CustomRaids.Patches;

[HarmonyPatch(typeof(RandomEvent))]
internal static class RandomEventOnClonePatch
{
    [HarmonyPatch(nameof(RandomEvent.Clone))]
    [HarmonyPostfix]
    private static void CarryConfigs(RandomEvent __instance, RandomEvent __result)
    {
        RaidManager.CopyReference(__instance, __result);

        var extended = RandomEventCache.Get(__instance);

        if (extended is not null)
        {
            RandomEventCache.Initialize(__result, extended.Config);

            if (__instance.m_spawn.Count == __result.m_spawn.Count)
            {
                for (int i = 0; i < __instance.m_spawn.Count; ++i)
                {
                    var source = __instance.m_spawn[i];
                    var target = __result.m_spawn[i];

                    var sourceCache = SpawnDataCache.Get(source);

                    if (sourceCache is null)
                    {
                        continue;
                    }

                    SpawnDataCache.GetOrCreate(target)
                        .SetRaidConfig(sourceCache.RaidConfig)
                        .SetSpawnConfig(sourceCache.SpawnConfig);
                }
            }
            else
            {
                Log.LogError($"RandomEvent {__instance.m_name} was cloned incorrectly. Mismatching number spawns from original. Removing all cloned spawns to be safe.");
                __result.m_spawn.Clear();
            }
        }
    }
}
