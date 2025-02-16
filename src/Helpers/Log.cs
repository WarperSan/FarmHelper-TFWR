using BepInEx.Logging;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping for logging stuff
/// </summary>
internal static class Log
{
    private static ManualLogSource _logger;
    
    /// <summary>
    /// Assigns the logger of this mod to the given logger
    /// </summary>
    public static void SetLogger(ManualLogSource logger) => _logger = logger;
    
    private static void LogSelf(object data, LogLevel level) => _logger?.Log(level, data ?? "null");

    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogDebug"/>
    public static void Debug(object data) => LogSelf(data, LogLevel.Debug);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    public static void Info(object data) => LogSelf(data, LogLevel.Message);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogWarning"/>
    public static void Warning(object data) => LogSelf(data, LogLevel.Warning);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogError"/>
    public static void Error(object data) => LogSelf(data, LogLevel.Error);
}