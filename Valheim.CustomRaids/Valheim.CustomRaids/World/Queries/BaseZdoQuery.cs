﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Valheim.CustomRaids.Utilities.Extensions;
using Valheim.CustomRaids.World.Zone;

namespace Valheim.CustomRaids.World.Queries;

/// <summary>
/// Base class for making ZDO based queries within an area of the world.
/// </summary>
public abstract class BaseZdoQuery
{
    protected Vector3 Center { get; }
    protected int Range { get; }

    protected List<Vector2i> ZoneIds { get; private set; }
    protected List<ZDO> Zdos { get; private set; }

    protected int MinX { get; private set; }
    protected int MinZ { get; private set; }
    protected int MaxX { get; private set; }
    protected int MaxZ { get; private set; }

    private bool initialized;

    /// <summary>
    /// Prepares for querying zdo's within the zones 
    /// indicated by the center and range.
    /// Selects all ZDO's and ZoneId's using the square formed
    /// by the input, and caches them for subsequent queries.
    /// </summary>
    protected BaseZdoQuery(Vector3 center, int range)
    {
        Center = center;
        Range = range;

        Initialize();
    }

    protected virtual void Initialize()
    {
        if (initialized)
        {
            return;
        }

        (MinX, MaxX) = GetRange((int)Center.x, Range);
        (MinZ, MaxZ) = GetRange((int)Center.z, Range);

        // Get zones to check
        ZoneIds = ZoneUtils.GetZonesInSquare(MinX, MinZ, MaxX, MaxZ);

        // Get zdo's
        Zdos = new List<ZDO>();

        foreach (var zone in ZoneIds)
        {
            ZDOMan.instance.FindObjects(zone, Zdos);
        }

        initialized = true;
    }

    protected static (int min, int max) GetRange(int center, int range)
    {
        return (center - range, center + range);
    }

    protected bool IsWithinRangeManhattan(ZDO zdo)
    {
        // Check if within manhattan distance.
        if (zdo.m_position.x < MinX || zdo.m_position.x > MaxX)
        {
            return false;
        }

        if (zdo.m_position.z < MinZ || zdo.m_position.z > MaxZ)
        {
            return false;
        }

        return true;
    }

    protected bool IsWithinRange(ZDO zdo)
    {
        // Check if within manhattan distance.
        if (zdo.m_position.x < MinX || zdo.m_position.x > MaxX)
        {
            return false;
        }

        if (zdo.m_position.z < MinZ || zdo.m_position.z > MaxZ)
        {
            return false;
        }

        // Check if within circle distance
        return zdo.m_position.WithinHorizontalDistance(Center, Range);
    }
}
