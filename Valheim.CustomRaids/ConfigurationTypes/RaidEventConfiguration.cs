using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.ConfigurationCore;

namespace Valheim.CustomRaids
{
    [Serializable]
    public class RaidEventConfiguration : ConfigurationGroup<SpawnConfiguration>
    {
        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true, "Enable/disable raid configuration from being used.");

        public ConfigurationEntry<bool> Random = new ConfigurationEntry<bool>(true, "Sets whether event is random or not. Eg. boss fights are events, but not random.");

        public ConfigurationEntry<string> Biomes = new ConfigurationEntry<string>("", "Array (separate by \",\") of biomes required to allow spawn.\nLeave empty for all");

        public ConfigurationEntry<float> Duration = new ConfigurationEntry<float>(60, "Duration of raid in seconds.");

        public ConfigurationEntry<string> StartMessage = new ConfigurationEntry<string>("Raid start", "Raid start message");

        public ConfigurationEntry<string> EndMessage = new ConfigurationEntry<string>("Raid done", "Raid end message");

        public ConfigurationEntry<string> Name = new ConfigurationEntry<string>("Deer Army", "Raid name. Does not seem to hold any significant importance apart from being a unique identifier.");

        public ConfigurationEntry<bool> NearBaseOnly = new ConfigurationEntry<bool>(true, "Spawn raid near base only. Looks like this one might need to always be true due to the games valid spawn logic.");

        public ConfigurationEntry<string> NotRequiredGlobalKeys = new ConfigurationEntry<string>("", "Array (separate by \",\") of required global keys. Leave empty for no requirement.");

        public ConfigurationEntry<string> RequiredGlobalKeys = new ConfigurationEntry<string>("", "Array (separate by \",\") of required global keys. Leave empty for no requirement.");

        public ConfigurationEntry<string> RequireOneOfGlobalKeys = new ConfigurationEntry<string>("", "Array (separate by \",\") of global keys of which one is required. Leave empty for no requirement.");

        public ConfigurationEntry<bool> PauseIfNoPlayerInArea = new ConfigurationEntry<bool>(true);

        public ConfigurationEntry<string> ForceEnvironment = new ConfigurationEntry<string>("", "Environmental effect to set for raid.");

        public ConfigurationEntry<string> ForceMusic = new ConfigurationEntry<string>("CombatEventL1", "Music to play for raid.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMin = new ConfigurationEntry<float>(0, "Minimum number of in-game days of the world, for this raid to be possible.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMax = new ConfigurationEntry<float>(0, "Maximum number of in-game days of the world, for this raid to be possible. 0 means no limit");

        public ConfigurationEntry<bool> CanStartDuringDay = new ConfigurationEntry<bool>(true, "Enable/toggle this raid activating during day.");

        public ConfigurationEntry<bool> CanStartDuringNight = new ConfigurationEntry<bool>(true, "Enable/toggle this raid activating during night.");

        public ConfigurationEntry<string> Faction = new ConfigurationEntry<string>("Boss", "Assign a faction to all entities in raid. See readme for faction options.");

        public ConfigurationEntry<float> RaidFrequency = new ConfigurationEntry<float>(0, "Minutes between checks for this raid to run. 0 uses game default (46 minutes). This is only used if UseIndividualRaidChecks is set in general config.");

        public ConfigurationEntry<float> RaidChance = new ConfigurationEntry<float>(0, "Chance at each check for this raid to run. 0 uses game default (20%). This is only used if UseIndividualRaidChecks is set in general config.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMin = new ConfigurationEntry<float>(0, "Minimum distance to center for this raid to activate.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMax = new ConfigurationEntry<float>(0, "Maximum distance to center for this raid to activate. 0 means limitless.");

        public List<SpawnConfiguration> SpawnConfigurations => Sections.Values.ToList();
    }

    [Serializable]
    public class SpawnConfiguration : ConfigurationSection
    {
        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true);

        public ConfigurationEntry<string> Name = new ConfigurationEntry<string>("DeerALot", "Spawn configuration name.");

        public ConfigurationEntry<string> PrefabName = new ConfigurationEntry<string>("Deer", "Prefab name of entity to spawn. This can be any prefab.");

        public ConfigurationEntry<int> MaxSpawned = new ConfigurationEntry<int>(2, "Maximum entities of type spawned in area.");

       public ConfigurationEntry<float> SpawnInterval = new ConfigurationEntry<float>(1, "Interval (seconds) between wave checks.");

        public ConfigurationEntry<float> SpawnChancePerInterval = new ConfigurationEntry<float>(100, "Chance (0 to 100) to spawn new wave per check.");

        public ConfigurationEntry<float> SpawnDistance = new ConfigurationEntry<float>(1, "Minimum distance to another entity. Highly volatile setting.");

        public ConfigurationEntry<float> SpawnRadiusMin = new ConfigurationEntry<float>(1, "Minimum spawn radius. Highly volatile setting.");

        public ConfigurationEntry<float> SpawnRadiusMax = new ConfigurationEntry<float>(10, "Maximum spawn radius. Highly volatile setting.");

        public ConfigurationEntry<string> RequiredGlobalKey = new ConfigurationEntry<string>("", "Global key required for spawning. Leave empty for no requirement.");

        public ConfigurationEntry<string> RequiredEnvironments = new ConfigurationEntry<string>("", "Array (separate by \",\" of required environments. Leave empty for no requirement.");

        public ConfigurationEntry<float> GroupRadius = new ConfigurationEntry<float>(1, "Size of circle to spawn group inside.");

        public ConfigurationEntry<int> GroupSizeMin = new ConfigurationEntry<int>(1, "Minimum amount of spawns per wave.");

        public ConfigurationEntry<int> GroupSizeMax = new ConfigurationEntry<int>(1, "Maximum amount of spawns per wave.");

        public ConfigurationEntry<bool> SpawnAtNight = new ConfigurationEntry<bool>(true, "Can spawn at night.");
        
        public ConfigurationEntry<bool> SpawnAtDay = new ConfigurationEntry<bool>(true, "Can spawn at day.");

        public ConfigurationEntry<float> AltitudeMin = new ConfigurationEntry<float>(-1000, "Minimum required altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> AltitudeMax = new ConfigurationEntry<float>(1000, "Maximum required altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> TerrainTiltMin = new ConfigurationEntry<float>(0, "Minium required tilt of terrain to spawn in.");

        public ConfigurationEntry<float> TerrainTiltMax = new ConfigurationEntry<float>(35, "Maximum required tilt of terrain to spawn in.");

        public ConfigurationEntry<bool> InForest = new ConfigurationEntry<bool>(true, "Toggles spawning in forest.");

        public ConfigurationEntry<bool> OutsideForest = new ConfigurationEntry<bool>(true, "Toggles spawning outside of forest.");

        public ConfigurationEntry<float> OceanDepthMin = new ConfigurationEntry<float>(0, "Minimum required ocean depth to spawn in. Ignored if min == max.");

        public ConfigurationEntry<float> OceanDepthMax = new ConfigurationEntry<float>(0, "Maximum required ocean depth to spawn in. Ignored if min == max.");

        public ConfigurationEntry<bool> HuntPlayer = new ConfigurationEntry<bool>(true, "Does what it says. Will not work for all mobs, Deer will ignore it.");

        public ConfigurationEntry<int> MinLevel = new ConfigurationEntry<int>(1, "Min level of spawn. (2 is one star).");

        public ConfigurationEntry<int> MaxLevel = new ConfigurationEntry<int>(3, "Max level of spawn. (2 is one star).");

        public ConfigurationEntry<float> GroundOffset = new ConfigurationEntry<float>(0.5f, "Offset above ground at which entity will be spawned.");

        public ConfigurationEntry<string> Faction = new ConfigurationEntry<string>("", "Set custom faction for mob. This overrules the raids faction setting if set.");
    }
}
