using System;
using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids.Extensions;
using Valheim.CustomRaids.Raids.Schedulers;
using Valheim.CustomRaids.Raids2.Schedulers;

namespace Valheim.CustomRaids.Raids.Serverside.Schedulers.DefaultOverhauled
{
    public class DefaultOverhauledScheduler : IRaidScheduler
    {
        private const string ZdoPrefix = "scheduler_defaultOverhaul";

        private Dictionary<string, Raid> Raids = new Dictionary<string, Raid>();

        private TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(46);

        private float Chance { get; set; } = 20;

        public void Initialize()
        {
        }

        public void Load()
        {
        }

        public void ManageRaid(Raid raid)
        {
            Raids[raid.Id] = raid;
        }

        public void Save()
        {
        }

        public void Update(float deltaTime)
        {
            var instance = RandEventSystem.instance;

            var raidTimer = instance.m_eventTimer += deltaTime;

            //Check if we have passed the minimum time between raids.
            if (raidTimer < Interval.TotalSeconds)
            {
                return;
            }

            if(instance.HasActiveEvent())
            {
                Log.LogTrace("Skipping check of new raids due to already active event.");
                return;
            }

            // Reset timer.
            instance.m_eventTimer = 0;

            var chance = UnityEngine.Random.Range(0f, 100f);

            var raids = new List<ValidRaid>();

            if(chance <= Chance)
            {
                // Check raid conditions.
                foreach (var raid in Raids.Values)
                {
                    var possibleRaids = StartConditionManager.GetValidRaids(raid);

                    if((possibleRaids?.Count ?? 0) == 0)
                    {
                        continue;
                    }

                    // Select random possible raidcenter.

                }
            }
        }
    }
}
