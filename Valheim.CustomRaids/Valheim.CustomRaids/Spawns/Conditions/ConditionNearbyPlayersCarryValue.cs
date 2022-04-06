using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.Spawns.Conditions;

public class ConditionNearbyPlayersCarryValue : ISpawnCondition
{
    private static ConditionNearbyPlayersCarryValue _instance;

    public static ConditionNearbyPlayersCarryValue Instance => _instance ??= new ConditionNearbyPlayersCarryValue();

    public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
    {
        if (!spawner || !spawner.transform || spawner is null || config is null || spawner.transform?.position is null)
        {
            return false;
        }

        if (Isvalid(spawner.transform.position, config))
        {
            return false;
        }

        Log.LogTrace($"Filtering spawn [{config.SectionKey}] due to condition nearby players carry value {config.ConditionNearbyPlayersCarryValue.Value}.");
        return true;
    }

    public bool Isvalid(Vector3 pos, SpawnConfiguration config)
    {
        if ((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
        {
            return true;
        }

        if ((config.ConditionNearbyPlayersCarryValue?.Value ?? 0) <= 0)
        {
            return true;
        }

        List<Player> players = PlayerUtils.GetPlayersInRadius(pos, config.DistanceToTriggerPlayerConditions.Value);

        var valueSum = 0;

        foreach (var player in players)
        {
            if (player.IsNull())
            {
                continue;
            }

            var items = player.GetInventory()?.GetAllItems() ?? Enumerable.Empty<ItemDrop.ItemData>();

#if DEBUG && VERBOSE
                Log.LogTrace($"Player '{player.m_name}': {items.Join(x => x.m_shared?.m_name)}");
#endif

            foreach (var item in items)
            {
                valueSum += item?.GetValue() ?? 0;
            }
        }

        return valueSum >= config.ConditionNearbyPlayersCarryValue.Value;
    }
}
