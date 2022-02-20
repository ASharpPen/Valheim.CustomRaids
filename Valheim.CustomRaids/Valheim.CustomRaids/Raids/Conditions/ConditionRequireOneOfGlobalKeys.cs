using System;
using Valheim.CustomRaids.Compatibilities;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids;

namespace Valheim.CustomRaids.Conditions
{
    internal static class ConditionRequireOneOfGlobalKeys
    {
        public static bool ShouldFilter(RandomEvent randomEvent, string playerName)
        {
            var raidConfig = RandomEventCache.GetConfig(randomEvent);

            if(raidConfig is null)
            {
                return false;
            }

            //Check key conditions.
            if (raidConfig.RequireOneOfGlobalKeys.Value.Length > 0)
            {
                var keys = raidConfig.RequireOneOfGlobalKeys.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

#if DEBUG
                Log.LogInfo("Found RequireOneOfGlobalKeys keys: ");
                foreach (var key in keys)
                {
                    Log.LogInfo("\t" + key);
                }
#endif
                bool foundRequiredKey = false;
                foreach (var key in keys)
                {
                    if (HasKey(key.Trim(), playerName))
                    {
#if DEBUG
                        Log.LogInfo("Found RequiredOneOfKey: " + key);
#endif

                        foundRequiredKey = true;
                        break;
                    }
                }

                if (!foundRequiredKey)
                {
#if DEBUG
                    Log.LogDebug($"Unable to find any of the keys {raidConfig.RequireOneOfGlobalKeys.Value}");
#endif
                    return true;
                }
            }

            return false;
        }

        private static bool HasKey(string key, string playerName)
        {
            if(CustomRaidPlugin.EnhancedProgressTrackerInstalled)
            {
                return EnhancedProgressTrackerCompatibilities.HaveGlobalKey(playerName, key);
            }
            else
            {
                return ZoneSystem.instance.GetGlobalKey(key.Trim());
            }
        }
    }
}
