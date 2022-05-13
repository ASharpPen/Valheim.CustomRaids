using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valheim.CustomRaids.World.Queries;

namespace Valheim.CustomRaids.Raids.Conditions;

public class ConditionMustNotBeNearPrefab : IRaidCondition
{
    public HashSet<int> PrefabHashes = new();

    public HashSet<string> Prefabs { get; }

    public int Distance { get; set; } = 100;

    public ConditionMustNotBeNearPrefab(int distance, IEnumerable<string> prefabNames)
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
        return !query.HasAny(PrefabHashes);
    }

    private class PrefabQuery : BaseZdoQuery
    {
        public PrefabQuery(Vector3 center, int range) : base(center, range)
        {
        }

        public bool HasAny(HashSet<int> prefabHashes)
        {
            Initialize();

            return Zdos.Any(x => prefabHashes.Contains(x.m_prefab) && IsWithinRange(x));
        }
    }
}
