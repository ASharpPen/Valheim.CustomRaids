# Custom Raids

This mod allows for customizing raids to your liking.

Want to have armies besieging your fortress? A bit of extra environmental effects along your way? Or just disable trolls?

This mod can help you do all of that!

Comes preconfigured with two additional end-game raids (disabled by default), to show how and what can be done:
- Ragnarok
- Deathsquitoseason

Enable those raids at own risk!

# Features

- Can change frequency of raids
- Add new raid events, with full configuration options, including spawns
- Override existing raid (eg. disable trolls)
- Supplemental raid configurations. Add your own raid in its own file, and Custom Raids will scan and apply it. 
- Potential for hours of frustration/fun as you figure out how to best configure these damn things to work as expected.
- Server-side configurations
- Fixed bug with raids only being able to spawn near player bases.
- Support for [Creature Level and Loot Control](https://www.nexusmods.com/valheim/mods/495)

# FAQ

- Is it server side only?
	- No. Both client and server needs the mod.
	- The way events work is that the host / dedicated server will run the logic for when to apply raids, and then send a message with the name of the raid to be assigned to all clients. The client in charge of zone (valheim multiplayer is weird) will then manage the actual spawning for the raid.
- Can I just have no raids?
	- Yes. Raid activation happens serverside, making this even simpler, you should only need this mod serverside if you just want no raids. (You can also use Event Enhancer for this)
	- Set configuration "EventTriggerChance=0" or
	- Set configuration "RemoveAllExistingRaids=true", as long you don't have something custom, this will remove the options, thereby removing raids.

# Client / Server

Custom Raids needs to be installed on all clients (and server) to work.

From v1.2.0 clients will request the configurations currently loaded by the server, and use those without affecting the clients config files.
This means you should be able to have server-specific configurations, and the client can have its own setup for singleplayer.
For this to work, the mod needs to be installed on the server, and have configs set up properly there. When players join with Custom Raids v1.2.0, their mod will use the servers configs.

# Configuration

All configurations are placed in the default BepInEx configuration folder, and generated upon starting the game.

## General "custom_raids.cfg"

General configuration includes general mod controls, overall event system changes, and debugging options.

``` INI

[General]

## Loads raid configurations from supplemental files.
## Eg. custom_raid.supplemental.my_raid.cfg will be included on load.
LoadSupplementalRaids = true

## Generates pre-defined supplemental raids. The generated raids are disabled by default.
GeneratePresetRaids = true

[EventSystem]

## If enabled, removes all existing raids and only allows configured. Will only remove non-random events, leaving boss events as is.
RemoveAllExistingRaids = false

## Enable/disable override of existing events when event names match.
OverrideExisting = true

## Frequency between checks for new raids. Value is in mintues.
EventCheckInterval = 46

## Chance of raid, per check interval. 100 is 100%.
EventTriggerChance = 20

[IndividualRaids]

## If enabled, Custom Raids will overhaul the games way of checking for raids.
## This allows for setting individual frequences and chances for each raid.
UseIndividualRaidChecks = false

## If overhaul is enhabled, ensures a minimum amount of minutes between each raid.
MinimumTimeBetweenRaids = 46

[Debug]

## If enabled, scans existing raid event data, and dumps to a file in the plugin folder.
WriteDefaultEventDataToDisk = false

## If enabled, dumps raid event data after applying configuration to a file in the plugin folder.
WritePostChangeEventDataToDisk = false

## If enabled, scans existing environment data, and dumps to a file in the plugin folder.
WriteEnvironmentDataToDisk = false

## If enabled, scans existing global keys, and dumps to a file in the plugin folder.
WriteGlobalKeyDataToDisk = false

```

## Main raid file "custom_raids.raids.cfg"

Main file for adding raid configuration.

To add a raid you must:

1. Add a raid section with general event configuration

``` INI 
[YourRaidName]
Enabled=true
Name=MyRaid
... additional configuration
``` 

2. Add spawns to the raid event.
``` INI
[YourRaidName.Index]
Enabled=true
Name=Draugr Party Time
PrefabName=Draugr
MaxSpawned=500
... additional configuration
```

Repeat step 2, for additional spawns in the same raid event. Just make sure to increase the index every time.

For multiple raids, repeat from step 1.

## Supplemental raid file "custom_raids.supplemental.my_raid_name.cfg"

Exactly as for the main raid file (custom_raids.raids.cfg). This is simply to allow for easy splitting into multiple files, and for others to easily add new raids to Custom Raids.

## Just disable the trolls please

Just add the below to "custom_raids.raids.cfg". OverrideExisting is on by default.

``` INI
[foresttrolls]
Name=foresttrolls
Enabled=false
```

## Tips for configuring

Raid events are generally a bit "janky" to configure, so I suggest making use of the "RemoveAllExistingRaids" and console while testing.
Enable only your own change, and use console commands "randomevent" and "stopevent" to test things out.

Spawning during also seems to be very inconsistent, meaning with the same interval setting, you will sometimes have a bunch of wave triggers inside a short span, and sometimes it takes ages.

It is also important to understand how raids are started and run.

A pretty comprehensive guide for prefabs can be found [here](https://gist.github.com/Sonata26/e2b85d53e125fb40081b18e2aee6d584)

See the [Valheim Wiki - Event System](https://github.com/Valheim-Modding/Wiki/wiki/Valheim-Event-System) for further details on raid configuration and how they work.

### Raid initialization:
Basically, the host/server will make a check every once in a while. When the check happens, it then rolls a die to check if it should start an event.
A list of what it believes are "possible" raids is calculated, and a random one is picked.
These raids are based off the host/server instances configurations, meaning it needs to know about a raid to even to any checks.
Any clients are then notified that the raid should be started.

### Raid spawning:
When a client receives a start event message, and is in charge of the area, its world spawners (see [Spawning](https://github.com/Valheim-Modding/Wiki/wiki/Spawning)) will then start attempting to spawn in mobs.
It does this by finding the raid event it has loaded itself, based on the name received from the host.
If the event is found, the world spawners will try to spawn in the mobs in the event. For each mob they will check if spawn conditions are right. 
THIS is what usually makes most raids stumble. If the raid starts, but nothing spawns in, it is usually because the spawners can't get the conditions right.

# The Details - Raid Event

| Setting | Type | Example Value | Description |
| ------- | ----- | ---- | ---- |
| Name | String | DeerArmy | Name of event. Can be used to override existing configurations with same name (I am looking at you, foresttrolls...) |
| Enabled | bool | true | Enable/disable raid configuration from being used |
| Duration | float | 90 | Duration of raid in seconds |
| StartMessage | String | Raid started | Message shown on raid start |
| EndMessage | String | Raid ended | Message shown on raid end |
| NearBaseOnly | bool | true | Spawn raid near base only |
| RequiredGlobalKeys | string | defeated_bonemass, defeated_dragon | Array (separate by ",") of required global keys. Leave empty for no requirement. |
| NotRequiredGlobalKeys | string | defeated_bonemass, defeated_dragon | Array (separate by ",") of required global keys. Leave empty for no requirement. Not sure what it is used for. |
| RequireOneOfGlobalKeys | string | defeated_bonemass, defeated_gdking | Array (separate by ",") of global keys of which one is required. Leave empty for no requirement.
| PauseIfNoPlayerInArea | bool | true | |
| ForceEnvironment | string | Misty | Name of environment to set for raid |
| ForceMusic | string | CombatEventL1 | Name of music to set for raid |
| Random | bool | true | Include raid in random raid spawning. |
| Biomes | string | | Array (separate by ",") of biomes. Leave empty for all allowed. |
| ConditionWorldAgeDaysMin | float | 10 | Minimum number of in-game days of the world, for this raid to be possible. | 
| ConditionWorldAgeDaysMax | float | 100 | Maximum number of in-game days of the world, for this raid to be possible. 0 means no limit |
| ConditionDistanceToCenterMin | float | 1000 | Minimum distance to center for this raid to activate. |
| ConditionDistanceToCenterMax | float | 2000 | Maximum distance to center for this raid to activate. 0 means limitless. |
| CanStartDuringDay | bool | true | Enable/toggle this raid activating during day. |
| CanStartDuringNight | bool | true | Enable/toggle this raid activating during night |
| Faction | string | Boss | Assign a faction to all entities in raid. |
| RaidFrequency | float | 46 | Minutes between checks for this raid to run. 0 uses game default (46 minutes). This is only used if UseIndividualRaidChecks is set in general config. |
| RaidChance | float | 20 | Chance at each check for this raid to run. 0 uses game default (20%). This is only used if UseIndividualRaidChecks is set in general config. |


# The Details - Raid Spawns

| Setting | Type | Example | Description |
| --- | --- | --- | --- |
| Name | string | Draugr Party Time | Spawn configuration name |
| Enabled | bool | true | |
| PrefabName | string | Draugr | Prefab name of entity to spawn. This can be any prefab |
| MaxSpawned | int | 5 | Maximum entities of type spawned in area |
| SpawnInterval | float | 1 | Interval (seconds) between wave checks |
| SpawnChancePerInterval | 100 | Chance (0 to 100) to spawn new wave per interval |
| SpawnDistance | float | 0 | Minimum distance to another entity. Highly volatile setting |
| SpawnRadiusMin | float | 0 | Minimum spawn radius. Highly volatile setting |
| SpawnRadiusMax | float | 1 | Maximum spawn radius. Highly volatile setting |
| GroupSizeMin | int | 5 | Minimum number of spawns per wave |
| GroupSizeMax | int | 5 | Maxium number of spawns per wave |
| SpawnAtNight | bool | true | Can spawn at night |
| SpawnAtDay | bool | true | Can spawn at day |
| HuntPlayer | bool | true | Does what it says. Will not work for all mobs, Deer will ignore it |
| GroundOffset | float | 0.5 | Offset above ground at which entity will be spawned |
| MinLevel | int | 1 | Min level of spawn. (2 is one star) |
| MaxLevel | int | 3 | Max level of spawn. (2 is one star) |
| RequiredGlobalKey | string | defeated_bonemass | Global key required for spawning. Leave empty for no requirement. |
| RequiredNotGlobalKey | string | defeated_bonemass, KilledTroll | Array of global keys which disable the spawning of this entity if any are detected |
| RequiredEnvironments | string | Array (separate by "," of required environments. Leave empty for no requirement. |
| GroupRadius | float | 1 | Size of circle to spawn group inside. |
| AltitudeMin | float | -1000 | Minimum required altitude (distance to water surface) to spawn in |
| AltitudeMax | float | 1000 | Maximum required altitude (distance to water surface) to spawn in |
| TerrainTiltMin | float | 0 | Minium required tilt of terrain to spawn in |
| TerrainTiltMax | float | 35 | Maximum required tilt of terrain to spawn in |
| InForest | bool | true | Toggles spawning in forest |
| OutsideForest | bool | true | Toggles spawning outside of forest |
| OceanDepthMin | float | 0 | Minimum required ocean depth to spawn in. Ignored if min == max |
| OceanDepthMax | float | 0 | Maximum required ocean depth to spawn in. Ignored if min == max |
| Faction | string | ForestMonsters | Set custom faction for mob. This overrules the raids faction setting if set. |
| ConditionDistanceToCenterMin| float | 1000 | Minimum distance to center for entity to spawn |
| ConditionDistanceToCenterMax | float | 5000 | Maximum distance to center for entity to spawn. 0 means limitless |
| ConditionWorldAgeDaysMin | float | 10 | Minimum world age in in-game days for this entity to spawn |
| ConditionWorldAgeDaysMax | float | 50 | Maximum world age in in-game days for this entity to spawn. 0 means no max |
| DistanceToTriggerPlayerConditions | int | 100 | Distance of player to spawner, for player to be included in player based checks such as ConditionNearbyPlayersCarryValue |
| ConditionNearbyPlayersCarryValue | int | 25 | Checks if nearby players have a combined value in inventory above this condition. Eg. If set to 100, entry will only activate if nearby players have more than 100 worth of values combined |
| ConditionNearbyPlayerCarriesItem | string | IronScrap, DragonEgg | Checks if nearby players have any of the listed item prefab names in inventory |
| ConditionNearbyPlayersNoiseThreshold | float | 80 | Checks if any nearby players have accumulated noise at or above the threshold |

# Mod Specific Configuration

These are implemented soft-dependant, meaning if the mod is not present, the configuration will simply do nothing

## Creature Level and Loot Control
Additional options for [Creature Level and Loot Control](https://www.nexusmods.com/valheim/mods/495).
See the mod nexus page for more in-depth documentation for the options.

CLLC options can be set by adding another section to the raid creature desired changed as:

```INI
[YourRaidName.Index.CreatureLevelAndLootControl]
```

Eg.

```INI
[MyRaid]
Name=MyRaid

[MyRaid.0]
PrefabName=Troll

[MyRaid.0.CreatureLevelAndLootControl]
SetInfusion=Fire
```

| Setting | Type | Example | Description |
| --- | --- | --- | --- |
| ConditionWorldLevelMin | int | -1 | Minimum CLLC world level for spawn to activate. Negative value disables this condition |
| ConditionWorldLevelMax | int | 2 | Maximum CLLC world level for spawn to active. Negative value disables this condition |
| SetInfusion | string | Fire | Assigns the specified infusion to creature spawned. Ignored if empty |
| SetExtraEffect | string | Armored | Assigns the specified effect to creature spawned. Ignored if empty |
| SetBossAffix | string | Mending | Assigns the specified boss affix to spawned boss. Ignored if empty |
| UseDefaultLevels | bool | false | Use the default LevelMin and LevelMax for level assignment, ignoring the usual CLLC level control |

### Boss Affixes
- None
- Reflective
- Shielded
- Mending
- Summoner
- Elementalist
- Enraged
- Twin

### Extra Effects
- None
- Aggressive
- Quick
- Regenerating
- Curious
- Splitting
- Armored

### Infusions
- None
- Lightning
- Fire
- Frost
- Poison
- Chaos
- Spirit

## Spawn That!

Additional options while using [Spawn That](https://valheim.thunderstore.io/package/ASharpPen/Spawn_That/).
Spawn That options can be set by adding another section to the raid creature desired changed as:

```INI
[YourRaidName.Index.SpawnThat]
```

Eg.

```INI
[MyRaid]
Name=MyRaid

[MyRaid.0]
PrefabName=Greyling

[MyRaid.0.SpawnThat]
SetTryDespawnOnAlert=true
```

| Setting | Type | Example | Description |
| --- | --- | --- | --- |
| TemplateId | string | 12345_MyId | Technical setting intended for cross-mod identification of mobs spawned by this config entry. Sets a custom identifier which will be assigned to the spawned mobs ZDO as 'ZDO.Set("spawn_template_id", TemplateIdentifier)' |
| SetRelentless | bool | true | When true, forces mob AI to always be alerted |
| SetTryDespawnOnAlert | bool | true | When true, mob will try to run away and despawn when alerted |

# Configuration Options

## Biomes

- Meadows
- Swamp
- Mountain
- BlackForest
- Plains
- AshLands
- DeepNorth
- Ocean
- Mistlands

## Factions

- Players
- AnimalsVeg
- ForestMonsters
- Undead
- Demon
- MountainMonsters
- SeaMonsters
- PlainsMonsters
- Boss

## ForcedEnvironment (defaults, it should be possible to mod in more, and refer to them by name)

- Clear
- Twilight_Clear
- Misty
- Darklands_dark
- Heath clear
- DeepForest Mist
- GDKing
- Rain
- LightRain
- ThunderStorm
- Eikthyr
- GoblinKing
- nofogts
- SwampRain
- Bonemass
- Snow
- Twilight_Snow
- Twilight_SnowStorm
- SnowStorm
- Moder
- Ashrain
- Crypt
- SunkenCrypt

## Global Keys 

- defeated_eikthyr
- defeated_gdking
- defeated_bonemass
- defeated_dragon
- defeated_goblinking
- KilledTroll
- killed_surtling

Additional keys can be created manually through console commands, or by a mod like [Enhanced Progress Tracker](https://valheim.thunderstore.io/package/ASharpPen/Enhanced_Progress_Tracker/).

## Noise
Noise is set on each player based on certain activities they perform. It is set directly, and does not accumulate, meaning a player chopping trees will have the same noise of 100 for each chop and not increasingly higher.

Certain creatures will treat the noise as a "sound range". This means if the noise is greater than their "hearing" setting, and "noise" is within range of the creature (100 noise is 100 meters), they will react.

Noise constantly decays if no action is performed.
Known causes of noise:
- Dodge: 5
- Punching: 5
- Walking: 15
- Running: 30
- Jumping: 30
- Chopping / Pickaxing: 40
- Remove building piece: 50
- Chopping trees: 100
- Breaking rocks: 100

Apart from that, every attack will have a hit-noise and swing noise. By default this is 30 and 10, but this could be different for each attack type.

# Changelog 
- v1.4.0: 
	- Support for Creature Level and Loot Control.
	- Support for most Spawn That modifiers.
	- Ported over Spawn That spawning conditions.
	- Stopped raids from starting a new raid, if another event is already active.
	- Fixed bug causing a lot of raids to not run when not using IndividualRaids.
- v1.3.7: 
	- RemoveAllExistingRaids no longer removes non-random events. This should fix boss events being cleared as well.
- v1.3.6: 
	- Scanning all-subfolders for Custom Raids supplemental files.
- v1.3.5: 
	- Fixed issue with individual frequencies causing raids to not start.
	- Fixed potential multiplayer issue with faction assignment.
- v1.3.4: 
	- Fixed a critical bug in the compatibility code of 1.3.3 causing raid creatures not to spawn.
- v1.3.3: 
	- Improved compatibility with mods supplying prefabs.
	- Fixed installation detection of Enhanced Progress Tracker.
	- Changed global key check logic to reduce complexity and potential for errors.
- v1.3.2: 
	- Raid debug files now follow config format.
	- Fixed issue with faction assignment for raids with different factions for same mob type.
- v1.3.1: 
	- Fixed issue with faction assignment being skipped in certain situations.
- v1.3.0:
	- Set raid faction. Defaults to boss now, for all spawned creatures.
	- Conditions for day/night
	- Conditions for world age
	- Conditions for distance to world center
	- Condition for requiring one global key out of multiple
	- Fixed Valheim bug of NearBaseOnly being required. This enables raids anywhere in the world when false.
	- Overhaul of raid frequencies and chances. Can now select individual frequency and chance for each raid.
	- Support for Enhanced Progress Tracker.
- v1.2.0:
	- Server-to-client config synchronization added.
	- Fixed various mistakes in config descriptions. Sorry guys, I am bad at reading. EventChance is in range 0-100, frequency is in minutes.
	- Removed "LoadRaidConfigsOnWorldStart" option. This is always done by default now.
- v1.1.0: 
	- Removing biome area. It is simply a gun to shoot yourselves in the foot with.
	- Removing Biome, replacing with Biomes, now with actual names, and not some insane binary flag!
	- Fixed spawn issue, where a lot of biomes were getting disabled by mistake. It should now be a lot easier to get raids to spawn as intended.
- v1.0.1: 
	- Fixed debug output file not being enable-only by toggle in options
