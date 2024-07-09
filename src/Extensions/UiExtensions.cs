using System;
using ModHelper.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ModHelper.Extensions;

/// <summary>
/// Class helping for creating UI
/// </summary>
public static class UiExtensions
{
    /// <summary>
    /// Adds an input field prefab to this parent
    /// </summary>
    public static TMP_InputField AddInputField(this Transform parent) 
        => parent.Add("InputField").GetComponent<TMP_InputField>();
    
    /// <summary>
    /// Adds a panel prefab to this parent
    /// </summary>
    public static GameObject AddPanel(this Transform parent) 
        => parent.Add("Panel");

    /// <summary>
    /// Adds a scrollbar prefab to this parent
    /// </summary>
    public static Scrollbar AddScrollbar(this Transform parent) 
        => parent.Add("Scrollbar").GetComponent<Scrollbar>();
    
    /// <summary>
    /// Adds a scroll view prefab to this parent
    /// </summary>
    public static ScrollRect AddScrollView(this Transform parent) 
        => parent.Add("ScrollView").GetComponent<ScrollRect>();

    /// <summary>
    /// Fetches the requested asset and creates a copy
    /// </summary>
    private static GameObject Add(this Transform parent, string name)
    {
        var asset = AssetHelper<ModHelperPlugin>.LoadAsset<GameObject>(API.Resource.GetBundle("uibundle.assets"), name);

        if (asset == null)
            throw new NullReferenceException($"No asset named '{name}' was found.");
        
        return Object.Instantiate(asset, parent);
    }
}