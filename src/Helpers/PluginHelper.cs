using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Bootstrap;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping for Plugins
/// </summary>
public static class PluginHelper
{
    /// <summary>
    /// Copies the list of all the plugins and returns it.
    /// </summary>
    /// <returns>The list of all the plugins</returns>
    public static IEnumerable<BaseUnityPlugin> GetPlugins() => Chainloader.PluginInfos
        .Select(pi => pi.Value.Instance)
        .Where(p => p?.Info != null);

    /// <summary>
    /// Fetches the instance of the plugin of the given type.
    /// </summary>
    /// <typeparam name="T">Type of the plugin</typeparam>
    /// <returns>Instance of the plugin or null if not found</returns>
    public static T GetPlugin<T>() where T : BaseUnityPlugin 
        => GetPlugins().FirstOrDefault(p => p is T) as T;
}