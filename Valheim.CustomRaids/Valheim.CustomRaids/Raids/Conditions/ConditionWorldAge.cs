using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionWorldAge : IRaidCondition
    {
        public float? MinDays { get; set; }

        public float? MaxDays { get; set; }

        public bool IsValid(RaidContext context)
        {
            //Get time
            var seconds = ZNet.instance.GetTimeSeconds();
            var day = EnvMan.instance.GetDay(seconds);

            if (MinDays is not null)
            {
                if (MinDays > day)
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due to world not being old enough. {MinDays} > {day}");
                    return false;
                }

            }

            if (MaxDays is not null)
            {
                if (MaxDays > 0 && MaxDays < day)
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due to world being too old. {MaxDays} < {day}");
                    return false;
                }
            }

            return true;
        }
    }
}
