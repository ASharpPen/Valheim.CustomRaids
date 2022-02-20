using System.Runtime.CompilerServices;
using Valheim.CustomRaids.Configuration.ConfigTypes;

namespace Valheim.CustomRaids.Spawns.Caches
{
    public class SpawnDataCache
    {
        private static ConditionalWeakTable<SpawnSystem.SpawnData, SpawnDataCache> SpawnDataTable = new();

        public static SpawnDataCache Get(SpawnSystem.SpawnData spawnData)
        {
            if (SpawnDataTable.TryGetValue(spawnData, out SpawnDataCache cache))
            {
                return cache;
            }

            return null;
        }

        public static SpawnDataCache GetOrCreate(SpawnSystem.SpawnData spawnData)
        {
            return SpawnDataTable.GetOrCreateValue(spawnData);
        }

        public SpawnDataCache SetSpawnConfig(SpawnConfiguration config)
        {
            SpawnConfig = config;
            return this;
        }

        public SpawnDataCache SetRaidConfig(RaidEventConfiguration config)
        {
            RaidConfig = config;
            return this;
        }

        public RaidEventConfiguration RaidConfig { get; set; }

        public SpawnConfiguration SpawnConfig { get; set; }
    }
}
