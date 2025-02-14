# Custom Raids

This mod allows for customizing raids to your liking.

Want to have armies besieging your fortress? A bit of extra environmental effects along your way? Or just disable trolls?

This mod can help you do all of that!

Comes preconfigured with two additional end-game raids (disabled by default), to show how and what can be done:
- Ragnarok
- Deathsquitoseason

Enable those raids at own risk!

# Documentation

Documentation can be found on the [Custom Raids Wiki](https://github.com/ASharpPen/Valheim.CustomRaids/wiki).

# Features

- Can change frequency of raids
- Add new raid events, with full configuration options, including spawns
- Override existing raid (eg. disable trolls)
- Supplemental raid configurations. Add your own raid in its own file, and Custom Raids will scan and apply it. 
- Potential for hours of frustration/fun as you figure out how to best configure these damn things to work as expected.
- Server-side configurations

# FAQ

- Is it server side only?
	- No. Both client and server needs the mod for most features. It can be server-only in very limited scenarios, like disabling raids, but generally it will need to be on both sides.
	- The way events work is that the host / dedicated server will run the logic for when to apply raids, and then send a message with the name of the raid to be assigned to all clients. The client in charge of zone (valheim multiplayer is weird) will then manage the actual spawning for the raid.
- Can I just have no raids?
	- Yes. Raid activation happens server-side, making this even simpler, you should only need this mod serverside if you just want no raids. (You can also use Event Enhancer for this)
	- Set configuration "EventTriggerChance=0" or
	- Set configuration "RemoveAllExistingRaids=true", as long you don't have something custom, this will remove the options, thereby removing raids.

# Client / Server

Custom Raids needs to be installed on all clients (and server) to work.

From v1.2.0 clients will request the configurations currently loaded by the server, and use those without affecting the clients config files.
This means you should be able to have server-specific configurations, and the client can have its own setup for singleplayer.
For this to work, the mod needs to be installed on the server, and have configs set up properly there. When players join with Custom Raids v1.2.0 or later, their mod will use the servers configs.

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
- v1.7.14:
	- Fixed: Issue with overriding raids when multiple shared same name. This caused only one of the two raids to get modified and the other left as is. From now on, a raid configuration will replace all raids with same name with a single configured one. This was detected due to the two vanilla raids "army_theelder" and "army_moder" having duplicates.
	- Fixed: `custom_raids.raids.location_events_before_changes.txt` printing to disk even though debug option was disabled.
- v1.7.13:
	- Compatibility: Valheim v0.219.10.
	- Added: Support for new vanilla player-based raid condition - ConditionPlayerMustHaveAllOfPlayerKeys.
- v1.7.12:
	- Compatibility: When World Advancement Progression is installed and using its private-keys, Custom Raids will now use that for global key lookups server-side.
- v1.7.11:
	- Compatibility: Valheim v0.218.15.
- v1.7.10:
	- Fixed: Compatibility with Valheim v.0.217.29. Serialization issue when syncing configs from servers resolved.
	- Added soft-dependency for LocalizationCache, to help it get loaded earlier.
	- Changed default value for setting 'StopTouchingMyConfigs' in custom_raids.cfg to true.
- v1.7.9:
	- Fixed: Issue with raids not always being added to "possible" list when using individual raids.
	- Fixed: Vanilla does not immediately update player-based keys added on creature kill. Custom Raids will always try to trigger the key update on creature kills now.
	- Added: Player-based raid conditions. These are the vanilla conditions that get enabled when using the world-modifiers for player-based events.
	- Added: Terminal commands for debugging the player-based events.
	- Changed: The global-key debug file now shows a list of creatures and which key is gained from killing it.
	- Various minor tweaks to generated debug files.
- v1.7.8:
	- Fixed: Setting "Faction" was only applying the faction on creature spawn, it now correctly re-applies on creature load.
- v1.7.7:
	- Fixed: Compatibility with Valheim v0.217.14.
- v1.7.6:
	- Fixed: Compatibility with Valheim v0.216.9.
- v1.7.5:
	- Fixed: Compatibility with Valheim v0.215.2.
- v1.7.4:
	- Compatibility: Moved all applications of spawn modifier settings to happen slightly later. This should solve issues with settings like CLLC UseDefaultLevels.
- v1.7.3:
	- Compatibility: Default biome settings for spawns and raids changed to include customly added biomes.
- v1.7.2:
	- Removed compatibility with deprecated mod (Enhanced Progress Tracker) in favour of adding compatibility with World Advancement Progression.
- v1.7.1:
	- Fixed individual raids 'RaidFrequency' and 'RaidChance' not using their intended default values, causing raids to sometimes happen a lot less frequently than expected (or never).
- v1.7.0:
	- Added new raid condition 'ConditionLocation'.
	- Added new debug setting 'WriteLocationsToDisk'.
	- Changed ConditionPlayersOnlineMin/Max to be skipped if not in a multiplayer game.
	- Fixed network packages being up to twize the size they needed, woops!
- v1.6.3:
	- Fixed compatibility for v0.212.6
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
