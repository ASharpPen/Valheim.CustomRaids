using System.Collections.Generic;
using UnityEngine;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities;

namespace Valheim.CustomRaids.Spawns.Conditions;

public class ConditionNearbyPlayersNoise : ISpawnCondition
{
    private static ConditionNearbyPlayersNoise _instance;

    public static ConditionNearbyPlayersNoise Instance => _instance ??= new ConditionNearbyPlayersNoise();

    public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
    {
        if (!spawner || !spawner.transform || spawner is null || config is null || spawner.transform?.position is null)
        {
            return false;
        }

        if (IsValid(spawner.transform.position, config))
        {
            return false;
        }

        Log.LogTrace($"Filtering spawn [{config.SectionKey}] due to not having any nearby players emitting noise of {config.ConditionNearbyPlayersNoiseThreshold.Value} or higher.");
        return true;
    }

    public bool IsValid(Vector3 pos, SpawnConfiguration config)
    {
        if ((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
        {
            return true;
        }

        if ((config.ConditionNearbyPlayersNoiseThreshold?.Value ?? 0) == 0)
        {
            return true;
        }

        List<ZDO> players = PlayerUtils.GetPlayerZdosInRadius(pos, config.DistanceToTriggerPlayerConditions.Value);

        foreach (var player in players)
        {
            if (player is null)
            {
                continue;
            }

            if (player.GetFloat("noise", 0) >= config.ConditionNearbyPlayersNoiseThreshold.Value)
            {
                return true;
            }
        }

        return false;
    }
}
