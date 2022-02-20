using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Spawns.Modifiers
{
    public interface ISpawnModifier
    {
        void Modify(SpawnContext context);
    }
}
