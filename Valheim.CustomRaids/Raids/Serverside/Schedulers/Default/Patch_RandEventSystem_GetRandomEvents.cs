using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Valheim.CustomRaids.Raids.Schedulers;

namespace Valheim.CustomRaids.Raids.Serverside.Schedulers.Default
{
    [HarmonyPatch(typeof(RandEventSystem))]
    internal static class Patch_RandEventSystem_GetPossibleRandomEvents
    {
        private static FieldInfo EventsField = AccessTools.Field(typeof(RandEventSystem), nameof(RandEventSystem.m_events));

        private static MethodInfo GetSchedulerEvents = AccessTools.Method(typeof(DefaultScheduler), nameof(DefaultScheduler.GetRandomEvents));

        [HarmonyPatch(nameof(RandEventSystem.GetPossibleRandomEvents))]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ReplaceRaidList(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                // Move to before events list is loaded
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld, EventsField))
                // Replace with call to own event list.
                .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Nop))
                .SetInstructionAndAdvance(new CodeInstruction(OpCodes.Call, GetSchedulerEvents))
                .InstructionEnumeration();
        }
    }
}
