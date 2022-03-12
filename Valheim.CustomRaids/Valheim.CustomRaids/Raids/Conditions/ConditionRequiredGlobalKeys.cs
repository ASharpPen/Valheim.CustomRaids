using System.Linq;
using Valheim.CustomRaids.Compatibilities;

namespace Valheim.CustomRaids.Conditions
{
    public static class ConditionRequiredGlobalKeys
    {
        public static bool ShouldFilter(RandomEvent randomEvent, string playerName)
        {
            if((randomEvent.m_requiredGlobalKeys?.Count ?? 0) == 0)
            {
                return false;
            }

            if (CustomRaidPlugin.EnhancedProgressTrackerInstalled)
            {
                return randomEvent.m_requiredGlobalKeys.Any(x => !EnhancedProgressTrackerCompatibilities.HaveGlobalKey(playerName, x.Trim()));
            }

            return randomEvent.m_requiredGlobalKeys.Any(x => !ZoneSystem.instance.GetGlobalKey(x.Trim()));
        }
    }
}
