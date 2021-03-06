﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Spawns.Conditions
{
    public class ConditionNearbyPlayersCarryValue : ISpawnCondition
    {
        private static ConditionNearbyPlayersCarryValue _instance;

        public static ConditionNearbyPlayersCarryValue Instance
        {
            get
            {
                return _instance ??= new ConditionNearbyPlayersCarryValue();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnSystem.SpawnData spawn, SpawnConfiguration config)
        {
            if (!spawner || !spawner.transform || spawner is null || config is null || spawner.transform?.position is null)
            {
                return false;
            }

            if ((config.DistanceToTriggerPlayerConditions?.Value ?? 0) <= 0)
            {
                return false;
            }

            List<Player> players = new List<Player>();
            Player.GetPlayersInRange(spawner.transform.position, config.DistanceToTriggerPlayerConditions.Value, players);

            var requiredSum = config.ConditionNearbyPlayersCarryValue?.Value ?? 0;
            var valueSum = 0;

            if ((players?.Count ?? 0) == 0)
            {
                Log.LogTrace($"Ignoring world config {config.Name} due to condition {nameof(ConditionNearbyPlayersCarryValue)}.");
                return true;
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

                    if (item.m_shared is null)
                    {
                        continue;
                    }

                    valueSum += item.GetValue();
                }
            }

            if (valueSum < requiredSum)
            {
                Log.LogTrace($"Filtering {config.Name} due to summed value of nearby players inventory.");
                return true;
            }

            return false;
        }
    }
}
