using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;
using Valheim.CustomRaids.Utilities.Extensions;
using SpawnConfiguration = Valheim.CustomRaids.Configuration.ConfigTypes.SpawnConfiguration;

namespace Valheim.CustomRaids.Spawns.Conditions
{
    internal class ConditionNotGlobalKeys : ISpawnCondition
    {
        private static ConditionNotGlobalKeys _instance;

        public static ConditionNotGlobalKeys Instance
        {
            get
            {
                return _instance ??= new ConditionNotGlobalKeys();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnConfiguration config)
        {
            if (IsValid(config))
            {
                return false;
            }

            Log.LogTrace($"Ignoring config {config.Name} due to finding a global key from {nameof(config.RequiredNotGlobalKey)}.");
            return true;
        }

        public bool IsValid(SpawnConfiguration config)
        {
            if (!string.IsNullOrEmpty(config.RequiredNotGlobalKey?.Value))
            {
                var requiredNotKeys = config.RequiredNotGlobalKey.Value.SplitByComma();

                if (requiredNotKeys.Count > 0)
                {
                    foreach (var key in requiredNotKeys)
                    {
                        if (ZoneSystem.instance.GetGlobalKey(key))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
