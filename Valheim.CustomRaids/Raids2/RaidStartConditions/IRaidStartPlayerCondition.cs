using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Raids2.Schedulers;

namespace Valheim.CustomRaids.Raids2.RaidStartConditions
{
    public interface IRaidStartPlayerCondition
    {
        bool IsValid<T>(T raid, ZDO playerZdo) where T : BaseSchedulerRaid;
    }
}
