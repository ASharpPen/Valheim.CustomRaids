﻿using System.Runtime.CompilerServices;
using UnityEngine;

namespace Valheim.CustomRaids.Spawns.Caches
{
    internal static class ZdoCache
    {
        private static ConditionalWeakTable<GameObject, ZDO> SpawnZdoTable = new();

        public static ZDO GetZDO(GameObject gameObject)
        {
            if (SpawnZdoTable.TryGetValue(gameObject, out ZDO existing))
            {
                return existing;
            }

            var znetView = gameObject.GetComponent<ZNetView>();
            if (!znetView || znetView is null)
            {
                return null;
            }

            var zdo = znetView.GetZDO();
            SpawnZdoTable.Add(gameObject, zdo);
            return zdo;
        }
    }
}
