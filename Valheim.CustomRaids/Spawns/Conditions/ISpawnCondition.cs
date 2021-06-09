using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Raids2.Configs.BepInEx;
using SpawnConfiguration = Valheim.CustomRaids.Configuration.ConfigTypes.SpawnConfiguration;

namespace Valheim.CustomRaids.Spawns.Conditions
{
    public interface ISpawnCondition
    {
        bool ShouldFilter(SpawnSystem spawnSystem, SpawnConfiguration config);
    }
}
