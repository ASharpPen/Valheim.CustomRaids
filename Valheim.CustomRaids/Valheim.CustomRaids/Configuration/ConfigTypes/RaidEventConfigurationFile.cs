using System;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Configuration.ConfigTypes
{
    [Serializable]
    public class RaidEventConfigurationFile : ConfigWithSubsections<RaidEventConfiguration>, IConfigFile
    {
        protected override RaidEventConfiguration InstantiateSubsection(string subsectionName)
        {
            return new RaidEventConfiguration();
        }
    }

    [Serializable]
    public class RaidEventConfiguration : ConfigWithSubsections<SpawnConfiguration>
    {
        protected override SpawnConfiguration InstantiateSubsection(string subsectionName)
        {
            return new SpawnConfiguration();
        }

        public ConfigurationEntry<string> Name = new("Deer Army", "Name of event. Can be used to override existing raid with same name.");

        public ConfigurationEntry<bool> Enabled = new(true, "Enable/disable raid.");

        public ConfigurationEntry<bool> Random = new(true, "Sets whether event is random or not. Eg. boss fights are events, but not random.");

        public ConfigurationEntry<string> Biomes = new("", "List of biomes required to allow spawn.\nLeave empty for all");

        public ConfigurationEntry<float> Duration = new(60, "Duration of raid in seconds.");

        public ConfigurationEntry<string> StartMessage = new("Raid start", "Message shown on raid start.");

        public ConfigurationEntry<string> EndMessage = new("Raid done", "Message shown on raid end");

        public ConfigurationEntry<bool> NearBaseOnly = new(true, "Spawn raid near base only.");

        public ConfigurationEntry<string> NotRequiredGlobalKeys = new("", "List of global keys disabling raid");

        public ConfigurationEntry<string> RequiredGlobalKeys = new("", "List of required global keys. Leave empty for no requirement");

        public ConfigurationEntry<string> RequireOneOfGlobalKeys = new("", "List of global keys of which one is required. Leave empty for no requirement.");

        public ConfigurationEntry<bool> PauseIfNoPlayerInArea = new(true, "Raid timer freezes when no players are nearby.");

        public ConfigurationEntry<string> ForceEnvironment = new("", "Name of weather/environment to set during raid.");

        public ConfigurationEntry<string> ForceMusic = new("CombatEventL1", "Name of music to play during raid.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMin = new(0, "Minimum number of in-game days of the world, for this raid to be possible.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMax = new(0, "Maximum number of in-game days of the world, for this raid to be possible. 0 means no limit");

        public ConfigurationEntry<bool> CanStartDuringDay = new (true, "Raid can start during daytime.");

        public ConfigurationEntry<bool> CanStartDuringNight = new(true, "Raid can start during nighttime.");

        public ConfigurationEntry<string> Faction = new("Boss", "Assign a single faction to all entities in raid. See readme for faction options.");

        public ConfigurationEntry<float> RaidFrequency = new (0, "Minutes between checks for this raid to run. 0 uses game default (46 minutes). This is only used if UseIndividualRaidChecks is set in general config.");

        public ConfigurationEntry<float> RaidChance = new(0, "Chance at each check for this raid to run. 0 uses game default (20%). This is only used if UseIndividualRaidChecks is set in general config.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMin = new(0, "Minimum distance to center for this raid to activate.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMax = new(0, "Maximum distance to center for this raid to activate. 0 means limitless.");

        public ConfigurationEntry<int> ConditionPlayersNearbyMin = new(0, "Minimum players in area for raid to activate.");

        public ConfigurationEntry<int> ConditionPlayersNearbyMax = new(0, "Maximum players in area for raid to activate. 0 means no limit.");

        public ConfigurationEntry<int> ConditionPlayersOnlineMin = new(0, "Minimum players online for raid to activate.");

        public ConfigurationEntry<int> ConditionPlayersOnlineMax = new(0, "Maximum players online for raid to activate. 0 means no limit.");

        public ConfigurationEntry<int> ConditionAltitudeMin = new(-1000, "Minimum altitude (distance to water surface) for raid to activate. Altitude is calculated when on dedicated server, so it might be a bit off.");

        public ConfigurationEntry<int> ConditionAltitudeMax = new(1000, "Maximum altitude (distance to water surface) for raid to activate. Altitude is calculated when on dedicated server, so it might be a bit off.");

        public ConfigurationEntry<string> ConditionEnvironment = new("", "List of environments enabling raid. Environment to compare to when on dedicated server is calculated, so it might be a bit off. Leave empty for no requirement.");

        public ConfigurationEntry<string> ConditionMustBeNearPrefab = new("", "List of prefab names enabling raid when nearby. A single prefab in the list must be near. Leave empty for no requirements.");
        public ConfigurationEntry<int> ConditionMustBeNearPrefabDistance = new(100, "Radius from raid center used for ConditionMustBeNearPrefab.");

        public ConfigurationEntry<string> ConditionMustBeNearAllPrefabs = new("", "List of prefab names enabling raid when all are nearby. If any prefab in the list is not nearby, raid will not be started. Leave empty for no requirements.");
        public ConfigurationEntry<int> ConditionMustBeNearAllPrefabsDistance = new(100, "Radius from raid center used for ConditionMustBeNearAllPrefabs.");

        public ConfigurationEntry<string> ConditionMustNotBeNearPrefab = new("", "List of prefab names disabling raid when nearby. None of the listed prefab can be near. Leave empty for no requirements.");
        public ConfigurationEntry<int> ConditionMustNotBeNearPrefabDistance = new(100, "Radius from raid center used for ConditionMustNotBeNearPrefab.");

        public ConfigurationEntry<string> OnStopStartRaid = new("", "When this event stops, start a new raid at the same position, with the given name. Note, this will ignore all conditions otherwise required for that raid.\nThis is intended for chaining raids.");
    }

    [Serializable]
    public class SpawnConfiguration : ConfigWithSubsections<Config>
    {
        protected override Config InstantiateSubsection(string subsectionName)
        {
            Config newModConfig = null;

            if (subsectionName == SpawnConfigCLLC.ModName.Trim().ToUpperInvariant())
            {
                newModConfig = new SpawnConfigCLLC();
            }
            else if (subsectionName == SpawnConfigSpawnThat.ModName.Trim().ToUpperInvariant())
            {
                newModConfig = new SpawnConfigSpawnThat();
            }

            return newModConfig;
        }

        [NonSerialized]
        private int? index = null;

        public int Index
        {
            get
            {
                if (index.HasValue)
                {
                    return index.Value;
                }

                if (int.TryParse(SectionName, out int sectionIndex) && sectionIndex >= 0)
                {
                    index = sectionIndex;
                }
                else
                {
                    index = int.MaxValue;
                }

                return index.Value;
            }
        }

        public ConfigurationEntry<string> Name = new("DeerALot", "Spawn configuration name");

        public ConfigurationEntry<bool> Enabled = new(true, "Enable/disable this entry.");

        public ConfigurationEntry<string> Biomes = new("", "Biomes in which entity can spawn. Leave empty for all.");

        //public ConfigurationEntry<bool> DriveInward = new ConfigurationEntry<bool>(false, "Mobs always spawn towards the world edge from player.");

        //Bound to the spawner itself. Need to transpile in a change for this to work.
        //public ConfigurationEntry<float> LevelUpChance = new ConfigurationEntry<float>(10, "Chance to increase level above min. This is run multiple times. 100 is 100%.\nEg. if Chance is 10, LevelMin is 1 and LevelMax is 3, the game will have a 10% to become level 2. The game will then run an additional 10% check for increasing to level 3.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMin = new(0, "Minimum distance to center of map for entity to spawn.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMax = new(0, "Maximum distance to center of map for entity to spawn. 0 means limitless.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMin = new(0, "Minimum world age in in-game days for this entity to spawn.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMax = new(0, "Maximum world age in in-game days for this entity to spawn. 0 means no max.");

        public ConfigurationEntry<float> DistanceToTriggerPlayerConditions = new(100, "Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue.");

        public ConfigurationEntry<int> ConditionNearbyPlayersCarryValue = new(0, "Checks if nearby players have a combined value in inventory above this condition.\nEg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined.");

        public ConfigurationEntry<string> ConditionNearbyPlayerCarriesItem = new("", "Checks if nearby players have any of the listed item prefab names in inventory.\nEg. IronScrap, DragonEgg");

        public ConfigurationEntry<float> ConditionNearbyPlayersNoiseThreshold = new(0, "Checks if any nearby players are emitting noise at or above the threshold.");

        public ConfigurationEntry<string> RequiredNotGlobalKey = new("", "Global keys which disable spawning.\nEg. defeated_bonemass,KilledTroll");

        public ConfigurationEntry<string> Faction = new("", "Set a single custom faction for mob. This overrules the raids faction setting if set.");

        #region Default Configuration Options

        public ConfigurationEntry<string> PrefabName = new("Deer", "Prefab name of the entity to spawn.");

        public ConfigurationEntry<bool> HuntPlayer = new ConfigurationEntry<bool>(false, "Sets AI to hunt a player target.");

        public ConfigurationEntry<int> MaxSpawned = new(1, "Maximum number of prefab spawned in local surroundings.");

        public ConfigurationEntry<float> SpawnInterval = new(1, "Seconds between new spawn checks.");

        public ConfigurationEntry<float> SpawnChancePerInterval = new(100, "Chance (0 to 100) to spawn new wave per check.");

        public ConfigurationEntry<int> MinLevel = new(1, "Minimum level to spawn (2 is one star).");

        public ConfigurationEntry<int> MaxLevel = new(1, "Maximum level to spawn (2 is one star).");

        public ConfigurationEntry<float> LevelUpMinCenterDistance = new ConfigurationEntry<float>(0, "Minimum distance from world center, to allow higher than min level.");

        public ConfigurationEntry<float> SpawnDistance = new(0, "Must not have another spawn of same prefab within this distance for this template to spawn.");

        public ConfigurationEntry<float> SpawnRadiusMin = new(40, "Minimum spawn distance from player. 0 defaults to 40");

        public ConfigurationEntry<float> SpawnRadiusMax = new(80, "Maximum spawn distance from player. 0 defaults to 80");

        public ConfigurationEntry<string> RequiredGlobalKey = new("", "Required global key to spawn.\nEg. defeated_bonemass");

        public ConfigurationEntry<string> RequiredEnvironments = new("", "List of environments enabling spawn in.\tEg. Misty, Thunderstorm. Leave empty to allow all.");

        public ConfigurationEntry<int> GroupSizeMin = new(1, "Minimum count to spawn at a time. Group spawning.");

        public ConfigurationEntry<int> GroupSizeMax = new(1, "Maximum count to spawn at a time. Group spawning.");

        public ConfigurationEntry<float> GroupRadius = new(3, "Size of circle to spawn group inside. A spot within SpawnRadiusMin and SpawnRadiusMax will be picked as center of this circle.");

        public ConfigurationEntry<float> GroundOffset = new(0.5f, "Offset to ground to spawn at. Negative means below ground, the higher the further into the sky.");

        public ConfigurationEntry<bool> SpawnAtDay = new(true, "Toggles spawning at day. Will also cause despawning for creatures at day if false.");

        public ConfigurationEntry<bool> SpawnAtNight = new(true, "Can spawn at night.");

        public ConfigurationEntry<float> AltitudeMin = new(-1000, "Minimum altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> AltitudeMax = new(1000, "Maximum altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> TerrainTiltMin = new(0, "Minimum tilt of terrain to spawn in.");

        public ConfigurationEntry<float> TerrainTiltMax = new(35, "Maximum tilt of terrain to spawn in.");

        public ConfigurationEntry<bool> InForest = new(true, "Toggles spawning in forest.");

        public ConfigurationEntry<bool> OutsideForest = new(true, "Toggles spawning outside of forest.");

        public ConfigurationEntry<float> OceanDepthMin = new(0, "Minimum ocean depth to spawn in. Ignored if min == max.");

        public ConfigurationEntry<float> OceanDepthMax = new(0, "Maximum ocean depth to spawn in. Ignored if min == max.");

        public ConfigurationEntry<float> RotationX = new(0, "Rotate the spawned object on the x axis. Defaults to 0");

        public ConfigurationEntry<float> RotationY = new(0, "Rotate the spawned object on the y axis. Defaults to 0");

        public ConfigurationEntry<float> RotationZ = new(0, "Rotate the spawned object on the z axis. Defaults to 0");
        #endregion
    }

    [Serializable]
    public class SpawnConfigCLLC : Config
    {
        public const string ModName = "CreatureLevelAndLootControl";

        public ConfigurationEntry<int> ConditionWorldLevelMin = new ConfigurationEntry<int>(-1, "Minimum CLLC world level for spawn to activate. Negative value disables this condition.");

        public ConfigurationEntry<int> ConditionWorldLevelMax = new ConfigurationEntry<int>(-1, "Maximum CLLC world level for spawn to active. Negative value disables this condition.");

        public ConfigurationEntry<string> SetInfusion = new ConfigurationEntry<string>("", "Assigns the specified infusion to creature spawned. Ignored if empty.");

        public ConfigurationEntry<string> SetExtraEffect = new ConfigurationEntry<string>("", "Assigns the specified effect to creature spawned. Ignored if empty.");

        public ConfigurationEntry<string> SetBossAffix = new ConfigurationEntry<string>("", "Assigns the specified boss affix to spawned boss. Ignored if empty.");

        public ConfigurationEntry<bool> UseDefaultLevels = new ConfigurationEntry<bool>(false, "Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control.");
    }

    [Serializable]
    public class SpawnConfigSpawnThat : Config
    {
        public const string ModName = "SpawnThat";

        public ConfigurationEntry<string> TemplateId = new ConfigurationEntry<string>("", "Technical setting intended for cross-mod identification of mobs spawned by this config entry. Sets a custom identifier which will be assigned to the spawned mobs ZDO as 'ZDO.Set(\"spawn_template_id\", TemplateIdentifier)'.");

        public ConfigurationEntry<bool> SetRelentless = new ConfigurationEntry<bool>(false, "When true, forces mob AI to always be alerted.");

        public ConfigurationEntry<bool> SetTryDespawnOnAlert = new ConfigurationEntry<bool>(false, "When true, mob will try to run away and despawn when alerted.");
    }
}
