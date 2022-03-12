using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Configuration.ConfigTypes;

namespace Valheim.CustomRaids.Spawns.Conditions
{
    public interface ISpawnCondition
    {
        bool ShouldFilter(SpawnSystem spawnSystem, SpawnSystem.SpawnData spawn, SpawnConfiguration config);
    }
}
