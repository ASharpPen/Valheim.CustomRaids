using System.Runtime.CompilerServices;
using UnityEngine;

namespace Valheim.CustomRaids.Spawns.Caches
{
    internal static class SpawnCache
    {
        private static ConditionalWeakTable<GameObject, Character> SpawnCharacterTable = new();
        private static ConditionalWeakTable<GameObject, ZDO> SpawnZdoTable = new();

        public static Character GetCharacter(GameObject spawn)
        {
            if (!spawn || spawn is null)
            {
                return null;
            }

            if (SpawnCharacterTable.TryGetValue(spawn, out Character existingCharacter))
            {
                return existingCharacter;
            }

            var character = spawn.GetComponent<Character>();

            if (character is not null)
            {
                SpawnCharacterTable.Add(spawn, character);
            }

            return character;
        }

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
