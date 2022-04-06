using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Spawns.Conditions.ModSpecific.CLLC;

public class ConditionWorldLevel : ISpawnCondition
{
    private static ConditionWorldLevel _instance;

    public static ConditionWorldLevel Instance => _instance ??= new ConditionWorldLevel();

    public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration spawnerConfig)
    {
        if (spawnerConfig.TryGet(SpawnConfigCLLC.ModName, out Config modConfig))
        {
            if (modConfig is SpawnConfigCLLC config)
            {
                int worldLevel = CreatureLevelControl.API.GetWorldLevel();

                if (config.ConditionWorldLevelMin.Value >= 0 && worldLevel < config.ConditionWorldLevelMin.Value)
                {
                    Log.LogTrace($"Filtering spawn [{config.SectionKey}] due to CLLC world level being too low. {worldLevel} < {config.ConditionWorldLevelMin}.");
                    return true;
                }

                if (config.ConditionWorldLevelMax.Value >= 0 && worldLevel > config.ConditionWorldLevelMax.Value)
                {
                    Log.LogTrace($"Filtering spawn [{config.SectionKey}] due to CLLC world level being too high. {worldLevel} > {config.ConditionWorldLevelMax}.");
                    return true;
                }
            }
        }

        return false;
    }
}
