
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Configuration
{
    public static class ConfigurationMerger
    {
        public static void MergeInto(this RaidEventConfigurationFile source, RaidEventConfigurationFile target)
        {
            if (source.Subsections is null)
            {
                return;
            }

            foreach (var sourceRaid in source.Subsections)
            {
                if (!sourceRaid.Value.Enabled.Value)
                {
                    continue;
                }

                if (target.Subsections.ContainsKey(sourceRaid.Key))
                {
                    Log.LogWarning($"Overlapping raid configs for {sourceRaid.Value.SectionKey}, overriding existing.");
                }
 
                target.Subsections[sourceRaid.Key] = sourceRaid.Value;
            }
        }
    }
}
