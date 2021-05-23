using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Raids.Schedulers;
using Valheim.CustomRaids.Raids.Schedulers.Individual;

namespace Valheim.CustomRaids.Raids
{
    public static class SchedulerManager
    {
        private static Dictionary<string, IRaidScheduler> RegisteredSchedulers = new Dictionary<string, IRaidScheduler>();

        static SchedulerManager()
        {
            Resetter.StateResetter.Subscribe(() =>
            {
                RegisteredSchedulers = new Dictionary<string, IRaidScheduler>();

                SchedulerManager.RegisterScheduler(DefaultScheduler.Name, DefaultScheduler.Instance);
                SchedulerManager.RegisterScheduler(IndividualFrequencyScheduler.Name, new IndividualFrequencyScheduler());
            });
        }

        public static void RegisterScheduler(string schedulerName, IRaidScheduler scheduler)
        {
            if(RegisteredSchedulers.ContainsKey(schedulerName))
            {
                Log.LogWarning($"Overriding existing scheduler {schedulerName} with scheduler of type {scheduler.GetType().Name}");
            }

            RegisteredSchedulers[schedulerName] = scheduler;
        }

        public static void Save()
        {
            foreach(var scheduler in RegisteredSchedulers.Values)
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
    }
}
