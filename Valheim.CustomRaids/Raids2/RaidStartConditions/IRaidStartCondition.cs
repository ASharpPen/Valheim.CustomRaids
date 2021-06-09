using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Raids2.Schedulers;

namespace Valheim.CustomRaids.Raids2.RaidStartConditions
{
    public interface IRaidStartCondition
    {
        bool IsValid<T>(T raid) where T : BaseSchedulerRaid;
    }
}
