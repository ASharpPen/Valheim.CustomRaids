﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;
using SpawnConfiguration = Valheim.CustomRaids.Configuration.ConfigTypes.SpawnConfiguration;

namespace Valheim.CustomRaids.Spawns.Conditions
{
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

        public bool ShouldFilter(SpawnSystem spawnSystem, SpawnConfiguration spawnConfig)
        {
            if (!spawnSystem || spawnSystem is null || spawnConfig is null)
            {
                return false;
            }

            if (IsValid(spawnSystem.transform.position, spawnConfig))
            {
                return false;
            }

            Log.LogTrace($"Filtering {spawnConfig.Name} due to distance to center.");
            return true;
        }

        public bool IsValid(Vector3 position, SpawnConfiguration config)
        {
            var distance = position.magnitude;

            if (distance < config.ConditionDistanceToCenterMin.Value)
            {
                Log.LogTrace($"Ignoring world config {config.Name} due to distance less than min.");
                return false;
            }

            if (config.ConditionDistanceToCenterMax.Value > 0 && distance > config.ConditionDistanceToCenterMax.Value)
            {
                Log.LogTrace($"Ignoring world config {config.Name} due to distance greater than max.");
                return false;
            }

            return true;
        }
    }
}
