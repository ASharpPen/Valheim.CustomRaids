using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids2
{
    public static class RaidManager
    {
        private static Dictionary<string, string> RaidScheduler { get; set; } = new Dictionary<string, string>();

        private static Dictionary<string, string> RaidConductor { get; set; } = new Dictionary<string, string>();

        public static void RegisterRaid(string raidId, string schedulerId, string conductorId)
        {

        }

        public static void StartRaid(Raid raid)
        {

            if(RaidConductor.TryGetValue(raid.RaidId, out string conductorId))
            {
                //TODO: Run conductor through conductor manager.
                //TODO: Consider RPC's here, or making conductor run server side? Potentially run spawners server-side too?
            }
            else
            {
                Log.LogWarning($"Attempting to start raid {raid.RaidId}, but unable to find registered conductor. Ignoring raid start.");
            }
        }

        public static void StopRaid(string raidId)
        {

        }
    }
}
