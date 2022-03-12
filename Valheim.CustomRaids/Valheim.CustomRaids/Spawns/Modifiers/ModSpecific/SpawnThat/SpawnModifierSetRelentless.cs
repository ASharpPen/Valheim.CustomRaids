using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Cache;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.ST
{
    public class SpawnModifierSetRelentless : ISpawnModifier
    {
        private static SpawnModifierSetRelentless _instance;

        public static SpawnModifierSetRelentless Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetRelentless();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (context is null || context.Config is null || !context.Spawn || context.Spawn is null)
            {
                return;
            }

            if(context.Config.TryGet(SpawnConfigSpawnThat.ModName, out Config modConfig) && modConfig is SpawnConfigSpawnThat config)
            {
                if (config.SetRelentless.Value)
                {
                    Log.LogTrace("Setting relentless");

                    var zdo = ZdoCache.GetZdo(context.Spawn);

                    if (zdo is null)
                    {
                        Log.LogDebug("Unable to find zdo.");
                        return;
                    }

                    zdo.Set(SpawnThat.Options.Modifiers.ModifierSetRelentless.ZdoFeature, true);
                }
            }
        }
    }
}
