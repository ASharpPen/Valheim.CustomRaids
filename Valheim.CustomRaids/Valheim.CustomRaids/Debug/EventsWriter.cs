using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities;

namespace Valheim.CustomRaids.Debug;

[HarmonyPatch]
public static class EventsWriter
{
    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.SetupLocations))]
    [HarmonyPrefix]
    public static void WriteLocationEvents()
    {
        try
        {
            WriteToFile(LocationList.GetAllLocationLists().SelectMany(x => x.m_events).ToList(), "default_location_random_events.txt", "random events (raids) for specific locations, eg. caves,");
        }
        catch(Exception e)
        {
            Log.LogWarning("Error during attempt at writing default location specific random events to file.", e);
        }
    }

    public static void WriteToFile(List<RandomEvent> events, string fileName = "default_random_events.txt", string logDescription = "random events (raids)")
    {
        try
        {
            string filePath = FileUtils.PrepareWriteDebugFile(fileName, logDescription);

            List<string> lines = new List<string>(events.Count * 30);

            foreach (var entry in events)
            {
                lines.Add($"[{entry.m_name}]");
                lines.Add($"{nameof(RaidEventConfiguration.Name)}={entry.m_name}");
                lines.Add($"{nameof(RaidEventConfiguration.Enabled)}={entry.m_enabled}");
                lines.Add($"{nameof(RaidEventConfiguration.Random)}={entry.m_random}");
                lines.Add($"{nameof(RaidEventConfiguration.Biomes)}={BiomeArray(entry.m_biome)}");
                lines.Add($"{nameof(RaidEventConfiguration.Duration)}={entry.m_duration.ToString(CultureInfo.InvariantCulture)}");
                lines.Add($"{nameof(RaidEventConfiguration.StartMessage)}={entry.m_startMessage}");
                lines.Add($"{nameof(RaidEventConfiguration.EndMessage)}={entry.m_endMessage}");
                lines.Add($"{nameof(RaidEventConfiguration.NearBaseOnly)}={entry.m_nearBaseOnly}");
                lines.Add($"{nameof(RaidEventConfiguration.RequiredGlobalKeys)}={entry.m_requiredGlobalKeys?.Join() ?? ""}");
                lines.Add($"{nameof(RaidEventConfiguration.NotRequiredGlobalKeys)}={entry.m_notRequiredGlobalKeys?.Join() ?? ""}");
                lines.Add($"{nameof(RaidEventConfiguration.PauseIfNoPlayerInArea)}={entry.m_pauseIfNoPlayerInArea}");
                lines.Add($"{nameof(RaidEventConfiguration.ForceEnvironment)}={entry.m_forceEnvironment}");
                lines.Add($"{nameof(RaidEventConfiguration.ForceMusic)}={entry.m_forceMusic}");

                lines.Add("");
                for (int i = 0; i < entry.m_spawn.Count; ++i)
                {
                    var spawner = entry.m_spawn[i];

                    string environmentArray = "";
                    if ((spawner.m_requiredEnvironments?.Count ?? 0) > 0)
                    {
                        environmentArray = spawner.m_requiredEnvironments.Join();
                    }

                    lines.Add($"[{entry.m_name}.{i}]");

                    lines.Add($"{nameof(SpawnConfiguration.Name)}={spawner.m_name}");
                    lines.Add($"{nameof(SpawnConfiguration.Enabled)}={spawner.m_enabled}");
                    lines.Add($"{nameof(SpawnConfiguration.PrefabName)}={spawner.m_prefab.name}");
                    lines.Add($"{nameof(SpawnConfiguration.HuntPlayer)}={spawner.m_huntPlayer}");
                    lines.Add($"{nameof(SpawnConfiguration.MaxSpawned)}={spawner.m_maxSpawned}");
                    lines.Add($"{nameof(SpawnConfiguration.SpawnInterval)}={spawner.m_spawnInterval.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.SpawnChancePerInterval)}={spawner.m_spawnChance.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.MinLevel)}={spawner.m_minLevel}");
                    lines.Add($"{nameof(SpawnConfiguration.MaxLevel)}={spawner.m_maxLevel}");
                    lines.Add($"{nameof(SpawnConfiguration.SpawnDistance)}={spawner.m_spawnDistance.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.SpawnRadiusMin)}={spawner.m_spawnRadiusMin.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.SpawnRadiusMax)}={spawner.m_spawnRadiusMax.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.RequiredGlobalKey)}={spawner.m_requiredGlobalKey}");
                    lines.Add($"{nameof(SpawnConfiguration.RequiredEnvironments)}={environmentArray}");
                    lines.Add($"{nameof(SpawnConfiguration.GroupSizeMin)}={spawner.m_groupSizeMin}");
                    lines.Add($"{nameof(SpawnConfiguration.GroupSizeMax)}={spawner.m_groupSizeMax}");
                    lines.Add($"{nameof(SpawnConfiguration.GroupRadius)}={spawner.m_groupRadius.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.GroundOffset)}={spawner.m_groundOffset.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.SpawnAtDay)}={spawner.m_spawnAtDay}");
                    lines.Add($"{nameof(SpawnConfiguration.SpawnAtNight)}={spawner.m_spawnAtNight}");
                    lines.Add($"{nameof(SpawnConfiguration.AltitudeMin)}={spawner.m_minAltitude.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.AltitudeMax)}={spawner.m_maxAltitude.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.TerrainTiltMin)}={spawner.m_minTilt.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.TerrainTiltMax)}={spawner.m_maxTilt.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.InForest)}={spawner.m_inForest}");
                    lines.Add($"{nameof(SpawnConfiguration.OutsideForest)}={spawner.m_outsideForest}");
                    lines.Add($"{nameof(SpawnConfiguration.OceanDepthMin)}={spawner.m_minOceanDepth.ToString(CultureInfo.InvariantCulture)}");
                    lines.Add($"{nameof(SpawnConfiguration.OceanDepthMax)}={spawner.m_maxOceanDepth.ToString(CultureInfo.InvariantCulture)}");

                    lines.Add("");
                }
            }
            File.WriteAllLines(filePath, lines);
        }
        catch (Exception e)
        {
            Log.LogWarning("Error while attempting to write raid events to file.", e);
        }
    }

    private static string BiomeArray(Heightmap.Biome spawnerBiome)
    {
        string biomeArray = "";
        foreach (var b in Enum.GetValues(typeof(Heightmap.Biome)))
        {
            if (b is Heightmap.Biome biome && biome != Heightmap.Biome.BiomesMax)
            {
                biome = (biome & spawnerBiome);

                if (biome > Heightmap.Biome.None)
                {
                    biomeArray += biome + ",";
                }
            }
        }

        return biomeArray;
    }
}
