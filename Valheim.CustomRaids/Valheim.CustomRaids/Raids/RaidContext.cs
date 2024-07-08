using UnityEngine;

namespace Valheim.CustomRaids.Raids;

public class RaidContext
{
    public RandomEvent RandomEvent { get; set; }

    public Vector3 Position { get; set; }

    public long? PlayerUserId { get; set; }

    public long? IdentifyPlayerByPos(Vector3 pos)
    {
        foreach (var peer in ZNet.instance.GetPeers())
        {
            if (peer.m_refPos == pos)
            {
                return PlayerUserId = peer.m_characterID.UserID;
            }
        }

        return null;
    }

}
