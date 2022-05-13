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

    public static bool WithinHorizontalDistance(this Vector3 pos1, Vector3 pos2, float distance)
    {
        float x = pos1.x - pos2.x;
        float z = pos1.z - pos2.z;
        return x * x + z * z < distance * distance;
    }
}