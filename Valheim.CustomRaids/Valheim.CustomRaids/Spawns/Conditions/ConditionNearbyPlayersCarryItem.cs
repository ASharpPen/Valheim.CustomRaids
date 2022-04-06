using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities.Extensions;
using Valheim.CustomRaids.Utilities;

namespace Valheim.CustomRaids.Spawns.Conditions;

public class ConditionNearbyPlayersCarryItem : ISpawnCondition
{
    private static ConditionNearbyPlayersCarryItem _instance;

    public static ConditionNearbyPlayersCarryItem Instance => _instance ??= new ConditionNearbyPlayersCarryItem();

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

        Log.LogTrace($"Filtering spawn [{config.SectionKey}] due to not finding any required items on nearby players.");
        return true;
    }

    public bool IsValid(Vector3 pos, SpawnConfiguration config)
    {
        if ((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
        {
            return true;
        }

        if (string.IsNullOrWhiteSpace(config.ConditionNearbyPlayerCarriesItem?.Value))
        {
            return true;
        }

        List<Player> players = PlayerUtils.GetPlayersInRadius(pos, config.DistanceToTriggerPlayerConditions.Value);

        var itemsSearchedFor = config.ConditionNearbyPlayerCarriesItem?.Value?.SplitByComma(true)?.ToHashSet();

        if (itemsSearchedFor?.Any() != true)
        {
            return true;
        }

        foreach (var player in players)
        {
            if (player.IsNull())
            {
                continue;
            }

            var items = player.GetInventory()?.GetAllItems() ?? new(0);

            foreach (var item in items)
            {
                var itemPrefab = item?.m_dropPrefab;

                if (itemPrefab.IsNull())
                {
                    continue;
                }

                string itemName = itemPrefab.name.Trim().ToUpperInvariant();

                if (string.IsNullOrWhiteSpace(itemName))
                {
                    continue;
                }

                if (itemsSearchedFor.Contains(itemName))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
