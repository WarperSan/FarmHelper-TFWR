using BepInEx;
using BepInEx.Logging;

namespace ModHelper.Helpers;

/// <summary>
/// Class helping for logging stuff
/// </summary>
public static class Log
{
    private static void LogSelf<T>(object data, LogLevel level) where T : BaseUnityPlugin
    {
        // Fetch plugin
        var plugin = PluginHelper.GetPlugin<T>();

        if (plugin == null)
        {
            BepInEx.Logging.Logger.CreateLogSource(typeof(T).Name).LogWarning($"No plugin found for the type '{typeof(T)}'.");
            return;
        }
        
        // Fetch logger
        var logger = plugin.GetProperty<ManualLogSource>("Logger");

        if (logger == null)
        {
            BepInEx.Logging.Logger.CreateLogSource(typeof(T).Name).LogWarning($"No logger found for the plugin of the type '{typeof(T)}'.");
            return;
        }
        
        // Log
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