using System.Collections.Generic;
using System.Linq;

namespace Valheim.CustomRaids.Raids.Conditions;

public class ConditionRequiredNotGlobalKeys : IRaidCondition
{
    public List<string> GlobalKeys { get; }

    public ConditionRequiredNotGlobalKeys(List<string> globalKeys)
    {
        GlobalKeys = globalKeys;
    }

    public bool IsValid(RaidContext context)
    {
        if (GlobalKeys is null || 
            GlobalKeys.Count == 0)
        {
            return true;
        }

        return !GlobalKeys.Any(ZoneSystem.instance.GetGlobalKey);
    }
}
