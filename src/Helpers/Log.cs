using BepInEx;
using BepInEx.Logging;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping for logging stuff
/// </summary>
public static class Log
{
    private static void LogSelf<T>(object data, LogLevel level) where T : BaseUnityPlugin
    {
        // Log
        var plugin = (BaseUnityPlugin) PluginHelper.GetPlugin<T>();
        var logger = plugin?.Logger ?? BepInEx.Logging.Logger.CreateLogSource(typeof(T).Name);
        
        logger.Log(level, data ?? "null");
    }

    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    public static void Info<T>(object data) where T : BaseUnityPlugin => LogSelf<T>(data, LogLevel.Info);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogDebug"/>
    public static void Debug<T>(object data) where T : BaseUnityPlugin => LogSelf<T>(data, LogLevel.Debug);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogWarning"/>
    public static void Warning<T>(object data) where T : BaseUnityPlugin => LogSelf<T>(data, LogLevel.Warning);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogError"/>
    public static void Error<T>(object data) where T : BaseUnityPlugin => LogSelf<T>(data, LogLevel.Error);
}