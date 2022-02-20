using System.IO;
using BepInEx;
using Valheim.CustomRaids.Configuration;
using Valheim.CustomRaids.Core;
using Valheim.CustomRaids.Utilities.Extensions;

namespace Valheim.CustomRaids.Utilities;

internal static class FileUtils
{
    public static string PrepareWriteDebugFile(string filename, string fileDescription)
    {
        string debugDir = "Debug";

        if (ConfigurationManager.GeneralConfig?.DebugFileFolder is not null)
        {
            debugDir = Path.Combine(ConfigurationManager.GeneralConfig.DebugFileFolder.Value.SplitBySlash());
        }

        string filePath = Path.Combine(Paths.BepInExRootPath, debugDir, filename);

        FileUtils.EnsureDirectoryExistsForFile(filePath);

        Log.LogInfo($"Writing {fileDescription} to file {filePath}.");

        return filePath;
    }

    public static void EnsureDirectoryExistsForFile(string filePath)
    {
        var dir = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(dir))
        {
            Log.LogTrace("Creating missing folders in path.");
            Directory.CreateDirectory(dir);
        }
    }
}
