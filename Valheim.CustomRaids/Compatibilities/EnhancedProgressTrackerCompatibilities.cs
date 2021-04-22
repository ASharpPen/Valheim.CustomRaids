using Valheim.EnhancedProgressTracker.GlobalKey.Shared;

namespace Valheim.CustomRaids.Compatibilities
{
	/// <summary>
	/// Class containing the actual references to the Valheim.EnhancedProgressTracker. 
	/// This should avoid the rest of the code being hard-dependant upon the mod dll.
	/// If any of the methods in here is called, it should check the assembly reference, and if not present, will probably crash.
	/// Use the CustomRaidPlugin.EnhancedProgressTrackerInstalled to check if these calls can be made.
	/// </summary>
	internal static class EnhancedProgressTrackerCompatibilities
    {
        public static string GetPlayerName(ZDO characterZDO)
        {
			return characterZDO.GetString("playerName");
        }

		public static bool HaveGlobalKey(string playerName, string key)
        {
			return ZoneSystem.instance.HasGlobalKey(playerName, key);
        }
    }
}
