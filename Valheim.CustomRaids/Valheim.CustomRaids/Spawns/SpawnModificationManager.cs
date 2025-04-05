using System.Collections.Generic;
using UnityEngine;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Spawns.Caches;
using Valheim.CustomRaids.Spawns.Modifiers;
using Valheim.CustomRaids.Spawns.Modifiers.General;
using Valheim.CustomRaids.Spawns.Modifiers.ModSpecific;

namespace Valheim.CustomRaids.Spawns
{
    public static class SpawnModificationManager
    {
        private static HashSet<ISpawnModifier> SpawnModifiers = new();

        static SpawnModificationManager()
        {
            SpawnModifiers.Add(SpawnModifierSetFaction.Instance);
            SpawnModifiers.Add(SpawnModifierRotatePrefab.Instance);

            SpawnModifiers.Add(SpawnModifierLoaderCLLC.BossAffix);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.ExtraEffect);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.Infusion);
            SpawnModifiers.Add(SpawnModifierLoaderCLLC.SetLevel);

            SpawnModifiers.Add(SpawnModifierLoaderSpawnThat.SetRelentless);
            SpawnModifiers.Add(SpawnModifierLoaderSpawnThat.SetTemplateId);
            SpawnModifiers.Add(SpawnModifierLoaderSpawnThat.SetTryDespawnOnAlert);
        }

        public static void ApplyModifiers(SpawnSystem spawnSystem, GameObject spawn, SpawnSystem.SpawnData spawner)
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
                    Config = spawnData.SpawnConfig,
                    RaidConfig = spawnData.RaidConfig
                });
            }
        }
    }
}
