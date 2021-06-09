using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.CustomRaids.Raids2.Conductors
{
    [HarmonyPatch(typeof(ZNetScene))]
    internal static class Patch_ZNetScene_AddConductorPrefab
    {
        [HarmonyPatch(nameof(ZNetScene.Awake))]
        [HarmonyPrefix]
        private static void AddConductorPrefab(ZNetScene __instance)
        {
            GameObject raidConductor = new GameObject("RaidThat_Conductor");

            raidConductor.AddComponent<ZNetView>().m_persistent = true;
            raidConductor.AddComponent<SpawnSystem>();
            raidConductor.AddComponent<ConductorBehaviour>();

            __instance.m_prefabs.Add(raidConductor);
        }
    }
}
