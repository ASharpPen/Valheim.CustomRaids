using System.Collections.Generic;
using HarmonyLib;
using Valheim.CustomRaids.Resetter;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.World;

internal class ZoneManager
{
    private static Dictionary<Vector2i, ZoneHeightmap> HeightmapsLoaded = new();
    private static Dictionary<Vector2i, ZoneSimulated> SimulatedCache = new();

    static ZoneManager()
    {
        StateResetter.Subscribe(() =>
        {
            SimulatedCache = new();

        });
    }

    public static IZone GetZone(Vector2i zoneId)
    {
        if (HeightmapsLoaded.TryGetValue(zoneId, out var cached))
        {
            return cached;
        }

        if (SimulatedCache.TryGetValue(zoneId, out var cachedSimulated))
        {
            return cachedSimulated;
        }

        return SimulatedCache[zoneId] = new ZoneSimulated(zoneId);
    }

    [HarmonyPatch]
    private static class PatchHeightmap
    {
        [HarmonyPatch(typeof(Heightmap), nameof(Heightmap.Regenerate))]
        [HarmonyPostfix]
        private static void Record(Heightmap __instance)
        {
            // Sometimes distant heightmaps are generated, we need to skip those.
            // Not sure whats going on here, but they get generated in inconsistent positions
            // that do not fit on the zone grid.
            if (!__instance.m_isDistantLod)
            {
                return;
            }

            var zoneId = __instance.gameObject.transform.position.GetZoneId();

            HeightmapsLoaded[zoneId] = new ZoneHeightmap(__instance);
        }

        [HarmonyPatch(typeof(Heightmap), nameof(Heightmap.OnDestroy))]
        [HarmonyPostfix]
        private static void RemoveRecord(Heightmap __instance)
        {
            var zoneId = __instance.gameObject.transform.position.GetZoneId();
            HeightmapsLoaded.Remove(zoneId);
        }
    }
}
