
namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionPlayersOnline : IRaidCondition
    {
        public int? MinPlayersOnline { get; set; }

        public int? MaxPlayersOnline { get; set; }

        public bool IsValid(RaidContext context)
        {
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
