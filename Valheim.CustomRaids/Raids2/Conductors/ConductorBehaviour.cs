using UnityEngine;
using Valheim.CustomRaids.Caches;

namespace Valheim.CustomRaids.Raids2.Conductors
{
    public class ConductorBehaviour : MonoBehaviour
    {
        private IConductor RaidConductor { get; set; }

        private ZDO Zdo { get; set; }

        private const string RaidStartet = "RaidThat_Startet";
        private const string ConductorId = "RaidThat_ConductorId";
        private const string RaidId = "RaidThat_RaidId";

        public void Awake()
        {
            Zdo = ZdoCache.GetZDO(gameObject);

            if(Zdo.GetBool(RaidStartet, false))
            {
                // Re-awakening raid. Retrieving new conductor instance
                RaidConductor = ConductorManager.GetConductor(Zdo.GetString(RaidId));

                if(RaidConductor is null)
                {
                    //TODO: Destroy this object if no conductor can be found.
                    return;
                }
            }

            //TODO: Add object to somewhere, so we can see active raids?

            base.InvokeRepeating(nameof(UpdateConductor), 1f, 1f);
        }

        public void Start(Raid raid, string conductorId, IConductor conductor)
        {
            Zdo.Set(RaidStartet, true);
            Zdo.Set(ConductorId, conductorId);
            Zdo.Set(RaidId, raid.RaidId);
        }

        private float LastTime = Time.time;

        public void UpdateConductor()
        {
            var time = Time.time;
            RaidConductor.Update(time - LastTime);

            LastTime = time;
        }
    }
}
