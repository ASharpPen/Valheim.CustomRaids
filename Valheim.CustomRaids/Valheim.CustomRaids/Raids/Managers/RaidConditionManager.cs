using System;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids.Conditions;

namespace Valheim.CustomRaids.Raids.Managers;

public static class RaidConditionManager
{
    public static void RegisterCondition(string raidName, IRaidCondition condition)
    {
        if (RaidManager.TryGetRaid(raidName, out var raid))
        {
            raid.Conditions.Add(condition);
        }
        else
        {
            Log.LogWarning($"Attempted to register condition '{condition.GetType().Name}' for raid '{raidName}' before it was registered.");
        }
    }

    public static bool HasValidConditions(RandomEvent randomEvent, Vector3 raidPosition)
    {
        var raidContext = new RaidContext
        {
            RandomEvent = randomEvent,
            Position = raidPosition,
        };

        if (RaidManager.TryGetRaid(randomEvent, out var raid))
        {
            if (!raid.Conditions.All(x =>
            {
                try
                {
#if DEBUG
                        var isValid = x.IsValid(raidContext);
                    if (!isValid)
                    {
                        Log.LogDebug($"[{raid.Name}] Invalid condition {x.GetType().Name}.");
                    }
                    return isValid;
#else
                        return x.IsValid(raidContext);
#endif
                    }
                catch (Exception e)
                {
                    Log.LogWarning($"Error during check of raid condition '{x.GetType().Name}'. Ignoring condition.", e);
                    return true;
                }
            }))
            {
                // Invalid condition detected. Filtering raid.
                return false;
            }
        }

        return true;
    }
}
