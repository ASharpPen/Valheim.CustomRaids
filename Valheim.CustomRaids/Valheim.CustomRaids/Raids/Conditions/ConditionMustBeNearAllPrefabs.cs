using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.World.Queries;

namespace Valheim.CustomRaids.Raids.Conditions;

public class ConditionMustBeNearAllPrefabs : IRaidCondition
{
    public HashSet<int> PrefabHashes = new();

    public HashSet<string> Prefabs { get; }

    public int Distance { get; set; } = 100;

    public ConditionMustBeNearAllPrefabs(int distance, IEnumerable<string> prefabNames)
    {
        Distance = distance;

        Prefabs = prefabNames
            .Select(x => x?.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToHashSet();

        PrefabHashes = Prefabs
            .Select(x => x.GetStableHashCode())
            .ToHashSet();
    }

    public bool IsValid(RaidContext context)
    {
        if ((Prefabs?.Count ?? 0) == 0)
        {
            return true;
        }

        var query = new PrefabQuery(context.Position, Distance);
        return query.HasAll(PrefabHashes);
    }

    private class PrefabQuery : BaseZdoQuery
    {
        public PrefabQuery(Vector3 center, int range) : base(center, range)
        {
        }

        public bool HasAll(HashSet<int> prefabHashes)
        {
            HashSet<int> missing = new(prefabHashes);

            Initialize();

            foreach (var zdo in Zdos)
            {
                if (missing.Count == 0)
                {
                    break;
                }

                if (missing.Contains(zdo.m_prefab) &&
                    IsWithinRange(zdo))
                {
                    missing.Remove(zdo.m_prefab);
                }
            }

            return missing.Count == 0;
        }
    }
}
