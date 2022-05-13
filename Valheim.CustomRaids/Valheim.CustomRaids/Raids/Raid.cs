using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Raids.Actions;
using Valheim.CustomRaids.Raids.Conditions;

namespace Valheim.CustomRaids.Raids
{
    public class Raid
    {
        public Raid(string raidName)
        {
            Name = raidName;
        }

        public string Name { get; set; }

        public List<IRaidCondition> Conditions { get; set; } = new(0);

        public List<IRaidAction> OnStopActions { get; set; } = new(0);
    }
}
