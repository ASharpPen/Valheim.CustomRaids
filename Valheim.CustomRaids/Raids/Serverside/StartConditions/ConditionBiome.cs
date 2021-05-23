using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids.Serverside.StartConditions
{
    public class ConditionBiome : IRaidStartCondition
    {
        public Heightmap.Biome BiomeMask { get; set; }

        public bool RequiresPlayerInfo => false;

        public bool IsValid(Raid raid)
        {
            throw new NotImplementedException();
        }
    }
}
