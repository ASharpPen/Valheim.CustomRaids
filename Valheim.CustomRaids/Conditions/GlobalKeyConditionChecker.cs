
namespace Valheim.CustomRaids.Conditions
{
    public static class GlobalKeyConditionChecker
    {
        public static bool ShouldFilter(RandomEvent randomEvent, string playerName)
        {
            if(ConditionRequiredGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
                return true;
            }

            if(ConditionRequiredNotGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
                return true;
            }

            if(ConditionRequireOneOfGlobalKeys.ShouldFilter(randomEvent, playerName))
            {
                return true;
            }

            return false;
        }
    }
}
