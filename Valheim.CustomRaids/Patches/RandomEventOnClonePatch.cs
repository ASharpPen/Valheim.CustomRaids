using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(RandomEvent))]
    public static class RandomEventOnClonePatch
    {
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
        }
    }
}
