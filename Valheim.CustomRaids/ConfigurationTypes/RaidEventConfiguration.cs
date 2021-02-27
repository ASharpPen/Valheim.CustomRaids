using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.ConfigurationCore;

namespace Valheim.CustomRaids
{
    public class RaidEventConfiguration : ConfigurationGroup<SpawnConfiguration>
    {
        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true, "Enable/disable raid configuration from being used.");

        public ConfigurationEntry<int> Biome = new ConfigurationEntry<int>((int)Heightmap.Biome.BiomesMax, "Biome flag.");

        public ConfigurationEntry<float> Duration = new ConfigurationEntry<float>(60, "Duration of raid?");

        public ConfigurationEntry<string> StartMessage = new ConfigurationEntry<string>("Raid start", "Raid start message");

        public ConfigurationEntry<string> EndMessage = new ConfigurationEntry<string>("Raid done", "Raid end message");

        public ConfigurationEntry<string> Name = new ConfigurationEntry<string>("Deer Army", "Raid name. Does not seem to hold any significant importance apart from being a unique identifier.");

        public ConfigurationEntry<bool> NearBaseOnly = new ConfigurationEntry<bool>(true, "");

        /// <summary>
        /// Array of "global keys", whatever they might be.
        /// </summary>
        public ConfigurationEntry<string> NotRequiredGlobalKeys;

        /// <summary>
        /// Array of "global keys", whatever they might be.
        /// </summary>
        public ConfigurationEntry<string> RequiredGlobalKeys;

        public ConfigurationEntry<bool> PauseIfNoPlayerInArea = new ConfigurationEntry<bool>(true);

        /// <summary>
        /// Looks like environmental effects
        /// </summary>
        public ConfigurationEntry<string> ForceEnvironment = new ConfigurationEntry<string>("Ashrain", "Environmental effect to set for raid.");

        /// <summary>
        /// Predefined music id
        /// </summary>
        public ConfigurationEntry<string> ForceMusic = new ConfigurationEntry<string>("CombatEventL1", "Music to play for raid.");

        public List<SpawnConfiguration> SpawnConfigurations => Sections.Values.ToList();
    }

    public class SpawnConfiguration : ConfigurationSection
    {
        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true);

        public ConfigurationEntry<string> Name = new ConfigurationEntry<string>("DeerALot", "Spawn configuration name.");

        public ConfigurationEntry<string> PrefabName = new ConfigurationEntry<string>("Deer", "Prefab name of entity to spawn. This... might actually allow for anything.");

        /// <summary>
        /// Maybe?
        /// </summary>
        public ConfigurationEntry<string> Biome;

        /// <summary>
        /// Enum for where in area to allow spawn.
        /// </summary>
        public ConfigurationEntry<string> BiomeArea;

        public ConfigurationEntry<int> MaxSpawned = new ConfigurationEntry<int>(2, "Maximum alive at a time.");

        /// <summary>
        /// Seconds? Minutes? What is this
        /// </summary>
        public ConfigurationEntry<float> SpawnInterval = new ConfigurationEntry<float>(1, "Interval (seconds) between wave checks");

        /// <summary>
        /// 0 to 100
        /// </summary>
        public ConfigurationEntry<float> SpawnChancePerInterval = new ConfigurationEntry<float>(100, "Chance to spawn new wave per check.");

        /// <summary>
        /// Min distance to another... "instance"?
        /// </summary>
        public ConfigurationEntry<float> SpawnDistance = new ConfigurationEntry<float>(10, "Distance?");

        public ConfigurationEntry<float> SpawnRadiusMin = new ConfigurationEntry<float>(0, "");

        public ConfigurationEntry<float> SpawnRadiusMax = new ConfigurationEntry<float>(10, "");

        /// <summary>
        /// Only spawn if key is set. Where to find that though?
        /// </summary>
        public ConfigurationEntry<string> RequiredGlobalKey;

        /// <summary>
        /// This is a an array, not a single string. Just load it in as a string I guess, and split it into pieces.
        /// </summary>
        public ConfigurationEntry<string> RequiredEnvironment;

        /// <summary>
        /// Group spawning
        /// </summary>
        public ConfigurationEntry<int> GroupSizeMin = new ConfigurationEntry<int>(1, "Minimum amount of spawns per wave.");

        public ConfigurationEntry<int> GroupSizeMax = new ConfigurationEntry<int>(1, "Maximum amount of spawns per wave.");

        //public ConfigurationEntry<float> GroupSizeRadius = new ConfigurationEntry<float>(;

        public ConfigurationEntry<bool> SpawnAtNight = new ConfigurationEntry<bool>(true, "Can spawn at night.");
        
        public ConfigurationEntry<bool> SpawnAtDay = new ConfigurationEntry<bool>(true, "Can spawn at day");

        /// <summary>
        /// Not sure how distance is calculated here.
        /// </summary>
        public ConfigurationEntry<float> AltitudeMin;

        public ConfigurationEntry<float> AltitudeMax;

        /// <summary>
        /// Not sure how this is calculated. Maybe degrees?
        /// </summary>
        public ConfigurationEntry<float> TerrainTiltMin;

        public ConfigurationEntry<float> TerrainTiltMax;

        /// <summary>
        /// Requirement for spawning? Not sure
        /// </summary>
        public ConfigurationEntry<bool> InForest;

        /// <summary>
        /// Requirement for spawning? Not sure
        /// </summary>
        public ConfigurationEntry<bool> OutsideForest;

        /// <summary>
        /// Requirement for spawning? Not sure
        /// </summary>
        public ConfigurationEntry<float> OceanDepthMin;

        /// <summary>
        /// Requirement for spawning? Not sure
        /// </summary>
        public ConfigurationEntry<float> OceanDepthMax;

        /// <summary>
        /// Oooooh yeah!
        /// </summary>
        public ConfigurationEntry<bool> HuntPlayer = new ConfigurationEntry<bool>(true, "");

        public ConfigurationEntry<int> MinLevel = new ConfigurationEntry<int>(1, "");

        /// <summary>
        /// Hm... maybe we can increase level above 3?
        /// </summary>
        public ConfigurationEntry<int> MaxLevel = new ConfigurationEntry<int>(3, "Not sure how high this allows us to go.");

        public ConfigurationEntry<float> GroundOffset = new ConfigurationEntry<float>(0.5f, "Distance to ground on spawn.");
    }
}
