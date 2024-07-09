using System.Reflection;
using BepInEx;
using ModHelper.API.Attributes;
using ModHelper.API.UI;
using UnityEngine;

namespace ModHelper.Extensions;

/// <summary>
/// Extensions methods for <see cref="BaseUnityPlugin"/>
/// </summary>
public static class PluginExtensions
{
    internal static void CreatePage(this BaseUnityPlugin plugin, GameObject pluginUI)
    {
        // Create specific page
        var info = plugin.GetType().GetCustomAttribute<FarmInfoAttribute>();

        // Use callback
        // if (info is { PluginPageCallback: not null })
        // {
        //     plugin.CallMethod(info.PluginPageCallback, pluginUI);
        //     return;
        // }
        
        DefaultPage.Create(plugin, info, pluginUI);
    }
}