using System.Linq;
using ModHelper.Extensions;
using ModHelper.Helpers;
using UnityEngine;

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

        // Add navigation
        if (!AddNavigation())
            return;

        var prefab = GetPrefab();

        if (prefab == null)
        {
            FarmPlugin.Error<ModHelperPlugin>("Prefab for the a mod page was not found.");
            return;
        }
        
        var content = modsPage.transform.Find("Scroll View/Viewport/Content");
        var plugins = FarmPlugin.Plugins.OrderBy(x => x.Value.Name);
        
        foreach (var (key, plugin) in plugins)
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

    private static bool AddNavigation()
    {
        // Add button
        if (!UiHelper.AddTitleButton(1, 3, out var modsBtn))
        {
            FarmPlugin.Warning<ModHelperPlugin>("Could not create the title button for ModsPage.");
            Object.Destroy(modsPage);
            return false;
        }

        modsBtn.name = "ModsBtn";
        modsBtn.Text = $"Mods ({FarmPlugin.Plugins.Count})";
        modsBtn.SetListener(() => SetActive(true));
        
        // Close when back clicked
        modsPage.transform.Find("Scroll View/BackButton").GetComponent<ColoredButton>()
            .SetListener(() => SetActive(false));
        
        return true;
    }

    private static void SetActive(bool isActive)
    {
        modsPage.SetActive(isActive);
        UiHelper.SetActiveTitle(!isActive);
    }
    
    #endregion

    private static GameObject GetPrefab()
    {
        var saveChooser = GameObject.Find(Constants.SAVE_CHOOSER_PATH).GetComponent<SaveChooser>();
        var info = typeof(SaveChooser).GetField(
            "saveOptionPrefab", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
        );

        var option = info?.GetValue(saveChooser) as SaveOption;
        
        return option?.gameObject;
    }
}