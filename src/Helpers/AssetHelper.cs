using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FarmHelper.Helpers;

internal static class AssetHelper
{
    private static readonly Dictionary<string, AssetBundle> cachedBundles = new();

    internal static AssetBundle LoadBundle<T>(string name)
    {
        if (cachedBundles.TryGetValue(name, out AssetBundle bundle))
            return bundle;
        
        var stream = typeof(T).Assembly.GetManifestResourceStream(name);

        if (stream == null)
            throw new NullReferenceException($"No bundle named '{name}'.");
        
        return cachedBundles[name] = AssetBundle.LoadFromStream(stream);
    }
}

public static class AssetHelper<U>
{
    public static T LoadAsset<T>(string bundleName, string assetName) where T : Object
        => AssetHelper.LoadBundle<U>(bundleName)?.LoadAsset<T>(assetName);
}