using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.ConfigurationTypes;
using Valheim.CustomRaids.Patches;

namespace Valheim.CustomRaids
{
    [HarmonyPatch(typeof(RandEventSystem), "Start")]
    public static class RandEventSystemPatch
    {
        private static void Postfix(RandEventSystem __instance)
        {
            Log.LogDebug("Starting RandEventSystem");

            //If singleplayer, ZNet will not be initialized here.
            if (ZNet.instance == null)
            {
                Log.LogDebug("Loading configurations.");
                ConfigurationManager.LoadAllConfigurations();

                ApplyConfigurations();
            }
            else if (ZNet.instance.IsServer())
            {
                Log.LogDebug("Loading configurations.");
                ConfigurationManager.LoadAllConfigurations();

                ApplyConfigurations();
            }
        }

        public static void ApplyConfigurations()
        {
            Log.LogDebug("Applying configurations to RandEventSystem.");

            var __instance = RandEventSystem.instance;

            var events = __instance.m_events;

            __instance.m_eventIntervalMin = ConfigurationManager.GeneralConfig.EventCheckInterval.Value;
            __instance.m_eventChance = ConfigurationManager.GeneralConfig.EventTriggerChance.Value;

            if (ConfigurationManager.GeneralConfig.WriteDefaultEventDataToDisk.Value)
            {
                WriteToFile(events, ConfigurationManager.DebugOn);
            }

            if (ConfigurationManager.GeneralConfig.RemoveAllExistingRaids.Value)
            {
                Log.LogDebug("Removing default raids.");
                __instance.m_events.Clear();
            }

            Log.LogDebug($"Found {ConfigurationManager.RaidConfig.Count} raid configurations to apply.");

            foreach (var raid in ConfigurationManager.RaidConfig)
            {
                if (ConfigurationManager.GeneralConfig.OverrideExisting.Value)
                {
                    //Check for overrides
                    if (__instance.m_events.Count > 0)
                    {
                        for (int i = 0; i < __instance.m_events.Count; ++i)
                        {
                            string cleanedEventName = __instance.m_events[i].m_name.ToUpperInvariant().Trim();
                            string cleanedRaidName = raid.Name.Value.ToUpperInvariant().Trim();
                            if (cleanedEventName == cleanedRaidName)
                            {
                                Log.LogDebug($"Overriding existing event {__instance.m_events[i].m_name} with configured");
                                __instance.m_events.RemoveAt(i);
                                break;
                            }
                        }
                    }
                }

                if (!raid.Enabled.Value)
                {
                    continue;
                }

                Log.LogDebug($"Adding raid '{raid.Name}' to possible raids");

                try
                {
                    var randomEvent = CreateEvent(raid);
                    RandomEventCache.Initialize(randomEvent, raid);

                    __instance.m_events.Add(randomEvent);
                }
                catch (Exception e)
                {
                    Log.LogWarning($"Failed to create possible raid {raid.Name}: " + e.Message);

                    if (ConfigurationManager.DebugOn) Debug.LogException(e);
                }
            }

            if (ConfigurationManager.GeneralConfig.WritePostChangeEventDataToDisk.Value)
            {
                WriteToFile(__instance.m_events, ConfigurationManager.DebugOn, "custom_random_events.txt");
            }
        }

        private static Heightmap.Biome GetBiome(RaidEventConfiguration config)
        {
            var biomeString = config.Biomes.Value;

            if(string.IsNullOrEmpty(biomeString))
            {
                return (Heightmap.Biome)1023;
            }

            List<Heightmap.Biome> biomes = new List<Heightmap.Biome>();

            var biomeRequest = Heightmap.Biome.None;

            foreach (var biome in biomeString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if(Enum.TryParse(biome.Trim(), true, out Heightmap.Biome biomeFlag))
                {
                    biomes.Add(biomeFlag);

                    biomeRequest = biomeRequest | biomeFlag;
                }
                else
                {
                    Log.LogWarning($"Unable to parse biome '{biome}'");
                }
            }

            return biomeRequest;
        }

        private static RandomEvent CreateEvent(RaidEventConfiguration raidEvent)
        {
            var spawnList = new List<SpawnSystem.SpawnData>();

            foreach(var spawnConfig in raidEvent.SpawnConfigurations)
            {
                var spawnObject = ZNetScene.instance.GetPrefab(spawnConfig.PrefabName.Value);

                if(spawnObject is null)
                {
                    Log.LogWarning($"Unable to find spawn {spawnConfig.PrefabName.Value}");
                    continue;
                }

                var requiredEnvironments = spawnConfig.RequiredEnvironments?.Value?.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries);

                SpawnSystem.SpawnData spawn = new SpawnSystem.SpawnData
                {
                    m_name = spawnConfig.Name.Value,
                    m_enabled = spawnConfig.Enabled.Value,
                    m_prefab = spawnObject,
                    m_maxSpawned = spawnConfig.MaxSpawned.Value,
                    m_spawnInterval = spawnConfig.SpawnInterval.Value,
                    m_spawnChance = spawnConfig.SpawnChancePerInterval.Value,
                    m_spawnDistance = spawnConfig.SpawnDistance.Value,
                    m_spawnRadiusMin = spawnConfig.SpawnRadiusMin.Value,
                    m_spawnRadiusMax = spawnConfig.SpawnRadiusMax.Value,
                    m_groupSizeMin = spawnConfig.GroupSizeMin.Value,
                    m_groupSizeMax = spawnConfig.GroupSizeMax.Value,
                    m_huntPlayer = spawnConfig.HuntPlayer.Value,
                    m_maxLevel = spawnConfig.MaxLevel.Value,
                    m_minLevel = spawnConfig.MinLevel.Value,
                    m_groundOffset = spawnConfig.GroundOffset.Value,
                    m_groupRadius = spawnConfig.GroupRadius.Value,
                    m_inForest = spawnConfig.InForest.Value,
                    m_maxAltitude = spawnConfig.AltitudeMax.Value,
                    m_minAltitude = spawnConfig.AltitudeMin.Value,
                    m_maxOceanDepth = spawnConfig.OceanDepthMax.Value,
                    m_minOceanDepth = spawnConfig.OceanDepthMin.Value,
                    m_minTilt = spawnConfig.TerrainTiltMin.Value,
                    m_maxTilt = spawnConfig.TerrainTiltMax.Value,
                    m_outsideForest = spawnConfig.OutsideForest.Value,
                    m_spawnAtDay = spawnConfig.SpawnAtDay.Value,
                    m_spawnAtNight = spawnConfig.SpawnAtNight.Value,
                    m_requiredGlobalKey = spawnConfig.RequiredGlobalKey.Value,
                    m_requiredEnvironments = requiredEnvironments?.ToList() ?? new List<string>(),
                    m_biome = (Heightmap.Biome)1023,
                    m_biomeArea = (Heightmap.BiomeArea)7,
                };

                Log.LogDebug($"Adding {spawnConfig.Name} to {raidEvent.Name}");

                spawnList.Add(spawn);
            }

            var notRequiredGlobalKeys = raidEvent.NotRequiredGlobalKeys?.Value?.Split(new []{','}, StringSplitOptions.RemoveEmptyEntries);
            var requiredGlobalKeys = raidEvent.RequiredGlobalKeys?.Value?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);



            RandomEvent newEvent = new RandomEvent
            {
                m_name = raidEvent.Name.Value,
                m_duration = raidEvent.Duration.Value,
                m_forceEnvironment = raidEvent.ForceEnvironment.Value,
                m_forceMusic = raidEvent.ForceMusic.Value,
                m_nearBaseOnly = raidEvent.NearBaseOnly.Value,
                m_startMessage = raidEvent.StartMessage.Value,
                m_endMessage = raidEvent.EndMessage.Value,
                m_enabled = true,
                m_random = raidEvent.Random.Value,
                m_spawn = spawnList,
                m_biome = GetBiome(raidEvent),
                m_notRequiredGlobalKeys = notRequiredGlobalKeys?.ToList() ?? new List<string>(),
                m_requiredGlobalKeys = requiredGlobalKeys?.ToList() ?? new List<string>(),
                m_pauseIfNoPlayerInArea = raidEvent.PauseIfNoPlayerInArea.Value,                
            };

            return newEvent;
        }

        public static void WriteToFile(List<RandomEvent> events, bool debug, string fileName = "default_random_events.txt")
        {
            string filePath = Path.Combine(Paths.PluginPath, fileName);
            Log.LogDebug($"Writing default random events to '{filePath}'.");

            List<string> lines = new List<string>(events.Count * 30);

            foreach (var item in events)
            {
                lines.Add("");
                lines.Add("[Event]");

                Scan(item, lines);

                lines.Add("");
                foreach(var spawn in item.m_spawn)
                {
                    lines.Add("[Spawn]");

                    Scan(spawn, lines);
                }
            }
            File.WriteAllLines(filePath, lines);
        }

        private static void Scan(object obj, List<string> results, int depth = 1)
        {
            var fields = obj.GetType().GetFields();

            string indent = "";
            for (int i = 0; i < depth; ++i)
            {
                indent += "\t";
            }

            foreach (var field in fields)
            {
                if (typeof(List<string>).IsAssignableFrom(field.FieldType))
                {
                    results.Add($"{indent}{field.Name}:");

                    var indent2 = indent + "\t";
                    foreach(var str in field.GetValue(obj) as List<string>)
                    {
                        results.Add($"{indent2}{str}");
                    }
                }
                else
                {
                    results.Add($"{indent}{field.Name}: {field.GetValue(obj)}");
                }
            }
        }
    }
}
