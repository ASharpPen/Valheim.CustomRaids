#define VERBOSE

using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.World;

namespace Valheim.CustomRaids.Raids.Conditions;

internal class ConditionAltitude : IRaidCondition
{
    public float? Min { get; set; }

    public float? Max { get; set; }

    public bool IsValid(RaidContext context)
{
        var zone = ZoneManager.GetZone(ZoneSystem.instance.GetZone(context.Position));
        var floorAltitude = zone.Height(context.Position) - ZoneSystem.instance.m_waterLevel;

#if DEBUG && VERBOSE
        Log.LogTrace($"Altitude: {floorAltitude}, Min: {Min}, Max: {Max}");
#endif

        if (Min is not null && Min > floorAltitude)
        {
            return false;
        }

        if (Max is not null && Max < floorAltitude)
        {
            return false;
        }

        return true;
    }
}
