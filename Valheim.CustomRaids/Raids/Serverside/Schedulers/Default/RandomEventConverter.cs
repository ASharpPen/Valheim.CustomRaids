using System.Collections.Generic;
using System.Linq;
using Valheim.CustomRaids.Raids.Serverside.StartConditions;

namespace Valheim.CustomRaids.Raids.Serverside.Schedulers.Default
{
    internal static class RandomEventConverter
    {
        public static RandomEvent ConvertRandomEvent(this Raid raid)
        {

            Heightmap.Biome biome = GetBiome(raid);

            var nearBaseOnly = raid.StartConditions.Any(x => x.GetType() == typeof(ConditionNearBase));

            var requiredGlobalKeys = GetGlobalKeys(raid);
            var requiredNotGlobalKeys = GetNotGlobalKeys(raid);

            RandomEvent randomEvent = new RandomEvent()
            {
                m_biome = biome,
                m_duration = raid.DefaultSettings.Duration,
                m_enabled = true,
                m_startMessage = raid.DefaultSettings.AnnouncementStart,
                m_endMessage = raid.DefaultSettings.AnnouncementEnd,
                m_forceEnvironment = raid.DefaultSettings.ForceEnvironment,
                m_forceMusic = raid.DefaultSettings.ForceMusic,
                m_name = raid.Id,
                m_nearBaseOnly = nearBaseOnly,
                m_requiredGlobalKeys = requiredGlobalKeys,
                m_notRequiredGlobalKeys = requiredNotGlobalKeys,
                m_pauseIfNoPlayerInArea = raid.DefaultSettings.PauseIfNoPlayerInArea,
                m_random = true,
            };

            return randomEvent;
        }

        private static Heightmap.Biome GetBiome(Raid raid)
        {
            var biomeConditions = raid.StartConditions
                .Where(x => x.GetType() == typeof(ConditionBiome))
                .Select(x => (x as ConditionBiome).BiomeMask)
                .Distinct()
                .ToList();

            if ((biomeConditions?.Count ?? 0) == 0)
            {
                return (Heightmap.Biome)1023;
            }

            List<Heightmap.Biome> biomes = new List<Heightmap.Biome>();

            var biomeRequest = Heightmap.Biome.None;

            foreach (var biome in biomeConditions)
            {
                biomeRequest = biomeRequest | biome;
            }

            return biomeRequest;
        }

        private static List<string> GetGlobalKeys(Raid raid)
        {
            var globalKeyConditions = raid.StartConditions
                .Where(x => x.GetType() == typeof(ConditionGlobalKeys))
                .SelectMany(x => (x as ConditionGlobalKeys).GlobalKeys)
                .ToHashSet();

            return globalKeyConditions.ToList();
        }

        private static List<string> GetNotGlobalKeys(Raid raid)
        {
            var notGlobalKeyConditions = raid.StartConditions
                .Where(x => x.GetType() == typeof(ConditionNotGlobalKeys))
                .SelectMany(x => (x as ConditionNotGlobalKeys).NotGlobalKeys)
                .ToHashSet();

            return notGlobalKeyConditions.ToList();
        }
    }
}
