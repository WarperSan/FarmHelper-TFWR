using System.Linq;
using System.Reflection;
using BepInEx;
using FarmHelper.API.Attributes;
using FarmHelper.API.Interfaces;
using FarmHelper.API.UI;
using FarmHelper.Extensions;
using FarmHelper.Helpers;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace FarmHelper.UI;

public class PluginListMenu : PluginMenu
{
    /// <inheritdoc />
    public override string DisplayName => "Plugin List";

    /// <inheritdoc />
    protected override GameObject Create(Menu parent)
    {
        var menu = CreateMenu(parent.transform);
        
        var plugins = PluginHelper.GetPlugins()
            .OrderBy(x => x.Info.Metadata.Name).ToArray();
        
        // Add title button
        if (!AddTitleButton(menu, plugins.Length))
        {
            Destroy(menu);
            return null;
        }
        
        // Add pages
        foreach (var plugin in plugins)
            AddPluginPage(plugin, pluginList.content);
        
        return menu;
    }

    #region Menu UI

    private ScrollRect pluginList;

    private GameObject CreateMenu(Transform parent)
    {
        var menu = parent.AddPanel();
        menu.SetActive(true); // For Layout

        // Name
        menu.name = "PluginListMenu";
        
        // Back button
        var backBtn = menu.AddButton();
        backBtn.GetIcon().gameObject.SetActive(false);
        backBtn.Text = "Back";
        backBtn.nameText.fontSize *= 0.5f;
        backBtn.SetListener(Close);

        var backRect = backBtn.GetComponent<RectTransform>();
        backRect.anchorMin = new Vector2(0.01f, 1);
        backRect.anchorMax = new Vector2(1, 0.99f);
        
        // Scroll view
        pluginList = menu.AddScrollView();
        
        var group = pluginList.content.GetComponent<VerticalLayoutGroup>();
        group.spacing = 4;
        group.childControlHeight = false;
        group.childScaleHeight = true;
        
        var listRect = pluginList.GetComponent<RectTransform>();
        listRect.anchorMin = new Vector2(0.02f, 0.02f);
        listRect.anchorMax = new Vector2(0.98f, 0.90f);
        pluginList.GetComponent<Image>().color = new Color(45f / 255, 45f / 255, 45f / 255, 93f / 255);
        
        return menu;
    }
    
    private bool AddTitleButton(GameObject menu, int count)
    {
        // Add button
        if (!UiHelper.AddTitleButton(1, 3, out var pluginsBtn))
        {
            Log.Warning<FarmHelperPlugin>($"Could not create the title button for {nameof(PluginListMenu)}.");
            return false;
        }
        
        pluginsBtn.name = "PluginsBtn";
        pluginsBtn.Text = $"Plugins ({count})";
        pluginsBtn.SetListener(Open);
        
        return true;
    }

    private void AddPluginPage(BaseUnityPlugin plugin, Transform parent)
    {
        var pluginUI = parent.AddPanel();
        pluginUI.name = $"PluginPage - {plugin.Info.Metadata.GUID}";
        
        // Create specific page
        var farmInfo = plugin.GetType().GetCustomAttribute<FarmInfoAttribute>();

        // Use callback or default
        // ReSharper disable once SuspiciousTypeConversion.Global
        if (plugin is IPageCallback page)
            page.CreatePage(farmInfo, pluginUI);
        else
            plugin.CreateDefault(farmInfo, pluginUI);
        
        // Update all layout
        LayoutRebuilder.ForceRebuildLayoutImmediate(pluginUI.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(pluginUI.GetComponent<RectTransform>());
    }

    #endregion
}