
namespace Valheim.CustomRaids.Conditions
{
    public static class GlobalKeyConditionChecker
    {
        public static bool ShouldFilter(RandomEvent randomEvent, string playerName)
        {
            if(ConditionRequiredGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
#if DEBUG
                Log.LogTrace($"Filtering {randomEvent.m_name} for player {playerName} due to RequiredGlobalKeys");
#endif
                return true;
            }

            if(ConditionRequiredNotGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
#if DEBUG
                Log.LogTrace($"Filtering {randomEvent.m_name} for player {playerName} due to RequiredNotGlobalKeys");
#endif
                return true;
            }

            if(ConditionRequireOneOfGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
#if DEBUG
                Log.LogTrace($"Filtering {randomEvent.m_name} for player {playerName} due to RequiredOneOfGlobalKeys");
#endif
                return true;
            }

            return false;
        }
    }
}
