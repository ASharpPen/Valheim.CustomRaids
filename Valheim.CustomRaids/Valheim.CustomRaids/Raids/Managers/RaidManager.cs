using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Resetter;

namespace Valheim.CustomRaids.Raids.Managers;

public static class RaidManager
{
    private static ConditionalWeakTable<RandomEvent, Raid> RaidTable = new();
    private static Dictionary<string, List<Action<RandomEvent, Raid>>> Configurations = new();
    private static Dictionary<string, Raid> RaidsByName = new();

    static RaidManager()
    {
        StateResetter.Subscribe(() =>
        {
            RaidTable = new();
            Configurations = new();
            RaidsByName = new();
        });
    }

    internal static void CopyReference(RandomEvent from, RandomEvent to)
    {
        if (TryGetRaid(from, out var cached))
        {
            RaidTable.Add(to, cached);
        }
    }

    public static bool TryGetRaid(RandomEvent randomEvent, out Raid raid)
    {
        return RaidTable.TryGetValue(randomEvent, out raid);
    }

    public static bool TryGetRaid(string raidName, out Raid raid)
    {
        return RaidsByName.TryGetValue(raidName, out raid);
    }

    public static void RegisterRaid(RandomEvent randomEvent, Raid raid)
    {
        // Check if raid is already registered. If so, replace it.
        if (RaidTable.TryGetValue(randomEvent, out var existing))
        {
            RaidTable.Remove(randomEvent);
        }

        RaidTable.Add(randomEvent, raid);
        RaidsByName[raid.Name] = raid;

        // Apply configurations
        if (Configurations.TryGetValue(randomEvent.m_name, out var namedRaidConfigurations))
        {
            foreach (var namedConfig in namedRaidConfigurations)
            {
                ConfigureRaid(randomEvent, raid, namedConfig);
            }
        }

        if (Configurations.TryGetValue(randomEvent.m_name, out var generalRaidConfigurations))
        {
            foreach (var generalConfiguration in generalRaidConfigurations)
            {
                ConfigureRaid(randomEvent, raid, generalConfiguration);
            }
        }

        void ConfigureRaid(RandomEvent randomEvent, Raid raid, Action<RandomEvent, Raid> configureRaid)
        {
            try
            {
                configureRaid(randomEvent, raid);
            }
            catch (Exception e)
            {
                Log.LogWarning($"Error during registered configuration of raid '{randomEvent.m_name}'.", e);
            }
        }
    }

    /// <summary>
    /// Register callback for any RegisterRaid.
    /// Reset on world startup.
    /// </summary>
    public static void OnRegisterAnyRaid(Action<RandomEvent, Raid> configureRaid)
    {
        OnRegisterRaid(null, configureRaid);
    }

    /// <summary>
    /// Register callback for RegisterRaid for RandomEvent with raidName.
    /// Reset on world startup.
    /// </summary>
    public static void OnRegisterRaid(string raidName, Action<RandomEvent, Raid> configureRaid)
    {
        if (Configurations.TryGetValue(raidName, out var configurations))
        {
            configurations.Add(configureRaid);
        }
        else
        {
            Configurations[raidName] = new List<Action<RandomEvent, Raid>>() { configureRaid };
        }
    }
}
