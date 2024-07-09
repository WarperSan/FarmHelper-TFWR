using System.Reflection;
using BepInEx;
using ModHelper.API.Attributes;
using ModHelper.Extensions;
using ModHelper.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace ModHelper.API.UI;

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
    public static void Create(BaseUnityPlugin plugin, FarmInfoAttribute farmInfo, GameObject page)
    {
        var group = page.AddComponent<HorizontalLayoutGroup>();
        group.childControlHeight = false;
        group.childControlWidth = false;
        group.childForceExpandWidth = false;
        group.childAlignment = TextAnchor.MiddleLeft;
        group.spacing = 13f;
        group.padding.left = 9;
        
        PluginNameButton(page.transform, plugin);
        VisibilityButton(page.transform, plugin);
        LinkButton(page.transform, farmInfo?.Url);
    }

    private static void PluginNameButton(Transform page, BaseUnityPlugin plugin)
    {
        var modName = page.Find("PlayButton");
        
        // Set name
        modName.name = "ModName";
        
        // Remove text input
        Object.Destroy(modName.Find("InputField (TMP)").gameObject);

        // Set button click
        if (modName.TryGetComponent(out ColoredButton btn))
        {
            btn.State = ColoredButton.ButtonState.disabled;
            btn.Text = plugin.Info.Metadata.Name;

            var author = plugin.GetType().GetCustomAttribute<FarmInfoAttribute>()?.Author ?? "???";
            btn.tooltipDescription = $"Made by: {author}\nVersion: {plugin.Info.Metadata.Version}";
        }
    }
    private static void VisibilityButton(Transform page, BaseUnityPlugin plugin)
    {
        var toggle = page.Find("EditButton");
        Object.Destroy(toggle.gameObject);
        return;
        
        // Set name
        toggle.name = "ToggleVisibility";
        
        // Set image
        var toggleImage = toggle.Find("Image").GetComponent<Image>();
        toggleImage.LoadSprite<ModHelperPlugin>("Resources.icon-enable.png", 50);

        // Set button click
        if (toggle.TryGetComponent(out ColoredButton toggleBtn))
        {
            toggleBtn.SetListener(() =>
            {
                // toggleImage.LoadSprite<ModHelperPlugin>(
                //     toggleImage.sprite.texture.name.Contains("disable")
                //         ? Resource.GetImage("icon-enable.png")
                //         : Resource.GetImage("icon-disable.png"), 50);
            });
        }
    }
    private static void LinkButton(Transform page, string url)
    {
        var link = page.Find("DeleteButton");

        if (url == null)
        {
            Object.Destroy(link.gameObject);
            return;
        }

        // Set name
        link.name = "Link";
    
        // Set image
        link.Find("Image").GetComponent<Image>()
            .LoadSprite<ModHelperPlugin>(Resource.GetImage("icon-link.png"), 64);

        // Set button click
        if (link.TryGetComponent(out ColoredButton linkBtn))
        {
            linkBtn.SetListener(() =>
            {
                Application.OpenURL(url);
            });
        }
    }
}