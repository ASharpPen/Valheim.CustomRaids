using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids.Serverside.StartConditions
{
    public interface IRaidStartCondition
    {
        bool IsValid(Raid raid);
    }
}
