using Valheim.CustomRaids.Utilities;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionPlayersNearby : IRaidCondition
    {
        public int? MinPlayers { get; set; }

        public int? MaxPlayers { get; set; }

        public float Distance { get; set; } = 200;

        public bool IsValid(RaidContext context)
        {
            var playersNearby = PlayerUtils
                .GetPlayersInRadius(context.Position, Distance)
                .Count;

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
