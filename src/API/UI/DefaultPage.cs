using System.Reflection;
using BepInEx;
using FarmHelper.API.Attributes;
using FarmHelper.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace FarmHelper.API.UI;

/// <summary>
/// Default page for a plugin
/// </summary>
public static class DefaultPage
{
    /// <summary>
    /// Default page for a plugin
    /// </summary>
    /// <param name="plugin"></param>
    /// <param name="farmInfo"></param>
    /// <param name="page"></param>
    public static void CreateDefault(this BaseUnityPlugin plugin, FarmInfoAttribute farmInfo, GameObject page)
    {
        var group = page.AddComponent<HorizontalLayoutGroup>();
        group.childControlHeight = false;
        group.childControlWidth = false;
        group.childForceExpandWidth = false;
        group.childAlignment = TextAnchor.MiddleLeft;
        group.spacing = 13f;
        group.padding = new RectOffset(8, 8, 4, 4);

        page.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.MinSize;
        
        foreach (Transform child in page.transform)
            Object.Destroy(child.gameObject);
        
        PluginNameButton(page.transform, plugin);
        LinkButton(page.transform, farmInfo?.Url);
    }

    private static void PluginNameButton(Transform page, BaseUnityPlugin plugin)
    {
        var pluginName = page.AddButton();
        
        // Set name
        pluginName.name = "ModName";

        pluginName.State = ColoredButton.ButtonState.disabled;
        pluginName.Text = plugin.Info.Metadata.Name;

        var author = plugin.GetType().GetCustomAttribute<FarmInfoAttribute>()?.Author ?? "???";
        pluginName.tooltipDescription = $"Made by: {author}\nVersion: {plugin.Info.Metadata.Version}";
        
        // Image
        pluginName.GetIcon().gameObject.SetActive(false);
    }
    private static void LinkButton(Transform page, string url)
    {
        if (url == null)
            return;
        
        var link = page.AddButton();

        // Set name
        link.name = "Link";
        link.nameText.gameObject.SetActive(false);
    
        // Set image
        link.GetIcon().LoadSprite<FarmHelperPlugin>(Resource.GetImage("icon-link.png"), 64);

        // Set button click
        link.SetListener(() => Application.OpenURL(url));
    }
    
    // --- UNUSED ---
    private static void VisibilityButton(Transform page, BaseUnityPlugin plugin)
    {
        var toggle = page.Find("EditButton");
        
        // Set name
        toggle.name = "ToggleVisibility";
        
        // Set image
        var toggleImage = toggle.Find("Image").GetComponent<Image>();
        toggleImage.LoadSprite<FarmHelperPlugin>("Resources.icon-enable.png", 50);

        // Set button click
        if (toggle.TryGetComponent(out ColoredButton toggleBtn))
        {
            toggleBtn.SetListener(() =>
            {
                // toggleImage.LoadSprite<FarmHelperPlugin>(
                //     toggleImage.sprite.texture.name.Contains("disable")
                //         ? Resource.GetImage("icon-enable.png")
                //         : Resource.GetImage("icon-disable.png"), 50);
            });
        }
    }
}