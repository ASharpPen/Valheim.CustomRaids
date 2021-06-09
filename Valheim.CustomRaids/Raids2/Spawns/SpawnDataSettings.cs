using System.Collections.Generic;

namespace Valheim.CustomRaids.Raids2.Spawns
{
    /// <summary>
    /// Settings for SpawnSystem.SpawnData
    /// </summary>
    public class SpawnDataSettings
    {
        /// <summary>
        /// m_biome
        /// </summary>
        public Heightmap.Biome? ConditionBiome { get; set; }

        /// <summary>
        /// Not sure how this is used yet.
        /// m_biomeArea
        /// </summary>
        public Heightmap.BiomeArea? ConditionBiomeArea { get; set; }

        /// <summary>
        /// m_enabled
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// m_groundOffset
        /// </summary>
        public float? GroundOffset { get; set; }

        /// <summary>
        /// m_groupRadius
        /// </summary>
        public float? GroupSpawnRadius { get; set; }

        /// <summary>
        /// m_groupSizeMin
        /// </summary>
        public int? GroupSizeMin { get; set; }

        /// <summary>
        /// m_groupSizeMax
        /// </summary>
        public int? GroupSizeMax { get; set; }

        /// <summary>
        /// m_huntPlayer
        /// </summary>
        public bool? ModifierHuntPlayer { get; set; }

        /// <summary>
        /// m_inForest
        /// </summary>
        public bool? ConditionInForest { get; set; }

        /// <summary>
        /// m_levelUpMinCenterDistance
        /// </summary>
        public float? LevelUpMinCenterDistance { get; set; }

        /// <summary>
        /// m_maxAltitude
        /// </summary>
        public float? ConditionAltitudeMax { get; set; }

        /// <summary>
        /// m_maxLevel
        /// </summary>
        public int? ModifierLevelMax { get; set; }

        /// <summary>
        /// m_maxOceanDepth
        /// </summary>
        public float? ConditionOceanDepthMax { get; set; }

        /// <summary>
        /// m_maxSpawned
        /// </summary>
        public int? ConditionMaxSpawned { get; set; }

        /// <summary>
        /// m_maxTilt
        /// </summary>
        public float? ConditionTiltMax { get; set; }

        /// <summary>
        /// m_minAltitude
        /// </summary>
        public float? ConditionAltitudeMin { get; set; }

        /// <summary>
        /// m_minLevel
        /// </summary>
        public int? ModifierLevelMin { get; set; }

        /// <summary>
        /// m_minOceanDepth
        /// </summary>
        public float? ConditionOceanDepthMin { get; set; }

        /// <summary>
        /// m_minTilt
        /// </summary>
        public float? ConditionTiltMin { get; set; }

        /// <summary>
        /// m_name
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// m_outsideForest
        /// </summary>
        public bool? ConditionOutsideForest { get; set; }

        /// <summary>
        /// m_prefab
        /// 
        /// Note: Loaded in when needed.
        /// </summary>
        public string PrefabName { get; set; }

        /// <summary>
        /// m_requiredEnvironments
        /// </summary>
        public List<string> ConditionEnvironments { get; set; }

        /// <summary>
        /// m_requiredGlobalKey
        /// </summary>
        public string ConditionGlobalKey { get; set; }

        /// <summary>
        /// m_spawnAtDay
        /// </summary>
        public bool? ConditionDaytimeDay { get; set; }

        /// <summary>
        /// m_spawnAtNight
        /// </summary>
        public bool? ConditionDaytimeNight { get; set; }

        /// <summary>
        /// m_spawnChance
        /// </summary>
        public float? ConditionSpawnChance { get; set; }

        /// <summary>
        /// m_spawnDistance
        /// </summary>
        public float? ConditionSpawnDistance { get; set; }

        /// <summary>
        /// m_spawnRadiusMax
        /// </summary>
        public float? ConditionSpawnRadiusMax { get; set; }

        /// <summary>
        /// m_spawnRadiusMin
        /// </summary>
        public float? ConditionSpawnRadiusMin { get; set; }
    }
}
