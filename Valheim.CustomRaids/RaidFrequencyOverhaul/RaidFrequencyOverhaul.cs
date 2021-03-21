using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.CustomRaids.ConfigurationTypes;
using Valheim.CustomRaids.Patches;

namespace Valheim.CustomRaids.RaidFrequencyOverhaul
{
    [HarmonyPatch(typeof(RandEventSystem))]
    public static class RaidFrequencyOverhaul
    {
        /// <summary>
        /// Take control over raid checking.
        /// </summary>
        [HarmonyPatch("UpdateRandomEvent")]
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

            if (ZNet.instance.IsServer())
            {
                CheckAndStartRaids(__instance, dt, ref ___m_eventTimer);

                //Update current event timer. 
                //Just using the default solution here... It apparently sends out an update every 2 seconds with the current raid event.
                ___m_sendTimer += dt;
                if (___m_sendTimer > 2f)
                {
                    ___m_sendTimer = 0f;
                    SendCurrentRandomEvent(__instance);
                }
            }

            return false;
        }

        private static void CheckAndStartRaids(RandEventSystem instance, float dt, ref float m_eventTimer)
        {
            m_eventTimer += dt;

            //Check if we have passed the minimum time between raids.
            if (m_eventTimer < ConfigurationManager.GeneralConfig.MinimumTimeBetweenRaids.Value * 60) //EventTimer is in seconds.
            {
                return;
            }

            var time = ZNet.instance.GetTimeSeconds();

            //Get list of possible raids
            var possibleRaids = GetPossibleRaids(instance, time);

#if DEBUG
            Log.LogDebug($"Raids possible to spawn: {possibleRaids?.Count ?? 0}");
#endif

            //Filter by rolling chance on each.
            possibleRaids = possibleRaids
                .Where(x => UnityEngine.Random.Range(0, 100) <= x.EventChance)
                .ToList();

#if DEBUG
            Log.LogDebug($"Raids that passed chance check: {possibleRaids?.Count ?? 0}");
#endif

            if (possibleRaids.Count == 0)
            {
                return;
            }

            //Select one randomly
            var selectedRaid = possibleRaids[UnityEngine.Random.Range(0, possibleRaids.Count)];

            //Set event timer.
            m_eventTimer = 0;
            selectedRaid.EventData.LastRun = time;

#if DEBUG
            Log.LogDebug($"Starting raid: {selectedRaid?.Raid?.m_name}");
#endif

            //Start event.
            SetRandomEvent(instance, selectedRaid.Raid, selectedRaid.RaidCenter);
        }

        private static List<PossibleRaid> GetPossibleRaids(RandEventSystem randomEventSystem, double currentTime)
        {
            var possibleRaids = new List<PossibleRaid>();

            List<ZDO> allCharacterZDOS = ZNet.instance.GetAllCharacterZDOS();

            foreach (RandomEvent randomEvent in randomEventSystem.m_events)
            {
                if (randomEvent.m_enabled && randomEvent.m_random && ValidGlobalKeys(randomEventSystem, randomEvent))
                {
                    //Get event config
                    var eventData = RandomEventCache.Get(randomEvent);

                    var lastRun = eventData.LastRun;
                    var delta = currentTime - lastRun;

                    //Check if enough time has passed
                    if(delta < (eventData.Config?.RaidFrequency?.Value ?? 46) * 60) //Use default frequency of 46 minutes if something is wrong here.
                    {
#if DEBUG
                        Log.LogDebug($"Skipping raid {randomEvent.m_name} due not enough time having passed.");
#endif
                        continue;
                    }

                    List<Vector3> possibleRaidCenterPositions = GetRaidCenters(randomEventSystem, randomEvent, allCharacterZDOS);
                    if (possibleRaidCenterPositions.Count != 0)
                    {
                        Vector3 raidCenter = possibleRaidCenterPositions[UnityEngine.Random.Range(0, possibleRaidCenterPositions.Count)];
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
