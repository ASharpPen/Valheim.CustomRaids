using System.Runtime.CompilerServices;
using UnityEngine;

namespace Valheim.CustomRaids.Caches
{
    public static class ZdoCache
    {
        private static ConditionalWeakTable<GameObject, ZDO> ZdoTable = new();

        public static ZDO GetZDO(GameObject gameObject)
        {
            if (ZdoTable.TryGetValue(gameObject, out ZDO existing))
            {
                return existing;
            }

            var znetView = gameObject.GetComponent<ZNetView>();
            if (!znetView || znetView is null)
            {
                return null;
            }

            var zdo = znetView.GetZDO();
            ZdoTable.Add(gameObject, zdo);
            return zdo;
        }
    }
}
