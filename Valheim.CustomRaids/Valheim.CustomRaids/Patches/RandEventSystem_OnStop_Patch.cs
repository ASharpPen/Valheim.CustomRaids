using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Valheim.CustomRaids.Raids.Managers;

namespace Valheim.CustomRaids.Patches;

[HarmonyPatch]
internal static class RandEventSystem_OnStop_Patch
{
    [HarmonyPatch(typeof(RandEventSystem), nameof(RandEventSystem.SetRandomEvent))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnStopRandomEventRunRaidActions(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to right before random event is stopped and set to null.
            .MatchForward(true,
                new CodeMatch(OpCodes.Ldnull),
                new CodeMatch(OpCodes.Stfld))
            // Activate OnStop
            .InsertAndAdvance(Transpilers.EmitDelegate(RaidActionManager.ExecuteOnStopRandomEventActions))
            .InstructionEnumeration();
    }

    [HarmonyPatch(typeof(RandEventSystem), nameof(RandEventSystem.SetForcedEvent))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> OnStopForcedEventRunRaidActions(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to right before forced event is stopped and set to null.
            .MatchForward(true,
                new CodeMatch(OpCodes.Ldnull),
                new CodeMatch(OpCodes.Stfld))
            // Activate OnStop
            .InsertAndAdvance(Transpilers.EmitDelegate(RaidActionManager.ExecuteOnStopForcedEventActions))
            .InstructionEnumeration();
    }
}
