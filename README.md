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
- Support for [Creature Level and Loot Control](https://www.nexusmods.com/valheim/mods/495)

# Documentation

Documentation can be found on the [Custom Raids Wiki](https://github.com/ASharpPen/Valheim.CustomRaids/wiki).

# FAQ

- Is it server side only?
	- No. Both client and server needs the mod.
	- The way events work is that the host / dedicated server will run the logic for when to apply raids, and then send a message with the name of the raid to be assigned to all clients. The client in charge of zone (valheim multiplayer is weird) will then manage the actual spawning for the raid.
- Can I just have no raids?
	- Yes. Raid activation happens server-side, making this even simpler, you should only need this mod serverside if you just want no raids. (You can also use Event Enhancer for this)
	- Set configuration "EventTriggerChance=0" or
	- Set configuration "RemoveAllExistingRaids=true", as long you don't have something custom, this will remove the options, thereby removing raids.

# Client / Server

Custom Raids needs to be installed on all clients (and server) to work.

From v1.2.0 clients will request the configurations currently loaded by the server, and use those without affecting the clients config files.
This means you should be able to have server-specific configurations, and the client can have its own setup for singleplayer.
For this to work, the mod needs to be installed on the server, and have configs set up properly there. When players join with Custom Raids v1.2.0, their mod will use the servers configs.

# Example

```INI
[Example_Raid]
Enabled = true
Name = Example1
Random = true
NearBaseOnly = false
RaidFrequency = 0.1
RaidChance = 100

[Example_Raid.0]
PrefabName = Skeleton
HuntPlayer = true
MaxSpawned = 10
SpawnInterval = 2
SpawnChancePerInterval = 100
```

# Support

If you feel like it

<a href="https://www.buymeacoffee.com/asharppen"><img src="https://img.buymeacoffee.com/button-api/?text=Buy me a coffee&emoji=&slug=asharppen&button_colour=FFDD00&font_colour=000000&font_family=Cookie&outline_colour=000000&coffee_colour=ffffff" /></a>

# Changelog 
- v1.6.2:
	- Fixed individual raids not checking CanStartDuringDay and CanStartDuringNight correctly.
	- Fixed individual raids resetting raid cooldown too early under certain conditions, causing way less or no raids.
- v1.6.1:
	- Fixed individual raids not being affected by paused raid timer.
	- Changed individual raids to now be checked pr EventCheckInterval. This should make it easier to control individual raids frequency, especially when a lot of raids are available. Note this should generate less raids for most, and might require some tweaking of the EventCheckInterval if you liked it the old way (setting it to 0.01 should do it).
- v1.6.0:
	- Added server setting for pausing raid timers while no players are online. Enabled by default.
	- Added raid condition ConditionMustBeNearPrefab for checking if any of the listed prefabs is nearby.
	- Added raid condition ConditionMustBeNearAllPrefabs for checking if all of the listed prefabs are nearby.
	- Added raid condition ConditionMustNotBeNearPrefab for ensuring none of the listed prefabs are nearby.
	- Added action for starting new raid on raid stop.
- v1.5.3:
	- Fixed issue with spawning on mountains due to distance check from player including height.
	- Fixed spawn SetFaction modifier being case-sensitive.
- v1.5.2:
	- Minor robustness improvements and internal cleanup.
	- Removed mistakenly packaged dll.
- v1.5.1:
	- Fixed an unintentional reference to SpawnThat, causing an error when not installed.
- v1.5.0:
	- Added new raid conditions; altitude and environment.
	- Additional error handling.
	- Debug files are now printed to path BepInEx/Debug. Can be configured.
	- Fixed detection of Spawn That installation.
	- Fixed supplemental file detection not being restricted to cfg's.
	- Lots internal fixes and updates.
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
