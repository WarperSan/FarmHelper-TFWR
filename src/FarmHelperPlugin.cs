using BepInEx;
using FarmHelper.API.Attributes;
using FarmHelper.API.Interfaces;
using FarmHelper.API.UI;
using FarmHelper.UI;
using HarmonyLib;

// ReSharper disable StringLiteralTypo

namespace FarmHelper;

/// <summary>
/// Plugin made to help other plugins 
/// </summary>
[BepInPlugin("org.warpersan.farmhelper", "Farm Helper", "1.0.0.0")]
[FarmInfo("WarperSan", "https://github.com/WarperSan/FarmHelper-TFWR")]
public class FarmHelperPlugin : BaseUnityPlugin, IToggleable
{
    private void Awake()
    {
        var harmony = new Harmony("org.warpersan.farmhelper");
        harmony.PatchAll();
    }

    public void OnAdd()
    {
        ModHelperFunctions.LoadAll();
        PluginMenu.Create<PluginListMenu>();
        PluginMenu.Create<TitleMenu>().Open();
    }
}