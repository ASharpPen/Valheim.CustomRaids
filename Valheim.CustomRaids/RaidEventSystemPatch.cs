using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.CustomRaids
{
    [HarmonyPatch(typeof(RandEventSystem), "Start")]
    public static class RaidEventSystemPatch
    {
        private static void Postfix(ref RandEventSystem __instance)
        {
            Debug.Log("Starting RandEventSystem");

            var events = __instance.m_events;

            __instance.m_eventIntervalMin = CustomRaidPlugin.EventSystemConfig.EventCheckInterval.Value;
            __instance.m_eventChance = CustomRaidPlugin.EventSystemConfig.EventTriggerChance.Value;

            WriteToFile(events, true);

            Debug.Log("Removing default raids.");
            __instance.m_events.Clear();

            foreach(var raid in CustomRaidPlugin.RaidConfigurations)
            {
                if(!raid.Enabled.Value)
                {
                    continue;
                }

                Debug.Log($"Adding raid '{raid.Name}' to possible raids");

                __instance.m_events.Add(CreateEvent(raid));
            }
        }

        private static RandomEvent CreateEvent(RaidEventConfiguration raidEvent)
        {
            var spawnList = new List<SpawnSystem.SpawnData>();

            foreach(var spawnConfig in raidEvent.SpawnConfigurations)
            {
                var spawnObject = ZNetScene.instance.GetPrefab(spawnConfig.PrefabName.Value);

                if(spawnObject is null)
                {
                    Debug.LogWarning($"Unable to find spawn {spawnConfig.PrefabName.Value}");
                    continue;
                }

                SpawnSystem.SpawnData spawn = new SpawnSystem.SpawnData
                {
                    m_enabled = spawnConfig.Enabled.Value,
                    m_prefab = spawnObject,
                    m_maxSpawned = spawnConfig.MaxSpawned.Value,
                    m_spawnInterval = spawnConfig.SpawnInterval.Value,
                    m_spawnDistance = spawnConfig.SpawnDistance.Value,
                    m_spawnRadiusMin = spawnConfig.SpawnRadiusMin.Value,
                    m_spawnRadiusMax = spawnConfig.SpawnRadiusMax.Value,
                    m_groupSizeMin = spawnConfig.GroupSizeMin.Value,
                    m_groupSizeMax = spawnConfig.GroupSizeMax.Value,
                    m_huntPlayer = spawnConfig.HuntPlayer.Value,
                    m_maxLevel = spawnConfig.MaxLevel.Value,
                    m_minLevel = spawnConfig.MinLevel.Value,
                    m_biome = Heightmap.Biome.BiomesMax,
                };

                Debug.Log($"Adding {spawnConfig.Name} to {raidEvent.Name}");

                spawnList.Add(spawn);
            }

            RandomEvent newEvent = new RandomEvent
            {
                m_biome = (Heightmap.Biome)raidEvent.Biome.Value,
                m_name = raidEvent.Name.Value,
                m_duration = raidEvent.Duration.Value,
                m_forceEnvironment = raidEvent.ForceEnvironment.Value,
                m_forceMusic = raidEvent.ForceMusic.Value,
                m_nearBaseOnly = raidEvent.NearBaseOnly.Value,
                m_startMessage = raidEvent.StartMessage.Value,
                m_endMessage = raidEvent.EndMessage.Value,
                m_enabled = raidEvent.Enabled.Value,
                m_random = true,
                m_spawn = spawnList,
                m_notRequiredGlobalKeys = new List<string>(),
                m_requiredGlobalKeys = new List<string>()
            };

            return newEvent;
        }

        public static void WriteToFile(List<RandomEvent> events, bool debug)
        {
            string filePath = Path.GetFullPath(@".\random_events.txt");
            if (debug) Debug.Log($"Writing default random events to '{filePath}'.");

            var eventFields = typeof(RandomEvent).GetFields();
            var spawnFields = typeof(SpawnSystem.SpawnData).GetFields();

            List<string> lines = new List<string>(events.Count * eventFields.Length);

            foreach (var item in events)
            {
                lines.Add("");
                lines.Add("[Event]");

                foreach (var field in eventFields)
                {
                    lines.Add($"{field.Name}: {field.GetValue(item)}");
                }

                lines.Add("");
                foreach(var spawn in item.m_spawn)
                {
                    lines.Add("[Spawn]");

                    foreach(var field in spawnFields)
                    {
                        lines.Add($"{field.Name}: {field.GetValue(spawn)}");
                    }
                }
            }
            File.WriteAllLines(filePath, lines);
        }
    }
}
