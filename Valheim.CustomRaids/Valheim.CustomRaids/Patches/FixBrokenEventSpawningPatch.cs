using HarmonyLib;
using UnityEngine;

namespace Valheim.CustomRaids.Patches;

/// <summary>
/// Temporary fixes for broken vanilla code.
/// </summary>
[HarmonyPatch]
internal static class FixBrokenEventSpawningPatch
{
    [HarmonyPatch(typeof(RandEventSystem), nameof(RandEventSystem.CheckValidEnvironment))]
    [HarmonyPostfix]
    private static void FixEnvironmentCheck(RandomEvent ev, ref bool __result)
    {
        if (string.IsNullOrWhiteSpace(ev.m_requireEnvironment))
        {
            __result = true;
        }
    }

    [HarmonyPatch(typeof(RandEventSystem), nameof(RandEventSystem.IsInsideRandomEventArea))]
    [HarmonyPostfix]
    private static void FixIsInsideRandomEventArea(RandEventSystem __instance, RandomEvent re, Vector3 position , ref bool __result)
    {
        if (re.m_minAltitude == 0 && re.m_maxAltitude == 0)
        {
            __result = Utils.DistanceXZ(position, re.m_pos) < __instance.m_randomEventRange;
        }
        else if (position.y < re.m_minAltitude)
        {
            __result = false;
        }
        else if (position.y > re.m_maxAltitude)
        {
            __result = false;
        }
        else
        {
            __result = Utils.DistanceXZ(position, re.m_pos) < __instance.m_randomEventRange;
        }
    }
}
