using System;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.ST;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific
{
    public static class SpawnModifierLoaderSpawnThat
    {
        public static bool InstalledSpawnThat { get; } = Type.GetType("Valheim.SpawnThat.SpawnThatPlugin, Valheim.SpawnThat") is not null;

        public static SpawnModifierSetRelentless SetRelentless
        {
            get
            {
                if (InstalledSpawnThat) return SpawnModifierSetRelentless.Instance;

#if DEBUG
                Log.LogDebug("Did not detect SpawnThat installation");
#endif
                return null;
            }
        }

        public static SpawnModifierSetTemplateId SetTemplateId
        {
            get
            {
                if (InstalledSpawnThat) return SpawnModifierSetTemplateId.Instance;

#if DEBUG
                Log.LogDebug("Did not detect SpawnThat installation");
#endif
                return null;
            }
        }

        public static SpawnModifierSetTryDespawnOnAlert SetTryDespawnOnAlert
        {
            get
            {
                if (InstalledSpawnThat) return SpawnModifierSetTryDespawnOnAlert.Instance;

#if DEBUG
                Log.LogDebug("Did not detect SpawnThat installation");
#endif
                return null;
            }
        }
    }
}
