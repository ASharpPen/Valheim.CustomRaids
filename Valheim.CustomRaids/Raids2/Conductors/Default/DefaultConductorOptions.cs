using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids2.Conductors.Default
{
    public class DefaultConductorOptions
    {
        public TimeSpan Interval { get; set; } = TimeSpan.FromMinutes(46);

        public float Chance { get; set; } = 20;
    }
}
