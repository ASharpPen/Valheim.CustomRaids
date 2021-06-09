using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Raids2.Spawns.Conditions;
using Valheim.CustomRaids.Raids2.Spawns.Modifiers;

namespace Valheim.CustomRaids.Raids2.Spawns
{
    public class SpawnTemplate
    {
        public List<ISpawnCondition> Conditions { get; set; }

        public List<ISpawnModifier> Modifiers { get; set; }

        public SpawnDataSettings DefaultSettings { get; set; } = new SpawnDataSettings();
    }
}
