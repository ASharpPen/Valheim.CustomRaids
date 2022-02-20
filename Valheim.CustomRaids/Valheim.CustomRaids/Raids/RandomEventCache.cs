using System;
using System.Runtime.CompilerServices;
using Valheim.CustomRaids.Configuration.ConfigTypes;

namespace Valheim.CustomRaids.Raids
{
    public static class RandomEventCache
    {
        public static ConditionalWeakTable<RandomEvent, RandomEventData> EventTable = new ConditionalWeakTable<RandomEvent, RandomEventData>();

        public static void Initialize(RandomEvent randomEvent, RaidEventConfiguration config)
        {
            var data = EventTable.GetOrCreateValue(randomEvent);

            data.Config = config;
        }

        public static RandomEventData Get(RandomEvent randomEvent)
        {
            return EventTable.GetOrCreateValue(randomEvent);
        }

        public static RaidEventConfiguration GetConfig(RandomEvent randomEvent)
        {
            if (EventTable.TryGetValue(randomEvent, out RandomEventData data))
            {
                return data.Config;
            }

            return null;
        }
    }

    public class RandomEventData
    {
        public RaidEventConfiguration Config;

        public double LastChecked;
    }
}
