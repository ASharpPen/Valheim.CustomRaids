using UnityEngine;

namespace Valheim.CustomRaids.Core.Cache
{
    internal class ZdoCache
    {
        private static ManagedCache<ZDO> CacheTable { get; } = new();

        public static ZDO GetZdo(GameObject obj)
        {
            if (CacheTable.TryGet(obj, out ZDO cached))
            {
                return cached;
            }

            var znetView = ComponentCache.GetComponent<ZNetView>(obj);

            ZDO zdo = (!znetView || znetView is null)
                ? null
                : znetView.GetZDO();

            CacheTable.Set(obj, zdo);

            return zdo;
        }
    }
}
