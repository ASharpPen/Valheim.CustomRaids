using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Integrations;

namespace Valheim.CustomRaids.Raids.Conditions;

public class ConditionRequiredGlobalKeys : IRaidCondition
{
    public List<string> RequiredGlobalKeys { get; set; }

    public ConditionRequiredGlobalKeys(List<string> requiredGlobalKeys)
    {
        RequiredGlobalKeys = requiredGlobalKeys;
    }

    public bool IsValid(RaidContext context)
    {
        if ((RequiredGlobalKeys?.Count ?? 0) == 0)
        {
            return true;
        }

        if (InstallationManager.WAPInstalled &&
            WAPKeyChecks.ShouldUseWAP())
        {
            var playerId = context.PlayerUserId ?? context.IdentifyPlayerByPos(context.Position);

            if (playerId is not null)
            {
                return RequiredGlobalKeys.All(x => WAPKeyChecks.Check(playerId.Value, x));
            }

            return false; // Unable to identify player.
        }
        else
        {
            return RequiredGlobalKeys.All(ZoneSystem.instance.GetGlobalKey);
        }
    }
}
