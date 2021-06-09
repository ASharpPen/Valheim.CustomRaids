using UnityEngine;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids2.Spawns.Conditions
{
    public class ConditionDistanceToCenter : ISpawnCondition
    {
        public float DistanceToCenterMin { get; set; }

        public float DistanceToCenterMax { get; set; }

        public bool IsValid(Vector3 position)
        {
            var distance = position.magnitude;

            if (distance < DistanceToCenterMin)
            {
                Log.LogTrace($"{nameof(ConditionDistanceToCenter)} not valid due to distance being less than min {DistanceToCenterMin}.");
                return false;
            }

            if (DistanceToCenterMax > 0 && distance > DistanceToCenterMax)
            {
                Log.LogTrace($"{nameof(ConditionDistanceToCenter)} not valid due to distance greater than max {DistanceToCenterMax}.");
                return false;
            }

            return true;
        }
    }
}
