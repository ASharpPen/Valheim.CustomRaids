using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.CustomRaids.Patches;

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
