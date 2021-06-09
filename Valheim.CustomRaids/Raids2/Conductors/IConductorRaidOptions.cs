using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids2.Conductors
{
    public interface IConductorRaidOptions
    {
        string AnnouncementStart { get; set; }

        string AnnouncementEnd { get; set; }
    }
}
