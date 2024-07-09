using System.Linq;
using BepInEx;
using ModHelper.API.UI;
using ModHelper.Extensions;
using ModHelper.Helpers;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace ModHelper;

public class PluginListMenu : PluginMenu
{
    /// <inheritdoc />
    protected override GameObject Create()
    {
        var menu = GameObject.Find(Constants.SAVE_CHOOSER_PATH).Clone();
        SetUpPage(menu);
        
        var plugins = PluginHelper.GetPlugins()
            .OrderBy(x => x.Info.Metadata.Name).ToArray();
        
        // Add navigation
        if (!AddNavigation(menu, plugins.Length))
            return null;
        
        // Add pages
        var container = menu.transform.Find("Scroll View/Viewport/Content");
        foreach (var plugin in plugins)
            AddPluginPage(plugin, container);

        return menu;
    }

    private GameObject GetPrefab()
    {
        var saveChooser = GameObject.Find(Constants.SAVE_CHOOSER_PATH).GetComponent<SaveChooser>();

        return saveChooser.saveOptionPrefab.gameObject;
    }

    private static void SetUpPage(GameObject page)
    {
        page.name = "ModsPage";
        page.RemoveComponent<SaveChooser>();

        // Destroy extra UI
        Object.Destroy(page.transform.Find("Scroll View/NewGameButton").gameObject);
        Object.Destroy(page.transform.Find("Scroll View/OpenFolderButton").gameObject);
        
        // Cover 90% of the screen
        var rect = page.GetComponent<RectTransform>();
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.anchorMin = Vector2.one * 0.05f;
        rect.anchorMax = Vector2.one * 0.95f;
        
        // Set spacing
        page.transform.Find("Scroll View/Viewport/Content").GetComponent<VerticalLayoutGroup>().spacing = 4;
    }

    private bool AddNavigation(GameObject menu, int count)
    {
        // Add button
        if (!UiHelper.AddTitleButton(1, 3, out var pluginsBtn))
        {
            Log.Warning<ModHelperPlugin>($"Could not create the title button for {nameof(PluginListMenu)}.");
            Object.Destroy(menu);
            return false;
        }

        pluginsBtn.name = "PluginsBtn";
        pluginsBtn.Text = $"Plugins ({count})";
        pluginsBtn.SetListener(Open);
        
        // Close when back clicked
        menu.transform.Find("Scroll View/BackButton").GetComponent<ColoredButton>().SetListener(Close);
        
        return true;
    }
    private void SetActive(bool isActive)
    {
        // ColoredButton btn = modsPage.transform.Find("Scroll View/BackButton")
        //     .GetComponent<ColoredButton>();
        // btn.pressed = false;
        // btn.hovered = false;
        // btn.CallMethod("UpdateColor");
        
        //pluginsPage.SetActive(isActive);
        UiHelper.SetActiveTitle(!isActive);
    }

    private void AddPluginPage(BaseUnityPlugin plugin, Transform parent)
    {
        // Check for prefab
        var prefab = GetPrefab();

        if (prefab == null)
        {
            Log.Error<ModHelperPlugin>("Prefab for a plugin page was not found.");
            return;
        }
        
        // Create and clean
        var pluginUI = Object.Instantiate(prefab, parent, false);
        pluginUI.name = $"PluginUI - {plugin.Info.Metadata.GUID}";
        pluginUI.RemoveComponent<SaveOption>();

        plugin.CreatePage(pluginUI);
    }
}