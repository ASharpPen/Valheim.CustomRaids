using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Spawns.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class PreSpawnFilterPatch
    {
        private static FieldInfo FieldAnchor = AccessTools.Field(typeof(SpawnSystem.SpawnData), "m_enabled");
        private static MethodInfo FilterMethod = AccessTools.Method(typeof(PreSpawnFilterPatch), nameof(FilterSpawners), new[] { typeof(SpawnSystem), typeof(SpawnSystem.SpawnData), typeof(bool) });

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AddFilterConditions(IEnumerable<CodeInstruction> instructions)
        {
            var matcher = new CodeMatcher(instructions)
                .MatchForward(
                    true,
                    new CodeMatch(OpCodes.Ldfld, FieldAnchor),
                    new CodeMatch(OpCodes.Brfalse));

            var escapeLoopLabel = matcher.Operand;

            return matcher
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Callvirt, FilterMethod))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, escapeLoopLabel))
                .InstructionEnumeration();
        }

        private static bool FilterSpawners(SpawnSystem spawner, SpawnSystem.SpawnData spawn, bool eventSpawners)
        {
            if (!eventSpawners)
            {
                return false;
            }

            try
            {
                return SpawnConditionManager.Filter(spawner, spawn);
            }
            catch (Exception e)
            {
                Log.LogError($"Error while checking if spawn template {spawn?.m_prefab?.name} should be filtered.", e);
                return false;
            }
        }
    }
}
