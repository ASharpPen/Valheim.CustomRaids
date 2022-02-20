using UnityEngine;
using Valheim.CustomRaids.Raids;

namespace Valheim.CustomRaids.RaidFrequencyOverhaul
{
    internal class PossibleRaid
    {
        public RandomEventData EventData;

        public RandomEvent Raid;

        public Vector3 RaidCenter;

        public float EventChance = 20;
    }
}
