using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Caches;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids.Schedulers.Individual
{
    public class IndividualFrequencyScheduler : IRaidScheduler
    {
        public const string Name = "individual_frequency";

        private DateTimeOffset? LastRaid { get; set; }

        public void Initialize()
        {
            var zdo = ZdoCache.GetZDO(RandEventSystem.instance.gameObject);


        }

        public void ManageRaid(Raid raid)
        {
            throw new NotImplementedException();
        }

        public void Update(float deltaTime)
        {
            Log.LogTrace("Checking for possible raids.");


            if (CheckAndStartRaids(RandEventSystem.instance, ___m_eventTimer))
            {
                ___m_eventTimer = 0;
            }
        }

        private bool CheckAndStartRaids()
        {
            //Check if we have passed the minimum time between raids.
            if (m_eventTimer < ConfigurationManager.GeneralConfig.MinimumTimeBetweenRaids.Value * 60) //EventTimer is in seconds.
            {
                return false;
            }

            if (instance.GetActiveEvent() is not null || instance.GetCurrentRandomEvent() is not null)
            {
                Log.LogTrace("Skipping check of new raids due to already active event.");
                return false;
            }

            var time = ZNet.instance.GetTimeSeconds();

            //Get list of possible raids
            var possibleRaids = GetPossibleRaids(instance, time);

#if DEBUG
            Log.LogDebug($"Raids possible to spawn: {possibleRaids?.Count ?? 0}");
#endif

            if (possibleRaids.Count == 0)
            {
                return false;
            }

            //Select one randomly
            var selectedRaid = possibleRaids[UnityEngine.Random.Range(0, possibleRaids.Count)];

            selectedRaid.EventData.LastChecked = time;

            //Check chance
            if (selectedRaid.EventChance < UnityEngine.Random.Range(0, 100))
            {
                return false;
            }

            Log.LogDebug($"Starting raid: {selectedRaid?.Raid?.m_name}");

            //Start event.
            SetRandomEvent(instance, selectedRaid.Raid, selectedRaid.RaidCenter);
            return true;
        }
    }
}
