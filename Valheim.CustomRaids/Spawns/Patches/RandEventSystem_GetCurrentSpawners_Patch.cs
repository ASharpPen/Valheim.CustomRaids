using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.CustomRaids.Spawns.Patches
{
    [HarmonyPatch(typeof(RandEventSystem))]
    class RandEventSystem_GetCurrentSpawners_Patch
    {
        [HarmonyPatch(nameof(RandEventSystem.GetCurrentSpawners))]
        [HarmonyPostfix]
        private static void Filter(ref List<SpawnSystem.SpawnData> __result)
        {
            List<int> toBeRemoved = new(__result.Count);

            for(int i = 0; i < __result.Count; ++i)
            {
                if(SpawnConditionManager.Filter)
            }
        }
    }
}
