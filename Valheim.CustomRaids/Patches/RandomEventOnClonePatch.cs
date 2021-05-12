using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Spawns.Caches;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(RandomEvent))]
    public static class RandomEventOnClonePatch
    {
        /*
        [HarmonyPatch("Clone")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ReattachExtended(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Stloc_0))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RandomEventOnClonePatch), nameof(SetExtendedForClone), new[] { typeof(RandomEvent), typeof(RandomEvent) })))
                .InstructionEnumeration();
        }

        private static void SetExtendedForClone(RandomEvent clone, RandomEvent source)
        {
            var extended = RandomEventCache.Get(source);

            if(extended is not null)
            {
                RandomEventCache.Initialize(clone, extended.Config);
            }
        }*/

        [HarmonyPatch("Clone")]
        [HarmonyPostfix]
        private static void CarryConfigs(RandomEvent __instance, RandomEvent ___result)
        {
            var extended = RandomEventCache.Get(__instance);

            if (extended is not null)
            {
                RandomEventCache.Initialize(___result, extended.Config);

                if(__instance.m_spawn.Count == ___result.m_spawn.Count)
                {
                    for(int i = 0; i < __instance.m_spawn.Count; ++i)
                    {
                        var source = __instance.m_spawn[i];
                        var target = ___result.m_spawn[i];

                        var sourceCache = SpawnDataCache.Get(source);

                        if(sourceCache is null)
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
                    ___result.m_spawn.Clear();
                }
            }
        }
    }
}
