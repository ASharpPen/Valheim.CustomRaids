using System.Collections.Generic;
using UnityEngine;
using Valheim.CustomRaids.Raids.Schedulers;
using Valheim.CustomRaids.Raids.Serverside.StartConditions;
using Valheim.CustomRaids.Spawns;

namespace Valheim.CustomRaids.Raids
{
    public class Raid
    {
        public string Id { get; set; }

        public string Scheduler { get; set; } = DefaultScheduler.Name;

        public string Conductor { get; set; }

        public List<SpawnTemplate> SpawnTemplates { get; set; }

        public List<IRaidStartCondition> StartConditions { get; set; }

        public List<IRaidStartPlayerCondition> StartPlayerConditions { get; set; }

        public RandomEventSettings DefaultSettings { get; set; }
    }

    public class RandomEventSettings
    {
        /// <summary>
        /// m_duration
        /// </summary>
        public float Duration { get; set; } = 60;

        /// <summary>
        /// m_startMessage
        /// </summary>
        public string AnnouncementStart { get; set; }

        /// <summary>
        /// m_endMessage
        /// </summary>
        public string AnnouncementEnd { get; set; }

        /// <summary>
        /// m_forceEnvironment
        /// </summary>
        public string ForceEnvironment { get; set; }

        /// <summary>
        /// m_forceMusic
        /// </summary>
        public string ForceMusic { get; set; }

        /// <summary>
        /// m_pauseIfNoPlayerInArea
        /// </summary>
        public bool PauseIfNoPlayerInArea { get; set; }

        /// <summary>
        /// m_pos
        /// </summary>
        public Vector3 RaidCenter { get; set; }
    }
}
