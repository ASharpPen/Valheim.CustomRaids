using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using Valheim.CustomRaids.Compatibilities;
using Valheim.CustomRaids.Conditions;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public static class RandEventSystemHaveGlobalKeyPatch
    {

        [HarmonyPatch("HaveGlobalKeys")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> RemoveStandardGlobalKeysCheck(IEnumerable<CodeInstruction> instructions)
        {
            var result = instructions;
            if(CustomRaidPlugin.EnhancedProgressTrackerInstalled)
            {
                //Only remove the default HaveGlobalKeys if EnhancedProgressTracker is installed
                result = new CodeMatcher(instructions)
                    .MatchForward(false, new CodeMatch(OpCodes.Call, typeof(RandEventSystem).GetMethod("HaveGlobalKeys")))
                    .RemoveInstructionsWithOffsets(-2, 1)
                    .InstructionEnumeration();
            }

#if DEBUG
            foreach(var instruction in instructions)
            {
                Log.LogDebug(instruction.ToString());
            }
#endif

            return result;
        }

        [HarmonyPatch("GetValidEventPoints")]
        [HarmonyPrefix]
        private static void FilterCharactersWithInvalidGlobalKeys(RandomEvent ev, List<ZDO> characters)
        {
            //Only filter using the enhanced keys if EnhancedProgressTracker is installed
            if(!CustomRaidPlugin.EnhancedProgressTrackerInstalled)
            {
                return;
            }

            List<int> toRemove = new List<int>(characters.Count);

            for(int i = 0; i < characters.Count; ++i)
            {
                var playerName = EnhancedProgressTrackerCompatibilities.GetPlayerName(characters[i]);

                if(playerName is not null)
                {
                    if (!EnhancedProgressTrackerCompatibilities.HaveGlobalKeys(playerName, ev))
                    {
#if DEBUG
                        Log.LogDebug($"Filtering raid for {playerName} due to global keys.");
#endif
                        toRemove.Add(i);
                    }

                    if (ConditionRequireOneOfGlobalKeys.ShouldFilter(ev, playerName))
                    {
#if DEBUG
                        Log.LogDebug($"Filtering raid for {playerName} due to RequireOneOf global keys.");
#endif

                        toRemove.Add(i);
                    }
                }
            }

            for(int i = toRemove.Count - 1; i >= 0; --i)
            {
                characters.RemoveAt(toRemove[i]);
            }
        }
    }
}
