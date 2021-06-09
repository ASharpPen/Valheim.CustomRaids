using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids2.Conductors
{
    public static class ConductorManager
    {
        private static Dictionary<string, ConductorRegistration> Conductors = new Dictionary<string, ConductorRegistration();
        private static Dictionary<string, ConductorRaidRegistration> ConductorRaids = new Dictionary<string, ConductorRaidRegistration>();

        public static void RegisterConductor<TConductor, TOptions>(string conductorId, TOptions options) 
            where TConductor : IConductor, IRequireOptions<TOptions>
            where TOptions : IConductorOptions
        {
            Conductors[conductorId] = new ConductorRegistration<TConductor, TOptions>
            {
                Options = options
            };
        }

        public static void RegisterConductorRaid<TConductor, TRaid>(string conductorId, string raidId, TRaid raid)
            where TConductor : IConductor, IRequireRaidOptions<TRaid>
            where TRaid : IConductorRaidOptions
        {
            ConductorRaids[raidId] = new ConductorRaidRegistration<TConductor, TRaid>
            {
                ConductorId = conductorId,
                Raid = raid
            };
        }

        public static IConductor? GetConductor(string raidId)
        {
            if(ConductorRaids.TryGetValue(raidId, out ConductorRaidRegistration raid))
            {
                if (Conductors.TryGetValue(raid.ConductorId, out ConductorRegistration registration))
                {
                    var conductor = Activator.CreateInstance(registration.ConductorType) as IConductor;
                    registration.Configure(conductor);
                    raid.SetRaid(conductor);

                    return conductor;
                }
                else
                {
                    Log.LogWarning($"Attempting to retrieve un-registered conductor '{raid.ConductorId}'.");
                    return null;
                }
            }
            else
            {
                Log.LogWarning($"Attempting to retrieve un-registered raid '{raidId}'.");

                return null;
            }
        }

        public static void StartRaid(Raid raid)
        {
            if (ConductorRaids.TryGetValue(raid.RaidId, out ConductorRaidRegistration registeredRaid))
            {
                if (Conductors.TryGetValue(registeredRaid.ConductorId, out ConductorRegistration registeredConductor))
                {
                    var gameObject = CreateAndStartRaidConductor(registeredConductor, registeredRaid, raid);
                }
            }
            else
            {
                Log.LogWarning($"Attempting to start un-registered raid '{raid.RaidId}'.");
            }

        }

        private static GameObject CreateAndStartRaidConductor(ConductorRegistration conductorRegistration, ConductorRaidRegistration raidRegistration, Raid raid)
        {
            var controller = UnityEngine.Object.Instantiate<GameObject>(ZoneSystem.instance.m_zoneCtrlPrefab, raid.RaidCenter, Quaternion.identity);
            var conductorComponent = controller.AddComponent(conductorRegistration.ConductorType) as IConductor;

            conductorRegistration.Configure(conductorComponent);
            raidRegistration.SetRaid(conductorComponent);

            // TODO: Check if this is in any way needed
            controller.GetComponent<ZNetView>().GetZDO().SetPGWVersion(ZoneSystem.instance.m_pgwVersion);

            conductorComponent.Start(raid);

            return controller;
        }

        private abstract class ConductorRegistration
        {
            public abstract Type ConductorType { get; }

            public virtual void Configure(IConductor conductor)
            {
            }
        }

        private class ConductorRegistration<T, TOptions> : ConductorRegistration 
            where T : IConductor, IRequireOptions<TOptions>
            where TOptions : IConductorOptions
        {
            public override Type ConductorType => typeof(T);

            public TOptions Options;

            public override void Configure(IConductor conductor)
            {
                if(conductor is T cond)
                {
                    cond.SetOptions(Options);
                }
                else
                {
                    // This really shouldn't happen. Just verifying that no-one is slipping in odd things.
                    Log.LogWarning($"Attempting to configure conductor registered as {typeof(T).Name} as if it was {conductor.GetType().Name}. Skipping configuration.");
                    return;
                }
            }
        }

        private abstract class ConductorRaidRegistration
        {
            public string ConductorId { get; set; }

            public virtual void SetRaid(IConductor conductor)
            {
            }
        }

        private class ConductorRaidRegistration<T, TRaid> : ConductorRaidRegistration
            where T : IConductor, IRequireRaidOptions<TRaid>
            where TRaid : IConductorRaidOptions
        {
            public TRaid Raid;

            public override void SetRaid(IConductor conductor)
            {
                if (conductor is T cond)
                {
                    cond.SetConductorRaid(Raid);
                }
                else
                {
                    // This really shouldn't happen. Just verifying that no-one is slipping in odd things.
                    Log.LogWarning($"Attempting to configure conductor registered as {typeof(T).Name} as if it was {conductor.GetType().Name}. Skipping configuration.");
                    return;
                }
            }
        }
    }
}
