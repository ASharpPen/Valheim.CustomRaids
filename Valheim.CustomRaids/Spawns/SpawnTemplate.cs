using System.Collections.Generic;
using Valheim.CustomRaids.Spawns.Conditions;
using Valheim.CustomRaids.Spawns.Modifiers;

namespace Valheim.CustomRaids.Spawns
{
    public class SpawnTemplate
    {
        public List<ISpawnCondition> Conditions { get; set; }

        public List<ISpawnModifier> Modifiers { get; set; }

        public SpawnDataSettings DefaultSettings { get; set; } = new SpawnDataSettings();
    }
}
