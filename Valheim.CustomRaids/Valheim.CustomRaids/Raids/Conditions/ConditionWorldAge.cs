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
                if (day < MinDays)
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due to world not being old enough. {day} < {MinDays}");
                    return false;
                }

            }

            if (MaxDays is not null)
            {
                if (MaxDays > 0 && day > MaxDays)
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due to world being too old. {day} > {MaxDays}");
                    return false;
                }
            }

            return true;
        }
    }
}
