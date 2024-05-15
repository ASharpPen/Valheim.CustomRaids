using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.Configuration;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids;
using Valheim.CustomRaids.Raids.Managers;

namespace Valheim.CustomRaids.RaidFrequencyOverhaul
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public static class RaidFrequencyOverhaul
    {
        /// <summary>
        /// Take control over raid checking.
        /// </summary>
        [HarmonyPatch(nameof(RandEventSystem.UpdateRandomEvent))]
        [HarmonyPrefix]
        private static bool UpdateIndividualRandomEvents(RandEventSystem __instance, float dt, ref float ___m_eventTimer, ref float ___m_sendTimer)
        {
            if ((ConfigurationManager.GeneralConfig?.UseIndividualRaidChecks?.Value ?? false) == false)
            {
                return true;
            }

            if(!ZNet.instance.IsServer())
            {
                return true;
            }

            if (ConfigurationManager.GeneralConfig.PauseEventTimersWhileOffline.Value &&
                ZNet.instance.GetAllCharacterZDOS().Count == 0)
            {
                return false;
            }

            ___m_eventTimer += dt;
            ___m_sendTimer += dt;

            if (___m_sendTimer > 2f)
            {
#if DEBUG
                Log.LogTrace($"Raid cooldown: {___m_eventTimer}, Raid timer: {___m_sendTimer}");
#endif

                if (___m_eventTimer >= ConfigurationManager.GeneralConfig.EventCheckInterval.Value * 60)
                {
                    Log.LogTrace("Checking for possible raids.");

                    if (CheckAndStartRaids(__instance, ___m_eventTimer))
                    {
                        ___m_eventTimer = 0;
                    }

#if DEBUG
                    foreach (RandomEvent randomEvent in __instance.m_events)
                    {
                        if (randomEvent.m_enabled && randomEvent.m_random)
                        {
                            var eventData = RandomEventCache.Get(randomEvent);

                            Log.LogTrace($"\t{randomEvent.m_name}: {eventData.LastChecked}");
                        }
                    }
#endif
                }

                //Update current event timer. 
                //Just using the default solution here... It apparently sends out an update every 2 seconds with the current raid event.
                if (___m_sendTimer > 2f)
                {
                    ___m_sendTimer = 0f;
                    __instance.SendCurrentRandomEvent();
                }
            }

            return false;
        }

        /// <returns>true, if raids were checked.</returns>
        private static bool CheckAndStartRaids(RandEventSystem instance, float m_eventTimer)
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

            Log.LogDebug($"Raids possible to spawn: {possibleRaids?.Count ?? 0}");
            if (possibleRaids.Count > 0)
            {
                Log.LogDebug($"{possibleRaids.Join(x => x.Raid.m_name)}");
            }

            if (possibleRaids.Count == 0)
            {
                return true;
            }

            //Select one randomly
            var selectedRaid = possibleRaids[UnityEngine.Random.Range(0, possibleRaids.Count)];

            selectedRaid.EventData.LastChecked = time;

            //Check chance
            if(selectedRaid.EventChance < UnityEngine.Random.Range(0, 100))
            {
                return true;
            }

            Log.LogDebug($"Starting raid: {selectedRaid?.Raid?.m_name}");

            //Start event.
            instance.SetRandomEvent(selectedRaid.Raid, selectedRaid.RaidCenter);
            return true;
        }

        private static List<PossibleRaid> GetPossibleRaids(RandEventSystem randomEventSystem, double currentTime)
        {
            var possibleRaids = new List<PossibleRaid>();

            RandEventSystem.RefreshPlayerEventData();

            foreach (RandomEvent randomEvent in randomEventSystem.m_events)
            {
                if (randomEvent.m_enabled && randomEvent.m_random)
                {
                    //Check for default global key handling.
                    if(!randomEventSystem.HaveGlobalKeys(randomEvent, RandEventSystem.s_playerEventDatas))
                    {
                        Log.LogDebug($"Skipping raid '{randomEvent.m_name}' due to not finding valid global keys.");
                        continue;
                    }

                    //Get event config
                    var eventData = RandomEventCache.Get(randomEvent);

                    var lastRun = eventData.LastChecked;
                    var delta = currentTime - lastRun;

                    //Check if enough time has passed
                    var eventFrequency = (eventData.Config?.RaidFrequency?.Value ?? 0) == 0
                        ? 46 //Use default frequency of 46 minutes.
                        : eventData.Config.RaidFrequency.Value;

                    if (delta < (eventFrequency * 60))
                    {
                        Log.LogTrace($"Skipping raid {randomEvent.m_name} due not enough time having passed.");
                        continue;
                    }

                    List<Vector3> possibleRaidCenterPositions = GetRaidCenters(randomEventSystem, randomEvent);

                    if (possibleRaidCenterPositions.Count == 0)
                    {
                        Log.LogDebug($"Skipping raid {randomEvent.m_name} due to player position conditions not being fulfilled (eg., biome or base-object count).");
                        continue;
                    }

                    Vector3 raidCenter = possibleRaidCenterPositions[UnityEngine.Random.Range(0, possibleRaidCenterPositions.Count)];

                    if (!RaidConditionManager.HasValidConditions(randomEvent, raidCenter))
                    {
                        Log.LogDebug($"Skipping raid {randomEvent.m_name} due not fulfilling conditions.");
                        continue;
                    }

                    possibleRaids.Add(new PossibleRaid
                    {
                        EventData = eventData,
                        EventChance = (eventData.Config?.RaidChance?.Value ?? 0) == 0
                            ? 20 //Use default of 20% chance.
                            : eventData.Config.RaidChance.Value, 
                        Raid = randomEvent,
                        RaidCenter = raidCenter
                    });
                }
            }
            return possibleRaids;
        }

        private static List<Vector3> GetRaidCenters(RandEventSystem instance, RandomEvent randomEvent) => 
            instance.GetValidEventPoints(randomEvent, RandEventSystem.s_playerEventDatas);
    }
}
