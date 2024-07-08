using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Integrations;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionRequireOneOfGlobalKeys : IRaidCondition
    {
        public List<string> GlobalKeys { get; set; }

        public ConditionRequireOneOfGlobalKeys(List<string> globalKeys)
        {
            GlobalKeys = globalKeys;
        }

        public bool IsValid(RaidContext context)
        {
            if (GlobalKeys is null || 
                GlobalKeys.Count == 0)
            {
                return true;
            }

            if (InstallationManager.WAPInstalled &&
                WAPKeyChecks.ShouldUseWAP())
            {
                var playerId = context.PlayerUserId ?? context.IdentifyPlayerByPos(context.Position);

                if (playerId is not null)
                { 
                    return GlobalKeys.Any(x => WAPKeyChecks.Check(playerId.Value, x));
                }

                return false; // Unable to identify player.
            }
            else
            {
                return GlobalKeys.Any(ZoneSystem.instance.GetGlobalKey);
            }
        }
    }
}
