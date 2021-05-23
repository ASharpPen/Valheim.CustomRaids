using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids2
{
    public static class RaidManager
    {
        private static Dictionary<string, string> RaidScheduler { get; set; } = new Dictionary<string, string>();

        private static Dictionary<string, string> RaidConductor { get; set; } = new Dictionary<string, string>();

        public static void RegisterRaid(string raidId, string schedulerId, string conductorId)
        {

        }

        public static void StartRaid(string raidId)
        {
        }

        public static void StopRaid(string raidId)
        {

        }
    }
}
