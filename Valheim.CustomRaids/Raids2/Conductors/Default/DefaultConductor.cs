using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.CustomRaids.Caches;

namespace Valheim.CustomRaids.Raids2.Conductors.Default
{
    public class DefaultConductor : IConductor, IRequireOptions<DefaultConductorOptions>, IRequireRaidOptions<DefaultConductorRaidOptions>
    {
        private double Duration { get; set; } = 90;

        private const string ZdoRaidTime = "RaidThat_RaidTime";

        private SpawnSystem Spawner { get; set; }
        private ZDO Zdo { get; set; }

        private Vector3 Position { get; set; }

        private DefaultConductorRaidOptions Raid { get; set; }

        public void SetOptions(DefaultConductorOptions options)
        {
        }

        public void SetConductorRaid(DefaultConductorRaidOptions raid)
        {
            Raid = raid;
        }

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Start(Raid raid)
        {
        }

        public void Update(float deltaTime)
        {
            var existingTime = Zdo.GetFloat(ZdoRaidTime);
            var currentTime = existingTime + deltaTime;

            if(currentTime > Duration)
            {
                //TODO: Stop Raid
            }

            var position = Spawner.transform.position;

            var validSpawns = Raid.SpawnTemplates.Where(x => x.Conditions.All(x => x.IsValid(position))).ToList();


        }

        private SpawnSystem.SpawnData TemplateToSpawnData(SpawnTemplate template)
        {

        }
    }
}
