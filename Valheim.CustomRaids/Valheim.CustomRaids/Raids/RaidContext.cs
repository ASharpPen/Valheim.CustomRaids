using UnityEngine;

namespace Valheim.CustomRaids.Raids;

public class RaidContext
{
    public RandomEvent RandomEvent { get; set; }

    public Vector3 Position { get; set; }

    public long? PlayerUserId { get; set; }

    public long? PlayerProfileId { get; set; }

    public long? IdentifyPlayerByPos(Vector3 pos)
    {
        foreach (var peer in ZNet.instance.GetPeers())
        {
            if (peer.m_refPos == pos)
            {
                PlayerProfileId = ZDOMan.instance.GetZDO(peer.m_characterID)?.GetLong(ZDOVars.s_playerID);
                PlayerUserId = peer.m_characterID.UserID;

                return PlayerProfileId != 0
                    ? PlayerProfileId
                    : null;
            }
        }

        return null;
    }

}
