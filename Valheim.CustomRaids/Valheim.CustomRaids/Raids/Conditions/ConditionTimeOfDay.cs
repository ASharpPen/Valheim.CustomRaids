using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionTimeOfDay : IRaidCondition
    {
        public bool? DuringDay { get; set; }

        public bool? DuringNight { get; set; }

        public bool IsValid(RaidContext context)
        {
            if (DuringDay is not null)
            {
                if (!DuringDay.Value && EnvMan.instance.IsDay())
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due to not being allowed to start during day.");
                    return false;
                }
            }

            if (DuringNight is not null)
            {
                if (!DuringNight.Value && EnvMan.instance.IsNight())
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due to not being allowed to start during night.");
                    return false;
                }
            }

            return true;
        }
    }
}
