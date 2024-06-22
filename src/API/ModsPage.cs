using System;
using System.Linq;
using System.Reflection;
using ModHelper.Extensions;
using ModHelper.Helpers;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ModHelper.API;

public static class ModsPage
{
    private static GameObject modsPage;
    
    public static void Create()
    {
        // Destroy if existed
        if (modsPage != null)
        {
            FarmPlugin.Warning<ModHelperPlugin>("Mods page already existed.");
            Object.Destroy(modsPage);
        }
        
        modsPage = GameObject.Find(Constants.SAVE_CHOOSER_PATH).Clone();
        modsPage.name = "ModsPage";
        modsPage.RemoveComponent<SaveChooser>();

        // Destroy extra UI
        Object.Destroy(modsPage.transform.Find("Scroll View/NewGameButton").gameObject);
        Object.Destroy(modsPage.transform.Find("Scroll View/OpenFolderButton").gameObject);

        var plugins = FarmPlugin.GetPlugins();
        
        // Add navigation
        if (!AddNavigation(plugins.Count))
            return;

        var prefab = GetPrefab();

        if (prefab == null)
        {
            FarmPlugin.Error<ModHelperPlugin>("Prefab for the a mod page was not found.");
            return;
        }
        
        var content = modsPage.transform.Find("Scroll View/Viewport/Content");
        
        foreach (var (key, plugin) in plugins.OrderBy(x => x.Value.Name))
        {
            var pluginUI = Object.Instantiate(prefab, content, false);
            pluginUI.name = $"PluginUI - {key}";
            pluginUI.RemoveComponent<SaveOption>();

            plugin.GetModPage(pluginUI);
        
            // If invalid, skip
            if (pluginUI == null)
                FarmPlugin.Warning<ModHelperPlugin>($"Invalid UI for '{key}' for the mod page.");
        }
    }

    #region Navigation

    private static bool AddNavigation(int count)
    {
        // Add button
        if (!UiHelper.AddTitleButton(1, 3, out var modsBtn))
        {
            FarmPlugin.Warning<ModHelperPlugin>("Could not create the title button for ModsPage.");
            Object.Destroy(modsPage);
            return false;
        }

        modsBtn.name = "ModsBtn";
        modsBtn.Text = $"Mods ({count})";
        modsBtn.SetListener(() => SetActive(true));
        
        // Close when back clicked
        modsPage.transform.Find("Scroll View/BackButton").GetComponent<ColoredButton>()
            .SetListener(() => SetActive(false));
        
        return true;
    }

    private static void SetActive(bool isActive)
    {
        // ColoredButton btn = modsPage.transform.Find("Scroll View/BackButton")
        //     .GetComponent<ColoredButton>();
        // btn.pressed = false;
        // btn.hovered = false;
        // btn.CallMethod("UpdateColor");
        
        modsPage.SetActive(isActive);
        UiHelper.SetActiveTitle(!isActive);
    }
    
    #endregion

    private static GameObject GetPrefab()
    {
        var saveChooser = GameObject.Find(Constants.SAVE_CHOOSER_PATH).GetComponent<SaveChooser>();
        
        return saveChooser?.GetField<SaveOption>("saveOptionPrefab")?.gameObject;
    }

    #region Default

    public static void DefaultPage(FarmPlugin plugin, GameObject page)
    {
        var group = page.AddComponent<HorizontalLayoutGroup>();
        group.childControlHeight = false;
        group.childControlWidth = false;
        group.childForceExpandWidth = false;
        group.childAlignment = TextAnchor.MiddleLeft;
        group.spacing = 13f;
        group.padding.left = 9;
        
        // Mod Name
        var modName = page.transform.Find("PlayButton");
        modName.name = "ModName";
        Object.Destroy(modName.Find("InputField (TMP)").gameObject);

        if (modName.TryGetComponent(out ColoredButton btn))
        {
            btn.State = ColoredButton.ButtonState.disabled;
            btn.Text = plugin.Name;
            btn.tooltipDescription = $"Made by: {plugin.Author ?? "???"}\nVersion: {plugin.Version}";
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
        
        Object.Destroy(toggle.gameObject);
        Object.Destroy(page.transform.Find("DeleteButton").gameObject);
    }
    
    #endregion
}