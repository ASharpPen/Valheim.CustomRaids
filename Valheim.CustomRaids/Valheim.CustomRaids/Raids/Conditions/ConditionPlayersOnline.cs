
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionPlayersOnline : IRaidCondition
    {
        public int? MinPlayersOnline { get; set; }

        public int? MaxPlayersOnline { get; set; }

        public bool IsValid(RaidContext context)
        {
            // If not a multiplayer game, skip this condition.
            if (ZNet.m_isServer &&
                !ZNet.m_openServer)
            {
                Log.LogDebug($"[{nameof(ConditionPlayersOnline)}] Not a multiplayer game. Skipping condition.");
                return true;
            }

            int playersOnline = ZNet.instance.GetPeerConnections();

            if (MinPlayersOnline is not null)
            {
                if (MinPlayersOnline > playersOnline)
                {
                    return false;
                }
            }

            if (MaxPlayersOnline is not null)
            {
                if (MaxPlayersOnline > 0 && MaxPlayersOnline < playersOnline)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
