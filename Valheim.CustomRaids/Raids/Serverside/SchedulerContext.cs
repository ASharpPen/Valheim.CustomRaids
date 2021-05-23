using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Valheim.CustomRaids.Raids.Serverside
{
    public class SchedulerContext
    {
        public string PlayerName { get; set; }

        public Vector3 RaidCenter { get; set; }

        public Raid Raid { get; set; }
    }
}
