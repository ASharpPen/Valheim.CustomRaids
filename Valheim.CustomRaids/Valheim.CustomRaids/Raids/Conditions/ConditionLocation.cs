using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.Raids.Conditions;

public class ConditionLocation : IRaidCondition
{
    private HashSet<string> _locations;

    public HashSet<string> Locations { 
        get => _locations; 
        set
        {
            _locations = value
                .Select(x => x.Trim().ToUpperInvariant())
                .ToHashSet();
        }
    }

    public ConditionLocation(IEnumerable<string> locations)
    {
        _locations = locations
            .Select(x => x.Trim().ToUpperInvariant())
            .ToHashSet();
    }

    public bool IsValid(RaidContext context)
    {
        if (Locations is null ||
            Locations.Count == 0 ||
            ZoneSystem.instance.IsNull())
        {
            return true;
        }

        var zoneId = ZoneSystem.instance.GetZone(context.Position);

        if (ZoneSystem.instance.m_locationInstances?.TryGetValue(zoneId, out var location) == true &&
            location.m_location is not null)
        {
            return Locations.Contains(location
                .m_location
                .m_prefabName
                .Trim()
                .ToUpperInvariant());
        }

        return false;
    }
}
