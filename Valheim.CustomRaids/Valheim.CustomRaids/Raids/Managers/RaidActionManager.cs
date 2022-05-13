using System;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Managers;

internal static class RaidActionManager
{
    public static void ExecuteOnStopRandomEventActions()
        => ExecuteOnStopActions(RandEventSystem.instance.m_randomEvent);

    public static void ExecuteOnStopForcedEventActions()
        => ExecuteOnStopActions(RandEventSystem.instance.m_forcedEvent);

    public static void ExecuteOnStopActions(RandomEvent randomEvent)
    {
        var raidContext = new RaidContext
        {
            RandomEvent = randomEvent,
            Position = randomEvent.m_pos,
        };

        if (randomEvent is null)
        {
            return;
        }

        try
        {
            if (RaidManager.TryGetRaid(randomEvent, out var raid))
            {
                foreach (var action in raid.OnStopActions)
                {
                    if (action is null)
                    {
                        continue;
                    }

                    try
                    {
                        Log.LogTrace("Executing on-stop raid action: " + action.GetType().Name);

                        action.Execute(raidContext);
                    }
                    catch (Exception e)
                    {
                        Log.LogError($"Error while attempting to execute raid on-stop action '{action.GetType().Name}' for random event '{randomEvent.m_name}'.", e);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to look up raid configuration for random event '{randomEvent?.m_name}'.", e);
        }
    }
}
