using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;
using Valheim.CustomRaids.Utilities.Extensions;
using SpawnConfiguration = Valheim.CustomRaids.Configuration.ConfigTypes.SpawnConfiguration;

namespace Valheim.CustomRaids.Spawns.Conditions
{
    public class ConditionNearbyPlayersCarryItem : ISpawnCondition
    {
        private static ConditionNearbyPlayersCarryItem _instance;

        public static ConditionNearbyPlayersCarryItem Instance
        {
            get
            {
                return _instance ??= new ConditionNearbyPlayersCarryItem();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnConfiguration config)
        {
            if (!spawner || !spawner.transform || spawner is null || config is null || spawner.transform?.position is null)
            {
                return false;
            }

            if ((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(config.ConditionNearbyPlayerCarriesItem?.Value))
            {
                return false;
            }

            List<Player> players = new List<Player>();
            Player.GetPlayersInRange(spawner.transform.position, config.DistanceToTriggerPlayerConditions.Value, players);

            var itemsLookedFor = config.ConditionNearbyPlayerCarriesItem?.Value?.SplitByComma(true)?.ToHashSet();

            if (itemsLookedFor is null)
            {
                return false;
            }

            foreach (var player in players.Where(x => x is not null && x))
            {
                var items = player.GetInventory()?.GetAllItems();

                if (items is null)
                {
                    continue;
                }

                foreach (var item in items)
                {
                    if (item is null)
                    {
                        continue;
                    }

                    string itemName = item.m_dropPrefab?.name?.Trim()?.ToUpperInvariant();

                    if (string.IsNullOrWhiteSpace(itemName))
                    {
                        continue;
                    }

                    if (itemsLookedFor.Contains(itemName))
                    {
                        return false;
                    }
                }
            }

            Log.LogTrace($"Filtering spawn {spawn.m_name} due to not finding any required items on nearby players.");
            return true;
        }
    }
}
