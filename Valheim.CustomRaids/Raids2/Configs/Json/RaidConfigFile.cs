using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Raids2.Conductors;
using Valheim.CustomRaids.Raids2.Schedulers;

namespace Valheim.CustomRaids.Raids2.Configs.Json
{
    public class RaidConfigFile
    {
        public string RaidId { get; set; }

        public IRaidScheduler Scheduler { get; set; }

        public IConductor Conductor { get; set; }
    }

    public class RaidScheduler
    {

    }

    public class RaidConductor
    {

    }
}
