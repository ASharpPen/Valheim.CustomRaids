using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Valheim.CustomRaids.Raids.Serverside
{
    public static class StartConditionManager
    {
        public static List<ValidRaid> GetValidRaids(Raid raid)
        {
            // Ensure all conditions not requiring player info is valid first.
            foreach(var condition in raid.StartConditions)
            {
                if(!condition.IsValid(raid))
                {
                    return new List<ValidRaid>();
                }
            }

            // Verify remaining conditions pr player.
            List<ZDO> allCharacterZDOS = ZNet.instance.GetAllCharacterZDOS();

            List<ValidRaid> validRaids = new List<ValidRaid>();

            foreach (var player in allCharacterZDOS)
            {
                if(raid.StartPlayerConditions.All(x => x.IsValid(raid, player)))
                {
                    validRaids.Add(new ValidRaid
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

    public class ValidRaid
    {
        public Raid Raid { get; set; }

        public ZDO PlayerZdo { get; set; }

        public Vector3 RaidCenter { get; set; }
    }
}
