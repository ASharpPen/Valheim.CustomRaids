using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.CustomRaids.Spawns.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class OnSpawnPatch
    {
        private static MethodInfo ModifySpawnMethod = AccessTools.Method(typeof(OnSpawnPatch), nameof(ModifySpawn));

        [HarmonyPatch("Spawn")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> HookSpawned(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Stloc_0))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, ModifySpawnMethod))
                .InstructionEnumeration();
        }

        private static void ModifySpawn(SpawnSystem spawnSystem, SpawnSystem.SpawnData spawner, GameObject spawn, bool isEventCreature)
        {
            if (!isEventCreature)
            {
                return;
            }

            SpawnModificationManager.ApplyModifiers(spawnSystem, spawn, spawner);
        }
    }
}
