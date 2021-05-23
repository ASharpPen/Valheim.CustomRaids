using System.Collections.Generic;
using Valheim.CustomRaids.Raids2.RaidStartConditions;

namespace Valheim.CustomRaids.Raids2.Schedulers
{
    public abstract class BaseSchedulerRaid
    {
        public string RaidId { get; set; }

        public List<IRaidStartCondition> StartConditions { get; set; } = new List<IRaidStartCondition>();

        public List<IRaidStartPlayerCondition> StartPlayerConditions { get; set; } = new List<IRaidStartPlayerCondition>();
    }
}
