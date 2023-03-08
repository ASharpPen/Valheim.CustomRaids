using System.Collections.Generic;
using System.Linq;

namespace Valheim.CustomRaids.Raids.Conditions;

public class ConditionRequiredGlobalKeys : IRaidCondition
{
    public List<string> RequiredGlobalKeys { get; set; }

    public ConditionRequiredGlobalKeys(List<string> requiredGlobalKeys)
    {
        RequiredGlobalKeys = requiredGlobalKeys;
    }

    public bool IsValid(RaidContext context)
    {
        if ((RequiredGlobalKeys?.Count ?? 0) == 0)
        {
            return true;
        }

        return RequiredGlobalKeys.All(ZoneSystem.instance.GetGlobalKey);
    }
}
