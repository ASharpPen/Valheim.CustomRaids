using HarmonyLib;
using System.Collections.Generic;
using Valheim.CustomRaids.Compatibilities;
using Valheim.CustomRaids.Conditions;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public static class RandEventSystemHaveGlobalKeyPatch
    {
        [HarmonyPatch("HaveGlobalKeys")]
        [HarmonyPostfix]
        private static void IgnoreIfEnhancedKeysIsInstalled(RandomEvent ev, ref bool __result)
        {
            __result = true;
        }

        [HarmonyPatch("GetValidEventPoints")]
        [HarmonyPrefix]
        private static void FilterCharactersWithInvalidGlobalKeys(RandomEvent ev, List<ZDO> characters)
        {
            List<int> toRemove = new List<int>(characters.Count);

            for(int i = 0; i < characters.Count; ++i)
            {

                var playerName = EnhancedProgressTrackerCompatibilities.GetPlayerName(characters[i]);

                if (playerName is not null)
                {
#if DEBUG
                    Log.LogDebug($"Player: {playerName}");
#endif
                    if(GlobalKeyConditionChecker.ShouldFilter(ev, playerName))
                    {
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
