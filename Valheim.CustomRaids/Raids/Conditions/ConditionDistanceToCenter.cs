using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionDistanceToCenter : IRaidCondition
    {
        public float? MinDistance { get; set; }

        public float? MaxDistance { get; set; }

        public bool IsValid(RaidContext context)
        {
            var distanceToCenter = context.Position.magnitude;

            if (MinDistance is not null)
            {
                if (MinDistance > distanceToCenter)
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due being too far from center. {MinDistance} > {distanceToCenter}");
                    return false;
                }
            }

            if (MaxDistance is not null)
            {
                if (MaxDistance > 0 && MaxDistance < distanceToCenter)
                {
                    Log.LogDebug($"Raid {context.RandomEvent.m_name} disabled due being too close to center. {MaxDistance} < {distanceToCenter}");
                    return false;
                }
            }

            return true;
        }
    }
}
