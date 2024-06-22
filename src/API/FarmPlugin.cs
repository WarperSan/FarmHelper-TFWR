using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global

namespace ModHelper.API;

/// <summary>
/// The type of plugin managed by ModHelper.
/// </summary>
public abstract class FarmPlugin : BaseUnityPlugin
{
    public Harmony Harmony { get; private set; }

    #region Info

    /// <inheritdoc cref="BepInPlugin.GUID"/>
    public string Guid => Info.Metadata.GUID;
    
    /// <inheritdoc cref="BepInPlugin.Name"/>
    public string Name => Info.Metadata.Name;
    
    /// <inheritdoc cref="BepInPlugin.Version"/>
    public Version Version => Info.Metadata.Version;

    /// <summary>
    /// The name of the author of this plugin.
    /// </summary>
    public virtual string Author => null;

    #endregion

    #region Register

    /// <summary>
    /// The list of all the plugins registered, with the fullname of the type.
    /// </summary>
    private static readonly Dictionary<string, FarmPlugin> Plugins = new();
    
    /// <summary>
    /// Copies the list of all the plugins registered and returns it.
    /// </summary>
    /// <returns>The list of all the plugins registered</returns>
    public static Dictionary<string, FarmPlugin> GetPlugins() => new(Plugins);
    
    /// <summary>
    /// Fetches the instance of the plugin of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the plugin</typeparam>
    /// <returns>Instance of the plugin or null if not found</returns>
    public static T GetPlugin<T>() where T : FarmPlugin 
        => Plugins.GetValueOrDefault(typeof(T).FullName) as T;

    /// <summary>
    /// Registers this plugin
    /// </summary>
    private void RegisterPlugin()
    {
        var key = GetType().FullName ?? "";
        if (Plugins.ContainsKey(key))
            Warning($"The name '{key}' is already used by another plugin. This plugin will override the previous one.");
        
        Plugins[key] = this;
    }

    #endregion

    #region Log

    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.Log"/>
    private void Log(object data, LogLevel level) => Logger.Log(level, data ?? "null");

    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    protected void Msg(object data) => Log(data, LogLevel.Info);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    protected void Warning(object data) => Log(data, LogLevel.Warning);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    protected void Error(object data) => Log(data, LogLevel.Error);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    public static void Msg<T>(object data) where T : FarmPlugin => GetPlugin<T>().Msg(data);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogWarning"/>
    public static void Warning<T>(object data) where T : FarmPlugin => GetPlugin<T>().Warning(data);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogError"/>
    public static void Error<T>(object data) where T : FarmPlugin => GetPlugin<T>().Error(data);
    
    #endregion

    #region Mod Page

    public virtual void GetModPage(GameObject page) => ModsPage.DefaultPage(this, page);

    #endregion
    
    #region MonoBehaviour
    
    /// <summary>
    /// Unity calls Awake when an enabled script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        Harmony = new Harmony(Guid);
        RegisterPlugin();
        OnAwake();
    }

    #endregion

    #region Virtual

    /// <inheritdoc cref="Awake"/>
    protected virtual void OnAwake() { }

    #endregion
}