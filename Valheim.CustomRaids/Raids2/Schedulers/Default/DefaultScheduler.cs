using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids.Extensions;
using Valheim.CustomRaids.Raids2.Conductors.Default;

namespace Valheim.CustomRaids.Raids2.Schedulers.Default
{
    public class DefaultScheduler : IRaidScheduler
    {
        private DefaultConductorOptions Options { get; }

        private Dictionary<string, DefaultSchedulerRaid> Raids { get; set; } = new Dictionary<string, DefaultSchedulerRaid>();

        public DefaultScheduler(DefaultConductorOptions options)
        {
            Options = options;
        }

        public void Update(float deltaTime)
        {
            var instance = RandEventSystem.instance;

            var raidTimer = instance.m_eventTimer += deltaTime;

            //Check if we have passed the minimum time between raids.
            if (raidTimer < Options.Interval.TotalSeconds)
            {
                return;
            }

            if (instance.HasActiveEvent())
            {
                Log.LogTrace("Skipping check of new raids due to already active event.");
                return;
            }

            // Reset timer.
            instance.m_eventTimer = 0;

            var chance = UnityEngine.Random.Range(0f, 100f);

            var raids = new List<ValidRaid<DefaultSchedulerRaid>>();

            if (Options.Chance < chance)
            {
                return;
            }

            // Check raid conditions.
            foreach (var raid in Raids.Values)
            {
                var possibleRaids = StartConditionManager.GetValidRaids(raid);

                if ((possibleRaids?.Count ?? 0) == 0)
                {
                    continue;
                }

                // Select random possible raidcenter.
                raids.Add(possibleRaids[UnityEngine.Random.Range(0, possibleRaids.Count)]);
            }

            if(raids.Count == 0)
            {
                return;
            }

            // Select random raid
            var raidToStart = raids[UnityEngine.Random.Range(0, raids.Count)];


        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            throw new NotImplementedException();
        }
    }
}
