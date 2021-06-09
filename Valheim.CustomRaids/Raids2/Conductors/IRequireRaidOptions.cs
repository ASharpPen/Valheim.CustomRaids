using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids2.Conductors
{
    public interface IRequireRaidOptions<T> where T : IConductorRaidOptions
    {
        void SetConductorRaid(T raid);
    }
}
