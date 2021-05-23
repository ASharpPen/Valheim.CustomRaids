using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Raids.Serverside.Schedulers.Default;
using Valheim.CustomRaids.Raids.Serverside.StartConditions;

namespace Valheim.CustomRaids.Raids.Schedulers
{
    public class DefaultScheduler : IRaidScheduler
    {
        public const string Name = "default";

        private static DefaultScheduler _instance;

        private List<Raid> ManagedRaids { get; } = new List<Raid>();
        private List<RandomEvent> ManagedEvents { get; } = new List<RandomEvent>();

        private Dictionary<Raid, RandomEvent> RaidToEventMap { get; } = new Dictionary<Raid, RandomEvent>();
        private Dictionary<RandomEvent, Raid> EventToRaidMap { get; } = new Dictionary<RandomEvent, Raid>();

        static DefaultScheduler()
        {
            Resetter.StateResetter.Subscribe(() =>
            {
                _instance = null;
            });
        }

        private DefaultScheduler() { }

        public static DefaultScheduler Instance
        {
            get
            {
                return _instance ??= new DefaultScheduler();
            }
        }

        public void Initialize()
        {

        }

        public void ManageRaid(Raid raid)
        {
            ManagedRaids.Add(raid);

            var randomEvent = raid.ConvertRandomEvent();
            ManagedEvents.Add(randomEvent);

            RaidToEventMap.Add(raid, randomEvent);
            EventToRaidMap.Add(randomEvent, raid);
        }

        public void Update(float deltaTime)
        {
            RandEventSystem.instance.UpdateRandomEvent(deltaTime);
        }

        internal static List<RandomEvent> GetRandomEvents()
        {
            return DefaultScheduler.Instance.ManagedEvents;
        }
    }
}
