using ModHelper.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    /// Adds a button prefab to this parent
    /// </summary>
    public static ColoredButton AddButton(this Transform parent)
        => parent.Add("Button").GetComponent<ColoredButton>();

    /// <summary>
    /// Fetches the requested asset and creates a copy
    /// </summary>
    private static GameObject Add(this Transform parent, string name)
    {
        var asset = AssetHelper<ModHelperPlugin>.LoadAsset<GameObject>(API.Resource.GetBundle("uibundle.assets"), name);

        if (asset == null)
            throw new System.NullReferenceException($"No asset named '{name}' was found.");
        
        return Object.Instantiate(asset, parent);
    }
}