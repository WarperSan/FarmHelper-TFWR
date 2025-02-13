using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FarmHelper.Helpers;

internal static class AssetHelper
{
    private static readonly Dictionary<string, AssetBundle> CachedBundles = new();

    internal static AssetBundle LoadBundle<T>(string name)
    {
        if (CachedBundles.TryGetValue(name, out var bundle))
            return bundle;
        
        var stream = typeof(T).Assembly.GetManifestResourceStream(name);

        if (stream == null)
            throw new NullReferenceException($"No bundle named '{name}'.");
        
        return CachedBundles[name] = AssetBundle.LoadFromStream(stream);
    }
}

public static class AssetHelper<TU>
{
    public static T LoadAsset<T>(string bundleName, string assetName) where T : Object
        => AssetHelper.LoadBundle<TU>(bundleName)?.LoadAsset<T>(assetName);
}