using UnityEngine;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Spawns.Conditions;

public class ConditionDistanceToCenter : ISpawnCondition
{
    private static ConditionDistanceToCenter _instance;

    public static ConditionDistanceToCenter Instance
    {
        get
        {
            return _instance ??= new ConditionDistanceToCenter();
        }
    }

    public bool ShouldFilter(SpawnSystem spawnSystem, SpawnSystem.SpawnData spawner, SpawnConfiguration spawnConfig)
    {
        if (!spawnSystem || spawnSystem is null || spawner is null || spawnConfig is null)
        {
            return false;
        }

        if (IsValid(spawnSystem.transform.position, spawnConfig))
        {
            return false;
        }

        Log.LogTrace($"Filtering spawn [{spawnConfig.SectionKey}] due to condition distance to center.");
        return true;
    }

    public bool IsValid(Vector3 position, SpawnConfiguration config)
    {
        var distance = position.magnitude;

        if (distance < config.ConditionDistanceToCenterMin.Value)
        {
            return false;
        }

        if (config.ConditionDistanceToCenterMax.Value > 0 && distance > config.ConditionDistanceToCenterMax.Value)
        {
            return false;
        }

        return true;
    }
}
