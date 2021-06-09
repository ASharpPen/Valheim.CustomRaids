using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;
using SpawnConfiguration = Valheim.CustomRaids.Configuration.ConfigTypes.SpawnConfiguration;

namespace Valheim.CustomRaids.Spawns.Conditions
{
    public class ConditionWorldAge : ISpawnCondition
    {
        private static ConditionWorldAge _instance;

        public static ConditionWorldAge Instance
        {
            get
            {
                return _instance ??= new ConditionWorldAge();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnConfiguration config)
        {
            if (IsValid(config))
            {
                return false;
            }

            Log.LogTrace($"Filtering spawn {config.Name} due to world age.");
            return true;
        }

        public bool IsValid(SpawnConfiguration config)
        {
            int day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

            if (config.ConditionWorldAgeDaysMin.Value > 0 && config.ConditionWorldAgeDaysMin.Value > day)
            {
                return false;
            }

            if (config.ConditionWorldAgeDaysMax.Value > 0 && config.ConditionWorldAgeDaysMax.Value < day)
            {
                return false;
            }

            return true;
        }
    }
}
