using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping to load bundles and assets
/// </summary>
public static class AssetHelper
{
    private static readonly Dictionary<string, AssetBundle> LoadedBundles = new();

    /// <summary>
    /// Loads the given bundle
    /// </summary>
    public static AssetBundle LoadBundle(string name)
    {
        if (LoadedBundles.TryGetValue(name, out var bundle))
            return bundle;

        var assembly = Assembly.GetCallingAssembly();
        var stream = assembly.GetManifestResourceStream(name);
        
        if (stream == null)
            throw new NullReferenceException($"No resource was found for '{name}'.");
        
        return LoadedBundles[name] = AssetBundle.LoadFromStream(stream);
    }

    /// <summary>
    /// Loads the given asset from the given bundle
    /// </summary>
    public static T LoadAsset<T>(string bundleName, string assetName) where T : Object 
        => LoadBundle(bundleName).LoadAsset<T>(assetName);
}