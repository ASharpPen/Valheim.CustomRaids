using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Raids;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public interface IRaidCondition
    {
        bool IsValid(RaidContext context);
    }
}
