using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.CustomRaids.Raids2.Schedulers
{
    public static class StartConditionManager
    {
        public static List<ValidRaid<TRaid>> GetValidRaids<TRaid>(TRaid raid) where TRaid : BaseSchedulerRaid
        {
            // Ensure all conditions not requiring player info is valid first.
            foreach (var condition in raid.StartConditions)
            {
                if (!condition.IsValid(raid))
                {
                    return new List<ValidRaid<TRaid>>();
                }
            }

            // Verify remaining conditions pr player.
            List<ZDO> allCharacterZDOS = ZNet.instance.GetAllCharacterZDOS();

            List<ValidRaid<TRaid>> validRaids = new List<ValidRaid<TRaid>>();

            foreach (var player in allCharacterZDOS)
            {
                if (raid.StartPlayerConditions.All(x => x.IsValid(raid, player)))
                {
                    validRaids.Add(new ValidRaid<TRaid>
                    {
                        Raid = raid,
                        PlayerZdo = player,
                        RaidCenter = player.m_position,
                    });
                }
            }

            return validRaids;
        }
    }

    public class ValidRaid<TRaid> where TRaid : BaseSchedulerRaid
    {
        public TRaid Raid { get; set; }

        public ZDO PlayerZdo { get; set; }

        public Vector3 RaidCenter { get; set; }
    }
}
