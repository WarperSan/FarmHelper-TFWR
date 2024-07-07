using System.Linq;
using BepInEx;
using ModHelper.Extensions;
using ModHelper.Helpers;
using UnityEngine;

namespace ModHelper.API.UI;

/// <summary>
/// Lists all the plugins currently loaded
/// </summary>
public static class PluginsPage
{
    private static GameObject pluginsPage;
    
    /// <summary>
    /// Creates the UI for the plugins page
    /// </summary>
    public static void Create()
    {
        // Destroy if existed
        if (pluginsPage != null)
        {
            Log.Warning<ModHelperPlugin>("Plugins page already existed.");
            Object.Destroy(pluginsPage);
        }
        
        pluginsPage = GameObject.Find(Constants.SAVE_CHOOSER_PATH).Clone();
        pluginsPage.name = "ModsPage";
        pluginsPage.RemoveComponent<SaveChooser>();

        // Destroy extra UI
        Object.Destroy(pluginsPage.transform.Find("Scroll View/NewGameButton").gameObject);
        Object.Destroy(pluginsPage.transform.Find("Scroll View/OpenFolderButton").gameObject);
        
        var plugins = PluginHelper.GetPlugins()
            .OrderBy(x => x.Info.Metadata.Name).ToArray();
        
        // Add navigation
        if (!AddNavigation(plugins.Length))
            return;
        
        // Add plugin pages
        var content = pluginsPage.transform.Find("Scroll View/Viewport/Content");
        
        foreach (var plugin in plugins)
            GetPluginPage(plugin, content);
    }

    #region Navigation

    private static bool AddNavigation(int count)
    {
        // Add button
        if (!UiHelper.AddTitleButton(1, 3, out var modsBtn))
        {
            Log.Warning<ModHelperPlugin>($"Could not create the title button for {nameof(PluginsPage)}.");
            Object.Destroy(pluginsPage);
            return false;
        }

        modsBtn.name = "PluginsBtn";
        modsBtn.Text = $"Plugins ({count})";
        modsBtn.SetListener(() => SetActive(true));
        
        // Close when back clicked
        pluginsPage.transform.Find("Scroll View/BackButton").GetComponent<ColoredButton>()
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
        
        pluginsPage.SetActive(isActive);
        UiHelper.SetActiveTitle(!isActive);
    }
    
    #endregion

    private static GameObject GetPrefab()
    {
        var saveChooser = GameObject.Find(Constants.SAVE_CHOOSER_PATH).GetComponent<SaveChooser>();
        
        return saveChooser?.GetField<SaveOption>("saveOptionPrefab")?.gameObject;
    }

    private static void GetPluginPage(BaseUnityPlugin plugin, Transform parent)
    {
        // Check for prefab
        var prefab = GetPrefab();

        if (prefab == null)
        {
            Log.Error<ModHelperPlugin>("Prefab for a plugin page was not found.");
            return;
        }
        
        // Create and clean
        var key = plugin.Info.Metadata.GUID;
        var pluginUI = Object.Instantiate(prefab, parent, false);
        pluginUI.name = $"PluginUI - {key}";
        pluginUI.RemoveComponent<SaveOption>();

        plugin.CreatePage(pluginUI);
    }
}