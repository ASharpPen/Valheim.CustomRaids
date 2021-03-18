using HarmonyLib;
using System.Linq;

namespace Valheim.CustomRaids.Compatibility
{
    public static class CreatureLevelAndLootControlCompatibility
    {
        public static void MakeCompatible(Harmony harmony)
        {
            RemoveLevelControl(harmony);
        }

        /// <summary>
        /// CreatureLevelAndLootControl takes control over spawn levels. 
        /// This causes the configured levels set in Custom Raids to be ignored.
        /// Removing this patch instead of fighting a war over who gets to set the levels.
        /// </summary>
        private static void RemoveLevelControl(Harmony harmony)
        {
            var spawnSystemSpawnPatch = AccessTools.Method(typeof(SpawnSystem), "Spawn");
            var conflictingPatches = Harmony.GetPatchInfo(spawnSystemSpawnPatch)
                .Transpilers
                .Where(x => x.owner == "org.bepinex.plugins.creaturelevelcontrol");

            foreach (var conflictingPatch in conflictingPatches)
            {
                harmony.Unpatch(spawnSystemSpawnPatch, conflictingPatch.PatchMethod);
            }
        }
    }
}
