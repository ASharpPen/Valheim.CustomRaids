using Valheim.CustomRaids.Configuration.ConfigTypes;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Core.Cache;
using Valheim.CustomRaids.Core.Configuration;

namespace Valheim.CustomRaids.Spawns.Modifiers.ModSpecific.ST
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

                var zdo = ZdoCache.GetZdo(context.Spawn);

                if (zdo is null)
                {
                    return;
                }

                Log.LogTrace($"Setting template id {config.TemplateId.Value}");
                zdo.Set(SpawnThat.Options.Modifiers.ModifierSetTemplateId.ZdoFeature, config.TemplateId.Value);
            }
        }
    }
}
