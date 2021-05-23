using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids.Serverside.StartConditions
{
    public interface IRaidStartPlayerCondition
    {
        bool IsValid(Raid raid, ZDO characterZdo);
    }
}
