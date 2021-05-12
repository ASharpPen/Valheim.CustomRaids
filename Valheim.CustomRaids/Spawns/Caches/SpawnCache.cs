using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.CustomRaids.Spawns.Caches
{
    internal static class SpawnCache
    {
        private static ConditionalWeakTable<GameObject, Character> SpawnCharacterTable = new();

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
    }
}
