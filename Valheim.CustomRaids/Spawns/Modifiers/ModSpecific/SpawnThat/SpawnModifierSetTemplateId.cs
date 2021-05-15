using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Configuration;
using Valheim.CustomRaids.Spawns.Caches;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.SpawnThat
{
    public class SpawnModifierSetTemplateId : ISpawnModifier
    {
        private static SpawnModifierSetTemplateId _instance;

        public static SpawnModifierSetTemplateId Instance
        {
            get
            {
                return _instance ??= new SpawnModifierSetTemplateId();
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
                if (string.IsNullOrWhiteSpace(config.TemplateId.Value))
                {
                    return;
                }

                var zdo = SpawnCache.GetZDO(context.Spawn);

                if (zdo is null)
                {
                    return;
                }

                Log.LogTrace($"Setting template id {config.TemplateId.Value}");
                zdo.Set(Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General.SpawnModifierSetTemplateId.ZdoFeature, config.TemplateId.Value);
            }
        }
    }
}
