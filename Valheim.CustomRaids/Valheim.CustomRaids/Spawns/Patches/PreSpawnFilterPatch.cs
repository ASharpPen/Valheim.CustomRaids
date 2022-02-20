using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Spawns.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class PreSpawnFilterPatch
    {
        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyPrefix]
        private static void FilterSpawners(SpawnSystem __instance, ref List<SpawnSystem.SpawnData> spawners, bool eventSpawners)
        {
            if (!eventSpawners)
            {
                return;
            }

            List<SpawnSystem.SpawnData> filtered = new List<SpawnSystem.SpawnData>();

            for (int i = 0; i < spawners.Count; ++i)
            {
                try
                {
                    if (SpawnConditionManager.Filter(__instance, spawners[i]))
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Log.LogError($"Error while checking if spawn template {spawners[i]?.m_prefab?.name} should be filtered.", e);
                }

                filtered.Add(spawners[i]);
            }

            spawners = filtered;
        }
    }
}
