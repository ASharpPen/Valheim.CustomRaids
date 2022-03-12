using UnityEngine;

namespace Valheim.CustomRaids.Utilities.Extensions;

internal static class Vector3Extensions
{
    public static Vector2i GetZoneId(this Vector3 position)
    {
        return GetZone((int)position.x, (int)position.z);
    }

    private static Vector2i GetZone(int x, int z)
    {
        return new Vector2i(Zonify(x), Zonify(z));
    }

    private static int Zonify(int coordinate)
    {
        return Mathf.FloorToInt((coordinate + 32) / 64f);
    }
}