
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Conditions
{
    public static class GlobalKeyConditionChecker
    {
        public static bool ShouldFilter(RandomEvent randomEvent, string playerName)
        {
            if(ConditionRequiredGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
                Log.LogDebug($"Raid {randomEvent.m_name} disabled for player {playerName} due to RequiredGlobalKeys");
                return true;
            }

            if(ConditionRequiredNotGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
                Log.LogDebug($"Raid {randomEvent.m_name} disabled for player {playerName} due to RequiredNotGlobalKeys");
                return true;
            }

            if(ConditionRequireOneOfGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
                Log.LogDebug($"Raid {randomEvent.m_name} disabled for player {playerName} due to RequiredOneOfGlobalKeys");
                return true;
            }

            return false;
        }
    }
}
