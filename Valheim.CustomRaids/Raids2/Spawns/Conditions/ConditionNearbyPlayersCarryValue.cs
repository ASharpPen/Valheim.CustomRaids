using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids2.Spawns.Conditions
{
    public class ConditionNearbyPlayersCarryValue : ISpawnCondition
    {
        public float RadiusToSearch { get; set; } = 100;
        public float WealthRequired { get; set; }

        public bool IsValid(Vector3 position)
        {
            if (RadiusToSearch <= 0)
            {
                return false;
            }

            List<Player> players = new List<Player>();
            Player.GetPlayersInRange(position, RadiusToSearch, players);

            var valueSum = 0;

            if ((players?.Count ?? 0) == 0)
            {
                Log.LogTrace($"Ignoring spawn due to condition {nameof(ConditionNearbyPlayersCarryValue)}.");
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

            if (valueSum < WealthRequired)
            {
                Log.LogTrace($"Filtering spawn due to {nameof(ConditionNearbyPlayersCarryValue)}.");
                return true;
            }

            return false;
        }
    }
}
