using System.Collections;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Actions;

/// <summary>
/// Starts a new raid in a short while.
/// </summary>
public class StartRaidAction : IRaidAction
{
    public string RaidName { get; set; }

    public StartRaidAction(string raidToStart)
    {
        RaidName = raidToStart.Trim();
    }

    public void Execute(RaidContext context)
    {
        if (string.IsNullOrWhiteSpace(RaidName))
        {
            return;
        }

        if (!ZNet.instance.IsServer())
        {
            return;
        }

        RandEventSystem.instance.StartCoroutine(StartRaid(RaidName, context));
    }

    public static IEnumerator StartRaid(string raidName, RaidContext context)
    {
        yield return null;

        // Validate raid is available.
        var raid = RandEventSystem.instance.GetEvent(raidName);

        if (raid is null)
        {
            Log.LogWarning($"Unable to find raid '{raidName}' while executing OnStopStartRaid action. Verify that you are using the right name or raid being properly enabled.");
        }
        else
        {
            Log.LogDebug($"StartRaidAction: Starting new raid '{raidName}' at '{context.Position}'");
            RandEventSystem.instance.SetRandomEventByName(raidName, context.Position);
        }
    }
}
