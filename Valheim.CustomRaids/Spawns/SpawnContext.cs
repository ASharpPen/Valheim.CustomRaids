﻿
using UnityEngine;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;

namespace Valheim.CustomRaids.Spawns
{
    public class SpawnContext
    {
        public SpawnSystem SpawnSystem { get; set; }
        public GameObject Spawn { get; set; }
        public SpawnSystem.SpawnData Spawner { get; set; }
        public SpawnConfiguration Config { get; set; }
        public RaidEventConfiguration RaidConfig { get; set; }
    }
}
