using System;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Raids2.Configs.BepInEx
{
    [Serializable]
    public class RaidConfig : ConfigWithSubsections<RaidConfig>, IConfigFile
    {
        protected override SpawnConfiguration InstantiateSubsection(string subsectionName)
        {
            return new SpawnConfiguration();
        }
    }

    [Serializable]
    public class SchedulerConfig : ConfigWithSubsections<Config>
    {
        protected override Config InstantiateSubsection(string subsectionName)
        {
        }
    }

    [Serializable]
    public class SchedulerConfigDefault : Config
    {

    }

    public class ConductorConfig : ConfigWithSubsections<Config>
    {

    }

    [Serializable]
    public class ConductorConfigDefault : Config
    {

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

        public ConfigurationEntry<string> Name = new ConfigurationEntry<string>("DeerALot", "Just a field for naming the configuration entry.");

        public ConfigurationEntry<bool> Enabled = new ConfigurationEntry<bool>(true, "Enable/disable this entry.");

        public ConfigurationEntry<string> Biomes = new ConfigurationEntry<string>("", "Biomes in which entity can spawn. Leave empty for all.");

        //public ConfigurationEntry<bool> DriveInward = new ConfigurationEntry<bool>(false, "Mobs always spawn towards the world edge from player.");

        //Bound to the spawner itself. Need to transpile in a change for this to work.
        //public ConfigurationEntry<float> LevelUpChance = new ConfigurationEntry<float>(10, "Chance to increase level above min. This is run multiple times. 100 is 100%.\nEg. if Chance is 10, LevelMin is 1 and LevelMax is 3, the game will have a 10% to become level 2. The game will then run an additional 10% check for increasing to level 3.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMin = new ConfigurationEntry<float>(0, "Minimum distance to center for entity to spawn.");

        public ConfigurationEntry<float> ConditionDistanceToCenterMax = new ConfigurationEntry<float>(0, "Maximum distance to center for entity to spawn. 0 means limitless.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMin = new ConfigurationEntry<float>(0, "Minimum world age in in-game days for this entity to spawn.");

        public ConfigurationEntry<float> ConditionWorldAgeDaysMax = new ConfigurationEntry<float>(0, "Maximum world age in in-game days for this entity to spawn. 0 means no max.");

        public ConfigurationEntry<float> DistanceToTriggerPlayerConditions = new ConfigurationEntry<float>(100, "Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue.");

        public ConfigurationEntry<int> ConditionNearbyPlayersCarryValue = new ConfigurationEntry<int>(0, "Checks if nearby players have a combined value in inventory above this condition.\nEg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined.");

        public ConfigurationEntry<string> ConditionNearbyPlayerCarriesItem = new ConfigurationEntry<string>("", "Checks if nearby players have any of the listed item prefab names in inventory.\nEg. IronScrap, DragonEgg");

        public ConfigurationEntry<float> ConditionNearbyPlayersNoiseThreshold = new ConfigurationEntry<float>(0, "Checks if any nearby players have accumulated noise at or above the threshold.");

        public ConfigurationEntry<string> RequiredNotGlobalKey = new ConfigurationEntry<string>("", "Array of global keys which disable the spawning of this entity if any are detected.\nEg. defeated_bonemass,KilledTroll");

        public ConfigurationEntry<string> Faction = new ConfigurationEntry<string>("", "Set custom faction for mob. This overrules the raids faction setting if set.");

        #region Default Configuration Options

        public ConfigurationEntry<string> PrefabName = new ConfigurationEntry<string>("Deer", "Prefab name of the entity to spawn.");

        public ConfigurationEntry<bool> HuntPlayer = new ConfigurationEntry<bool>(false, "Sets AI to hunt a player target.");

        public ConfigurationEntry<int> MaxSpawned = new ConfigurationEntry<int>(1, "Maximum entities of type spawned in area.");

        public ConfigurationEntry<float> SpawnInterval = new ConfigurationEntry<float>(1, "Interval (seconds) between wave checks.");

        public ConfigurationEntry<float> SpawnChancePerInterval = new ConfigurationEntry<float>(100, "Chance (0 to 100) to spawn new wave per check.");

        public ConfigurationEntry<int> MinLevel = new ConfigurationEntry<int>(1, "Minimum level to spawn.");

        public ConfigurationEntry<int> MaxLevel = new ConfigurationEntry<int>(1, "Maximum level to spawn.");

        public ConfigurationEntry<float> LevelUpMinCenterDistance = new ConfigurationEntry<float>(0, "Minimum distance from world center, to allow higher than min level.");

        public ConfigurationEntry<float> SpawnDistance = new ConfigurationEntry<float>(0, "Minimum distance to another entity.");

        public ConfigurationEntry<float> SpawnRadiusMin = new ConfigurationEntry<float>(0, "Minimum spawn radius.");

        public ConfigurationEntry<float> SpawnRadiusMax = new ConfigurationEntry<float>(0, "Maximum spawn radius.");

        public ConfigurationEntry<string> RequiredGlobalKey = new ConfigurationEntry<string>("", "Required global key to spawn.\nEg. defeated_bonemass");

        public ConfigurationEntry<string> RequiredEnvironments = new ConfigurationEntry<string>("", "Array (separate by comma) of environments required to spawn in.\tEg. Misty, Thunderstorm. Leave empty to allow all.");

        public ConfigurationEntry<int> GroupSizeMin = new ConfigurationEntry<int>(1, "Minimum count to spawn at per check.");

        public ConfigurationEntry<int> GroupSizeMax = new ConfigurationEntry<int>(1, "Maximum count to spawn at per check.");

        public ConfigurationEntry<float> GroupRadius = new ConfigurationEntry<float>(3, "Size of circle to spawn group inside.");

        public ConfigurationEntry<float> GroundOffset = new ConfigurationEntry<float>(0.5f, "Offset to ground to spawn at.");

        public ConfigurationEntry<bool> SpawnAtDay = new ConfigurationEntry<bool>(true, "Can spawn at day.");

        public ConfigurationEntry<bool> SpawnAtNight = new ConfigurationEntry<bool>(true, "Can spawn at night.");

        public ConfigurationEntry<float> AltitudeMin = new ConfigurationEntry<float>(-1000, "Minimum altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> AltitudeMax = new ConfigurationEntry<float>(1000, "Maximum altitude (distance to water surface) to spawn in.");

        public ConfigurationEntry<float> TerrainTiltMin = new ConfigurationEntry<float>(0, "Minium tilt of terrain to spawn in.");

        public ConfigurationEntry<float> TerrainTiltMax = new ConfigurationEntry<float>(35, "Maximum tilt of terrain to spawn in.");

        public ConfigurationEntry<bool> InForest = new ConfigurationEntry<bool>(true, "Toggles spawning in forest.");

        public ConfigurationEntry<bool> OutsideForest = new ConfigurationEntry<bool>(true, "Toggles spawning outside of forest.");

        public ConfigurationEntry<float> OceanDepthMin = new ConfigurationEntry<float>(0, "Minimum ocean depth to spawn in. Ignored if min == max.");

        public ConfigurationEntry<float> OceanDepthMax = new ConfigurationEntry<float>(0, "Maximum ocean depth to spawn in. Ignored if min == max.");

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
