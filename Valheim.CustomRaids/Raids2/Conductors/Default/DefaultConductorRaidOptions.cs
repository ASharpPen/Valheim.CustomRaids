using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Spawns;

namespace Valheim.CustomRaids.Raids2.Conductors.Default
{
    public class DefaultConductorRaidOptions : BaseRaidOptions
    {
        public float Duration { get; set; }

        public List<SpawnTemplate> SpawnTemplates { get; set; }
    }
}
