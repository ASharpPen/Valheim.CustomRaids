using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Core;

namespace Valheim.CustomRaids.Raids2.Schedulers
{
    public static class SchedulerManager
    {
        private static Dictionary<string, IRaidScheduler> RegisteredSchedulers = new Dictionary<string, IRaidScheduler>();

        static SchedulerManager()
        {
            Resetter.StateResetter.Subscribe(() =>
            {
                RegisteredSchedulers = new Dictionary<string, IRaidScheduler>();
            });
        }

        public static void RegisterScheduler(string schedulerId, IRaidScheduler scheduler)
        {
            if (RegisteredSchedulers.ContainsKey(schedulerId))
            {
                Log.LogWarning($"Overriding existing scheduler {schedulerId} with scheduler of type {scheduler.GetType().Name}");
            }

            RegisteredSchedulers[schedulerId] = scheduler;
        }

        public static void Save()
        {
            foreach (var scheduler in RegisteredSchedulers.Values)
            {
                scheduler.Save();
            }
        }

        public static void Update(float deltaTime)
        {
            foreach (var scheduler in RegisteredSchedulers.Values)
            {
                scheduler.Update(deltaTime);
            }
        }

        public static void StartRaid(string raidId)
        {

        }
    }
}
