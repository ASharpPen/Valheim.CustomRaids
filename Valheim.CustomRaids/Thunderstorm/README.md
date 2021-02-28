This mod allows for customizing raids to your liking.

Want to have armies besieging your fortress? A bit of extra environmental effects along your way? Or just disable trolls?

This mod can help you do all of that!

Comes preconfigured with two additional end-game raids (disabled by default), to show how and what can be done.

Enable those raids at own risk.

# Features

- Can change frequency of raids
- Add new raid events, with full configuration options, including spawns
- Override existing raid (eg. disable trolls)
- Supplemental raid configurations. Add your own file, and Custom Raids will scan and apply it
- Potential for hours of frustration/fun as you figure out how to best configure these damn things to work as expected.

# Configuration

All configurations are placed in the default BepInEx configuration folder, and generated upon starting the game.

## General "custom_raids.cfg"

General configuration includes general mod controls, overall event system changes, and debugging options.

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

Exactly as for the main raid file. This is simply to allow for easy splitting into multiple files, and for others to easily add new raids to Custom Raids.

## Just disable the trolls please

Just add the below to "custom_raids.raids.cfg". OverrideExisting is on by default.

``` INI
[foresttrolls]
Name=foresttrolls
Enabled=false
```

## Tips for configuring

Raid events are generally a bit "janky" to configure, so I suggest making use of the "RemoveAllExistingRaids" and "LoadRaidConfigsOnWorldStart"option from the general config. 
Enabling only your own change, and use console commands "randomevent" and "stopevent" to test things out.

Spawning during also seems to be very inconsistent, meaning with the same interval setting, you will sometimes have a bunch of wave triggers inside a short span, and sometimes it takes ages.

Biomes were a big annoyance to me during testing. To just "make it work", ensure that biome is set to 0 (everything).

ForcedEnvironment also seems to be taken more as a hint, than something forced. It should trigger most of the time though.

A pretty comprehensive guide for prefabs can be found here https://gist.github.com/Sonata26/e2b85d53e125fb40081b18e2aee6d584 or here https://steamcommunity.com/sharedfiles/filedetails/?id=2392080520

# The Details - Raid Event

| Setting | Type | Example Value | Description |
| ------- | ----- | ---- | ---- |
| Name | String | DeerArmy | Name of event. Can be used to override existing configurations with same name (I am looking at you, foresttrolls...) |
| Enabled | bool | true | |
| Duration | float | 90 | |
| StartMessage | String | Raid started | Message shown on raid start |
| EndMessage | String | Raid ended | Message shown on raid end |
| NearBaseOnly | bool | true | Spawn raid near base only. Looks like this one might need to always be true due to the games valid spawn logic. |
| RequiredGlobalKeys | string | defeated_bonemass, defeated_dragon | Array (separate by \",\" of required global keys. Leave empty for no requirement. |
| NotRequiredGlobalKeys | string | defeated_bonemass, defeated_dragon | Array (separate by \",\" of required global keys. Leave empty for no requirement. Not sure what it is used for. |
| PauseIfNoPlayerInArea | bool | true | |
| ForceEnvironment | string | Misty | Name of environment to set for raid |
| ForceMusic | string | CombatEventL1 | Name of music to set for raid |
| Random | bool | true | Include raid in random raid spawning. |
| Biome | int | 9 | Biome flag for allowed biomes. See biome documentation. |

# The Details - Raid Spawns

| Setting | Type | Example | Description |
| --- | --- | --- | --- |
| Name | string | Draugr Party Time | Name of spawn group. Seemingly not used for anything.
| Enabled | bool | true | |
| PrefabName | string | Draugr | Name of prefab to spawn. |
| MaxSpawned | int | 5 | Maximum alive at a time. |
| SpawnInterval | float | 1 | Interval (seconds) between wave checks. |
| SpawnChancePerInterval | 100 | Chance (0 to 100) to spawn new wave per interval.
| SpawnDistance | float | 0 | |
| SpawnRadiusMin | float | 0 | |
| SpawnRadiusMax | float | 1 | |
| GroupSizeMin | int | 5 | Minimum number of spawns per wave |
| GroupSizeMax | int | 5 | Maxium number of spawns per wave |
| SpawnAtNight | bool | true | |
| SpawnAtDay | bool | true | |
| HuntPlayer | bool | true | Does what it says. Will not work for all mobs, Deer will ignore it. |
| GroundOffset | float | 0.5 | Distance to ground on spawn |
| MinLevel | int | 1 | Min level to spawn. Range 1 to 3.
| MaxLevel | int | 3 | Max level to spawn. Range 1 to 3.
| Biome | int | 513 | Spawn biome. Unknown if this has any effect on raids. Binary flag for enums. See Biome flags |
| BiomeArea | int 7 | Spawn location in biome. Binary flag for enum. See Biome areas |
| RequiredGlobalKey | string | defeated_bonemass | Global key required for spawning. Leave empty for no requirement. |
| RequiredEnvironments | string | Array (separate by \",\" of required environments. Leave empty for no requirement. |
| GroupRadius | float | 1 | |
| AltitudeMin | float | -1000 | |
| AltitudeMax | float | 1000 | |
| TerrainTiltMin | float | 0 | |
| TerrainTiltMax | float | 35 | |
| InForest | bool | true | |
| OutsideForest | bool | true | |
| OceanDepthMin | float | 0 | |
| OceanDepthMax | float | 0 | |

## Configuration Options

### Biome flags:
Biomes are registered as binary flags in valheim, meaning they each have a specific bit of a number, which is checked for true or false.
This means to allow for both Meadows (value 1, position 1) and Blackforest (value 8, position 4), the biome flag should be 9 (binary value 1001);
Just use 0 (biomes none) if you are uncertain. That enables all biomes.

### Biomes (Name, Value, Position)
- None, 0, 0
- Meadows, 1, 1
- Swamp, 2, 2,
- Mountain, 4, 3
- Blackforest, 8, 4
- Plains, 16, 5
- AshLands, 32, 6
- DeepNorth, 64, 7
- Ocean, 256, 8
- Mistlands, 512, 9
- BiomesMax, 513

### BiomeArea (Name, Value, Position)
As with biome flags. Use 3 for all.

- Edge, 1, 0,
- Median, 2, 1,
- Everything, 3

### ForcedEnvironment (defaults, it should be possible to mod in more, and refer to them by name):
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

### Global Keys (incomplete list)
- defeated_eikthyr
- KilledTroll
- killed_surtling
- defeated_gdking
- defeated_bonemass
- defeated_dragon

# Changelog

- 1.0.1: Fixed debug output file not being enable-only by toggle in options