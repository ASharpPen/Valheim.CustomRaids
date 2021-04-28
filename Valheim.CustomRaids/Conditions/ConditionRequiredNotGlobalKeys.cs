using System.Linq;
using Valheim.CustomRaids.Compatibilities;

namespace Valheim.CustomRaids.Conditions
{
    public static class ConditionRequiredNotGlobalKeys
    {
        public static bool ShouldFilter(RandomEvent randomEvent, string playerName = null)
        {
            if((randomEvent.m_notRequiredGlobalKeys?.Count ?? 0) == 0)
            {
                return false;
            }

            if (CustomRaidPlugin.EnhancedProgressTrackerInstalled)
            {
                return randomEvent.m_notRequiredGlobalKeys.Any(x => EnhancedProgressTrackerCompatibilities.HaveGlobalKey(playerName, x.Trim()));
            }

            return randomEvent.m_notRequiredGlobalKeys.Any(x => ZoneSystem.instance.GetGlobalKey(x.Trim()));
        }
    }
}
