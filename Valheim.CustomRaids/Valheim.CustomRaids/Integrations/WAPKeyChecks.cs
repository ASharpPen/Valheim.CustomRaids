using Valheim.CustomRaids.Resetter;
using VentureValheim.Progression;

namespace Valheim.CustomRaids.Integrations;

internal static class WAPKeyChecks
{
    public static bool ShouldUseWAP()
    {
        if (LifecycleManager.State is GameState.Dedicated &&
            ProgressionConfiguration.Instance.GetUsePrivateKeys())
        {
            return true;
        }

        return false;
    }

    public static bool Check(long playerId, string key)
    {
        if (KeyManager.Instance.ServerPrivateKeysList.TryGetValue(playerId, out var playerKeys))
        {
            return playerKeys.Contains(key);
        }

        return false;
    }

}
