using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                    SendCurrentRandomEvent(__instance);
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
            SetRandomEvent(instance, selectedRaid.Raid, selectedRaid.RaidCenter);
            return true;
        }

        private static List<PossibleRaid> GetPossibleRaids(RandEventSystem randomEventSystem, double currentTime)
        {
            var possibleRaids = new List<PossibleRaid>();

            List<ZDO> allCharacterZDOS = ZNet.instance.GetAllCharacterZDOS();

            foreach (RandomEvent randomEvent in randomEventSystem.m_events)
            {
                if (randomEvent.m_enabled && randomEvent.m_random)
                {
                    //Check for default global key handling. Only check standard ValidGlobalKeys if enhanced keys are NOT installed.
                    if(!CustomRaidPlugin.EnhancedProgressTrackerInstalled && !ValidGlobalKeys(randomEventSystem, randomEvent))
                    {
                        Log.LogDebug($"Skipping raid '{randomEvent.m_name}' due to not finding valid global keys.");
                        continue;
                    }

                    //Get event config
                    var eventData = RandomEventCache.Get(randomEvent);

                    var lastRun = eventData.LastChecked;
                    var delta = currentTime - lastRun;

                    //Check if enough time has passed
                    if(delta < (eventData.Config?.RaidFrequency?.Value ?? 46) * 60) //Use default frequency of 46 minutes if something is wrong here.
                    {
                        Log.LogTrace($"Skipping raid {randomEvent.m_name} due not enough time having passed.");
                        continue;
                    }

                    List<Vector3> possibleRaidCenterPositions = GetRaidCenters(randomEventSystem, randomEvent, allCharacterZDOS.ToList());

                    if (possibleRaidCenterPositions.Count != 0)
                    {
                        Vector3 raidCenter = possibleRaidCenterPositions[UnityEngine.Random.Range(0, possibleRaidCenterPositions.Count)];

                        if (!RaidConditionManager.HasValidConditions(randomEvent, raidCenter))
                        {
                            continue;
                        }

                        possibleRaids.Add(new PossibleRaid
                        {
                            EventData = eventData,
                            EventChance = eventData.Config?.RaidChance?.Value ?? 20, //Use default of 20% chance if something is wrong.
                            Raid = randomEvent,
                            RaidCenter = raidCenter
                        });
                    }
                }
            }
            return possibleRaids;
        }

        private static MethodInfo HaveGlobalKeys = AccessTools.Method(typeof(RandEventSystem), "HaveGlobalKeys", new[] { typeof(RandomEvent) });

        private static bool ValidGlobalKeys(RandEventSystem instance, RandomEvent randomEvent)
        {
            return (bool)(HaveGlobalKeys.Invoke(instance, new object[] { randomEvent }) ?? false);
        }

        private static MethodInfo GetValidEventPoints = AccessTools.Method(typeof(RandEventSystem), "GetValidEventPoints", new[] { typeof(RandomEvent), typeof(List<ZDO>)});

        private static List<Vector3> GetRaidCenters(RandEventSystem instance, RandomEvent randomEvent, List<ZDO> characterZDOs)
        {
            return GetValidEventPoints.Invoke(instance, new object[] { randomEvent, characterZDOs }) as List<Vector3>;
        }

        private static MethodInfo SetRandomEventMethod = AccessTools.Method(typeof(RandEventSystem), "SetRandomEvent", new[] { typeof(RandomEvent), typeof(Vector3) });

        private static void SetRandomEvent(RandEventSystem instance, RandomEvent randomEvent, Vector3 raidCenter)
        {
            SetRandomEventMethod.Invoke(instance, new object[] { randomEvent, raidCenter });
        }

        private static MethodInfo SendCurrentRandomEventMethod = AccessTools.Method(typeof(RandEventSystem), "SendCurrentRandomEvent");

        private static void SendCurrentRandomEvent(RandEventSystem instance)
        {
            SendCurrentRandomEventMethod.Invoke(instance, new object[0]);
        }
    }
}
