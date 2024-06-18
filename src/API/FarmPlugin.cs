using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace ModHelper.API;

public abstract class FarmPlugin : BaseUnityPlugin
{
    public Harmony Harmony { get; private set; }

    #region Info

    public string Guid => Info.Metadata.GUID;
    public string Name => Info.Metadata.Name;
    public Version Version => Info.Metadata.Version;

    #endregion

    #region Singleton

    public static readonly Dictionary<string, FarmPlugin> Plugins = new();

    #endregion

    #region Log

    public ManualLogSource Log => Logger;

    #endregion
    
    private void Awake()
    {
        Harmony = new Harmony(Guid);
        Plugins[GetType().FullName ?? ""] = this;
        OnAwake();
    }

    /// <inheritdoc cref="Awake"/>
    protected virtual void OnAwake() { }
}

public abstract class FarmPlugin<T> : FarmPlugin where T : FarmPlugin
{
    public static T Instance => Plugins.GetValueOrDefault(typeof(T).FullName) as T;
    
    #region Log

    public new static void Log(object data, LogLevel level) => Instance?.Log.Log(level, data ?? "null");
    public static void LogInfo(object data) => Log(data, LogLevel.Info);
    public static void LogDebug(object data) => Log(data, LogLevel.Debug);
    public static void LogWarning(object data) => Log(data, LogLevel.Warning);
    public static void LogError(object data) => Log(data, LogLevel.Error);

    #endregion
}