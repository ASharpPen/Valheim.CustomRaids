using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Spawns.Caches;
using Valheim.CustomRaids.Spawns.Conditions;
using Valheim.CustomRaids.Spawns.Conditions.ModSpecific;

namespace Valheim.CustomRaids.Spawns
{
    public static class SpawnConditionManager
    {
        private static HashSet<ISpawnCondition> Conditions = new();

        static SpawnConditionManager()
        {
            Conditions.Add(ConditionDistanceToCenter.Instance);
            Conditions.Add(ConditionNearbyPlayersCarryItem.Instance);
            Conditions.Add(ConditionNearbyPlayersCarryValue.Instance);
            Conditions.Add(ConditionNearbyPlayersNoise.Instance);
            Conditions.Add(ConditionNotGlobalKeys.Instance);
            Conditions.Add(ConditionWorldAge.Instance);

            Conditions.Add(ConditionLoaderCLLC.ConditionWorldLevel);
        }

        public static bool Filter(SpawnSystem spawnSystem, SpawnSystem.SpawnData spawner)
        {
            var config = SpawnDataCache.Get(spawner)?.SpawnConfig;

            if(config is null)
            {
                return false;
            }

            return Conditions.Any(x =>
            {
                try
                {
                    return x?.ShouldFilter(spawnSystem, spawner, config) ?? false;
                }
                catch (Exception e)
                {
                    Log.LogError($"Error while attempting to check spawn condition {x.GetType().Name}.", e);
                    return false;
                }
            });
        }
    }
}
