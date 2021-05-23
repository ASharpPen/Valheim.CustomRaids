using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Raids.Extensions
{
    public static class RandEventSystemExtensions
    {
        public static bool HasActiveEvent(this RandEventSystem instance)
        {
            return instance.GetActiveEvent() is not null || instance.GetCurrentRandomEvent() is not null);
        }
    }
}
