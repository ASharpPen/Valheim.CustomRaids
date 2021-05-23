using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Caches;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Spawns.Conditions
{
    public class ConditionNearbyPlayersNoise : ISpawnCondition
    {
        private static ConditionNearbyPlayersNoise _instance;

        public static ConditionNearbyPlayersNoise Instance
        {
            get
            {
                return _instance ??= new ConditionNearbyPlayersNoise();
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

            if (config.ConditionNearbyPlayersNoiseThreshold.Value == 0)
            {
                return false;
            }

            List<Player> players = new List<Player>();
            Player.GetPlayersInRange(spawner.transform.position, config.DistanceToTriggerPlayerConditions.Value, players);

            foreach (var player in players.Where(x => x && x is not null))
            {
                Log.LogTrace($"Checking noise of player {player.GetPlayerName()}");

                var zdo = ZdoCache.GetZDO(player.gameObject);
                if (zdo is not null)
                {
                    var noise = zdo.GetFloat("noise", 0);

                    if (noise >= config.ConditionNearbyPlayersNoiseThreshold.Value)
                    {
                        return false;
                    }
                    else
                    {
                        Log.LogTrace($"Player {player.GetPlayerName()} have accumulated noise of {noise}");
                    }
                }
            }

            Log.LogTrace($"Ignoring spawn {config.Name} due to not having any nearby noisy players.");
            return true;
        }
    }
}
