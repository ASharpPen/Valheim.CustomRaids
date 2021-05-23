using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Utilities.Extensions
{
    public static class ZdoExtensions
    {
        public static string GetPlayerName(this ZDO zdo)
        {
            return zdo.GetString("playerName", null);
        }
    }
}
