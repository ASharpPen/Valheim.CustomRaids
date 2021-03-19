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
        private static MethodInfo CharacterComponentAnchor = AccessTools.Method(typeof(GameObject), "GetComponent", generics: new[] { typeof(Character) });
        private static MethodInfo ApplyFactionMethod = AccessTools.Method(typeof(OnSpawnFactionPatch), "ApplyFactionToRaidMobs", new[] { typeof(Character), typeof(SpawnSystem.SpawnData), typeof(bool) });

        [HarmonyPatch("Spawn")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AddSpawnedCreatureHook(IEnumerable<CodeInstruction> instructions)
        {
            var traverser = new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Callvirt, CharacterComponentAnchor))
                .Advance(2); //Skip the next line, where result is stored

            var loadCharacter = traverser.Instruction; //Get load character component instruction

            traverser
                .InsertAndAdvance(loadCharacter) //Load Character component
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1)) //Load SpawnData itself
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3)) //Load if eventSpawner bool
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, ApplyFactionMethod)); //Call new method

            var result = traverser.InstructionEnumeration();

            foreach(var instr in result)
            {
                Debug.Log(instr);
            }

            return result;
        }

        private static void ApplyFactionToRaidMobs(Character character, SpawnSystem.SpawnData spawner, bool isEventSpawner)
        {
            if(!isEventSpawner)
            {
                return;
            }

            var spawnerExtension = MatchSpawnerWithConfig(spawner);

            if (spawnerExtension == null || 
                (spawnerExtension.Item1 == null && spawnerExtension.Item2 == null))
            {
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

            var spawnerSectionName = spawner.m_name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

            if(spawnerSectionName.Length != 2)
            {
                return null;
            }

            foreach (var raidConfig in ConfigurationManager.RaidConfig)
            {
                if (raidConfig.Sections.TryGetValue(spawnerSectionName[1], out SpawnConfiguration spawnConfig))
                {
                    var cfg = new Tuple<RaidEventConfiguration, SpawnConfiguration>(raidConfig, spawnConfig);

                    configCache.Add(spawner, cfg);
                    return cfg;
                }
            }

            return null;
        }
    }
}
