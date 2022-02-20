using UnityEngine;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids;

namespace Valheim.CustomRaids.Conditions
{
    public static class ConditionChecker
    {
        public static bool ShouldFilter(RandomEvent randomEvent, Vector3 raidPosition)
        {
            //Do we have a config for it?
            var raidConfig = RandomEventCache.GetConfig(randomEvent);

            if (raidConfig != null)
            {
                //Lets check conditions.
                //Get time
                var seconds = ZNet.instance.GetTimeSeconds();
                var day = EnvMan.instance.GetDay(seconds);

                Log.LogTrace($"Checking raid conditionals at day {day}");

                if (!raidConfig.CanStartDuringDay.Value && EnvMan.instance.IsDay())
                {
                    Log.LogDebug($"Raid {raidConfig.Name} disabled due to not being allowed to start during day.");
                    return true;
                }

                if (!raidConfig.CanStartDuringNight.Value && EnvMan.instance.IsNight())
                {
                    Log.LogDebug($"Raid {raidConfig.Name} disabled due to not being allowed to start during night.");
                    return true;
                }

                if (raidConfig.ConditionWorldAgeDaysMin.Value > day)
                {
                    Log.LogDebug($"Raid {raidConfig.Name} disabled due to world not being old enough. {raidConfig.ConditionWorldAgeDaysMin} > {day}");
                    return true;
                }
                else if (raidConfig.ConditionWorldAgeDaysMax.Value > 0 && raidConfig.ConditionWorldAgeDaysMax.Value < day)
                {
                    Log.LogDebug($"Raid {raidConfig.Name} disabled due to world being too old. {raidConfig.ConditionWorldAgeDaysMax.Value} < {day}");
                    return true;
                }

                var distanceToCenter = raidPosition.magnitude;

                if (raidConfig.ConditionDistanceToCenterMin.Value > distanceToCenter)
                {
                    Log.LogDebug($"Raid {raidConfig.Name} disabled due being too far from center. {raidConfig.ConditionDistanceToCenterMin.Value} > {distanceToCenter}");
                    return true;
                }
                else if (raidConfig.ConditionDistanceToCenterMax.Value > 0 && raidConfig.ConditionDistanceToCenterMax.Value < distanceToCenter)
                {
                    Log.LogDebug($"Raid {raidConfig.Name} disabled due being too close to center. {raidConfig.ConditionDistanceToCenterMax.Value} < {distanceToCenter}");
                    return true;
                }
            }
            else
            {
#if DEBUG
                Log.LogDebug($"No config for event {randomEvent.m_name}");
#endif
            }

            return false;
        }
    }
}
