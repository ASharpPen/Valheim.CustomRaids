using System.Collections.Generic;
using UnityEngine;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Spawns.Caches;
using Valheim.CustomRaids.Spawns.Modifiers;
using Valheim.CustomRaids.Spawns.Modifiers.ModSpecific;

namespace Valheim.CustomRaids.Spawns
{
    public class SpawnModificationManager
    {
        private HashSet<ISpawnModifier> SpawnModifiers = new();

        SpawnModificationManager()
        {
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.BossAffix);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.ExtraEffect);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.Infusion);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.SetLevel);
        }

        public void ApplyModifiers(SpawnSystem spawnSystem, GameObject spawn, SpawnSystem.SpawnData spawner)
        {
            if (!spawnSystem || spawnSystem is null || !spawn || spawn is null || spawner is null)
            {
                return;
            }

            var spawnData = SpawnDataCache.Get(spawner);

            if (spawnData is null || spawnData.SpawnConfig is null)
            {
                return;
            }

            Log.LogTrace($"Applying modifiers to spawn {spawn.name}");

            foreach (var modifier in SpawnModifiers)
            {
                modifier?.Modify(new SpawnContext
                {
                    SpawnSystem = spawnSystem,
                    Spawner = spawner,
                    Spawn = spawn,
                    Config = spawnData.SpawnConfig
                });
            }
        }
    }
}
