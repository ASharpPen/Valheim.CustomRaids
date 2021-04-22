using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valheim.CustomRaids.ConfigurationTypes;

namespace Valheim.CustomRaids.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class OnSpawnFactionPatch
    {
        private static MethodInfo ApplyFactionMethod = AccessTools.Method(typeof(OnSpawnFactionPatch), nameof(OnSpawnFactionPatch.ApplyFactionToRaidMobs), new[] { typeof(GameObject), typeof(SpawnSystem.SpawnData), typeof(bool) });

        [HarmonyPatch("Spawn")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AddSpawnedCreatureHook(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Stloc_0))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, ApplyFactionMethod))
                .InstructionEnumeration();
        }

        private static void ApplyFactionToRaidMobs(GameObject spawn, SpawnSystem.SpawnData spawner, bool isEventSpawner)
        {
            if (!isEventSpawner)
            {
                return;
            }
#if DEBUG
            Log.LogDebug($"Attempting to apply faction for spawner {spawner.m_prefab.name}");
#endif

            Character character = spawn.GetComponent<Character>();

            if(!character)
            {
#if DEBUG
                Log.LogDebug($"No character component for spawned object '{spawner.m_prefab.name}'");
#endif
                return;
            }

            var spawnerExtension = MatchSpawnerWithConfig(spawner);

            if (spawnerExtension == null || 
                (spawnerExtension.Item1 == null && spawnerExtension.Item2 == null))
            {
#if DEBUG
                Log.LogDebug($"Unable to find config for spawner {spawner.m_prefab.name}");
#endif
                return;
            }

            var factionName = string.IsNullOrWhiteSpace(spawnerExtension.Item1?.Faction?.Value)
                ? spawnerExtension.Item2?.Faction.Value
                : spawnerExtension.Item1?.Faction.Value;

            Character.Faction creatureFaction = Character.Faction.Boss;

            if(Enum.TryParse(factionName.Trim(), out Character.Faction faction))
            {
                creatureFaction = faction;
            }
            else
            {
                Log.LogWarning($"Failed to parse faction '{factionName}'");
            }

#if DEBUG
            Log.LogDebug($"Setting faction {creatureFaction} for spawn {spawner.m_name}");
#endif

            character.m_faction = creatureFaction;
        }

        private static ConditionalWeakTable<SpawnSystem.SpawnData, Tuple<RaidEventConfiguration, SpawnConfiguration>> configCache = 
            new ConditionalWeakTable<SpawnSystem.SpawnData, Tuple<RaidEventConfiguration, SpawnConfiguration>>();

        private static Tuple<RaidEventConfiguration, SpawnConfiguration> MatchSpawnerWithConfig(SpawnSystem.SpawnData spawner)
        {
            if(configCache.TryGetValue(spawner, out Tuple<RaidEventConfiguration, SpawnConfiguration> config))
            {
                return config;
            }

            var randomEvent = RandEventSystem.instance.GetCurrentRandomEvent();
            var raidConfig = RandomEventCache.GetConfig(randomEvent);

            if (raidConfig is null)
            {
#if DEBUG
                Log.LogDebug($"Found no config to use for factiona assignment, for spawner {spawner.m_name}");
#endif
                return null;
            }

            var spawnerSectionName = spawner.m_name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if(spawnerSectionName.Length != 2)
            {
                return null;
            }

            if (raidConfig.Sections.TryGetValue(spawnerSectionName[1], out SpawnConfiguration spawnConfig))
            {
                var cfg = new Tuple<RaidEventConfiguration, SpawnConfiguration>(raidConfig, spawnConfig);

                configCache.Add(spawner, cfg);
                return cfg;
            }

            return null;
        }
    }
}
