using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.Spawns.Patches;

[HarmonyPatch(typeof(SpawnSystem))]
public static class OnSpawnPatch
{
    [HarmonyPatch(nameof(SpawnSystem.Spawn))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> GetSpawnReference(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false, new CodeMatch(OpCodes.Stloc_0))
            .Advance(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0))
            .InsertAndAdvance(Transpilers.EmitDelegate(StoreReference))
            .InstructionEnumeration();
    }

    private static GameObject _spawnReference;

    private static void StoreReference(GameObject spawn) => _spawnReference = spawn;

    [HarmonyPatch(nameof(SpawnSystem.Spawn))]
    [HarmonyPostfix]
    private static void ModifySpawn(SpawnSystem __instance, SpawnSystem.SpawnData critter, bool eventSpawner)
    {
        try
        {
            if (!eventSpawner)
            {
                return;
            }

            if (_spawnReference.IsNull())
            {
                return;
            }

            SpawnModificationManager.ApplyModifiers(__instance, _spawnReference, critter);
        }
        catch (Exception e)
        {
            Log.LogError("Error while attempting to modify event spawn.", e);
        }
    }
}
