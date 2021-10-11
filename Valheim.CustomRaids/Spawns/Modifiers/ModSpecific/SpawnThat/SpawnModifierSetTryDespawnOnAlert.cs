using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core.Cache;
using Valheim.CustomRaids.Core.Configuration;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.SpawnThat
{
    public class SpawnModifierSetTryDespawnOnAlert : ISpawnModifier
    {
        private static SpawnModifierSetTryDespawnOnAlert _instance;

        public static SpawnModifierSetTryDespawnOnAlert Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetTryDespawnOnAlert();
            }
        }

        public void Modify(SpawnContext context)
        {
            if (context is null || context.Config is null || !context.Spawn || context.Spawn is null)
            {
                return;
            }

            if (context.Config.TryGet(SpawnConfigSpawnThat.ModName, out Config modConfig) && modConfig is SpawnConfigSpawnThat config)
            {
                if (!config.SetTryDespawnOnAlert.Value)
                {
                    return;
                }

                var zdo = ZdoCache.GetZdo(context.Spawn);

                if (zdo is null)
                {
                    return;
                }

                zdo.Set(SpawnModifierDespawnOnAlert.ZdoFeature, true);
            }
        }
    }
}
