using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionPlayersNearby : IRaidCondition
    {
        public int? MinPlayers { get; set; }

        public int? MaxPlayers { get; set; }

        public float Distance { get; set; } = 200;

        public bool IsValid(RaidContext context)
        {            
            // If not a multiplayer game, skip this condition.
            if (ZNet.m_isServer &&
                !ZNet.m_openServer)
            {
                Log.LogDebug($"[{nameof(ConditionPlayersNearby)}] Not a multiplayer game. Skipping condition.");
                return true;
            }

            var connections = ZNet.instance.GetConnectedPeers();

            int playersNearby = 0;

            foreach (var connection in connections)
            {
                if (Utils.DistanceXZ(connection.GetRefPos(), context.Position) <= Distance)
                {
                    playersNearby++;
                }
            }

            if (MinPlayers is not null)
            {
                if (MinPlayers > playersNearby)
                {
                    return false;
                }
            }

            if (MaxPlayers is not null)
            {
                if (MaxPlayers > 0 && MaxPlayers < playersNearby)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
