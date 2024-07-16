using BepInEx;
using FarmHelper.API.Attributes;
using FarmHelper.API.Interfaces;
using FarmHelper.API.UI;
using FarmHelper.Helpers;
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
        LocalizerHelper.Add("error_wrong_number_args_detailed", "{0} takes {1} arguments.\n\nExpected: {2}\nReceived: {3}");
        LocalizerHelper.Add("error_wrong_args_detailed", "{0} expected '{1}' as the #{2} argument.\n\nInstead, it got {3}.");
        
        ModHelperFunctions.LoadAll();
        PluginMenu.Create<PluginListMenu>();
        PluginMenu.Create<TitleMenu>().Open();
    }
}