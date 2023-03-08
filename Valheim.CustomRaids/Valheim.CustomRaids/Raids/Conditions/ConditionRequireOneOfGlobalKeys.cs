using System.Collections.Generic;
using System.Linq;

namespace Valheim.CustomRaids.Raids.Conditions
{
    public class ConditionRequireOneOfGlobalKeys : IRaidCondition
    {
        public List<string> GlobalKeys { get; set; }

        public ConditionRequireOneOfGlobalKeys(List<string> globalKeys)
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

            return GlobalKeys.Any(ZoneSystem.instance.GetGlobalKey);
        }
    }
}
