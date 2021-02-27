
### Configuration Options

#### Biome flags:
Biomes are registered as binary flags in valheim, meaning they each have a specific bit of a number, which is checked for true or false.
This means to allow for both Meadows (value 1, position 1) and Blackforest (value 8, position 4), the biome flag should be 9 (binary value 1001);
Just use 513 (biomes max) if you are uncertain. That enables all biomes.

#### Biomes (Name, Value, Position)
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

#### BiomeArea (Name, Value, Position)
As with biome flags. Use 3 for all.

- Edge, 1, 0,
- Median, 2, 1,
- Everything, 3

#### ForcedEnvironment (defaults, it should be possible to mod in more, and refer to them by name):
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

#### Global Keys (incomplete list)
- defeated_eikthyr
- KilledTroll
- killed_surtling
- defeated_gdking
- defeated_bonemass
- defeated_dragon