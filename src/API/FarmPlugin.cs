using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using ModHelper.Extensions;
using UnityEngine;
using UnityEngine.UI;

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

    #region Singleton

    /// <summary>
    /// The list of all the plugins registered, with the fullname of the type.
    /// </summary>
    internal static readonly Dictionary<string, FarmPlugin> Plugins = new();
    
    /// <summary>
    /// Fetches the instance of the plugin of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the plugin</typeparam>
    /// <returns>Instance of the plugin or null if not found</returns>
    protected static T GetPlugin<T>() where T : FarmPlugin 
        => Plugins.GetValueOrDefault(typeof(T).FullName) as T;

    /// <summary>
    /// Registers this plugin
    /// </summary>
    private void RegisterPlugin()
    {
        var key = GetType().FullName ?? "";
        if (Plugins.ContainsKey(key))
            LogWarning($"The name '{key}' is already used by another plugin. This plugin will override the previous one.");
        
        Plugins[key] = this;
    }

    #endregion

    #region Log

    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.Log"/>
    public void Log(object data, LogLevel level) => Logger.Log(level, data ?? "null");
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    public void LogInfo(object data) => Log(data, LogLevel.Info);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogWarning"/>
    public void LogWarning(object data) => Log(data, LogLevel.Warning);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogError"/>
    public void LogError(object data) => Log(data, LogLevel.Error);
    
    #endregion

    #region Mod Page

    public virtual void GetModPage(GameObject page)
    {
        var group = page.AddComponent<HorizontalLayoutGroup>();
        group.childControlHeight = false;
        group.childControlWidth = false;
        group.childForceExpandWidth = false;
        group.childAlignment = TextAnchor.MiddleLeft;
        group.spacing = 52 / 2;
        group.padding.left = 9;
        group.CalculateLayoutInputHorizontal();
        
        // Mod Name
        var modName = page.transform.Find("PlayButton");
        modName.name = "ModName";
        Destroy(modName.Find("InputField (TMP)").gameObject);

        if (modName.TryGetComponent(out ColoredButton btn))
        {
            btn.State = ColoredButton.ButtonState.disabled;
            btn.Text = Name;
            btn.tooltipDescription = $"Made by: {Author ?? "???"}\nVersion: {Version}";
        }
        
        var toggle = page.transform.Find("EditButton");
        toggle.name = "ToggleVisibility";
        
        var toggleImage = toggle.Find("Image").GetComponent<Image>();
        toggleImage.LoadSprite<ModHelperPlugin>("Resources.icon-enable.png", 50);

        if (toggle.TryGetComponent(out ColoredButton toggleBtn))
        {
            toggleBtn.SetListener(() =>
            {
                toggleImage.LoadSprite<ModHelperPlugin>(
                    toggleImage.sprite.texture.name.Contains("disable")
                        ? "Resources.icon-enable.png"
                        : "Resources.icon-disable.png", 50);
            });
        }
        
        Destroy(page.transform.Find("DeleteButton").gameObject);
    }

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

/// <summary>
/// The type of plugin managed by ModHelper.
/// This allows ModHelper to get the instance of the plugin.
/// </summary>
/// <typeparam name="T">Type of this plugin</typeparam>
public abstract class FarmPlugin<T> : FarmPlugin where T : FarmPlugin
{
    public static T Instance => GetPlugin<T>();
    
    #region Log

    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    public new static void LogInfo(object data) => Instance?.LogInfo(data);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogInfo"/>
    public new static void LogWarning(object data) => Instance?.LogWarning(data);
    
    /// <inheritdoc cref="BepInEx.Logging.ManualLogSource.LogError"/>
    public new static void LogError(object data) => Instance?.LogError(data);

    #endregion
}