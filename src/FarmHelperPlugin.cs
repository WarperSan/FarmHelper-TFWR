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
        Log.SetLogger(Logger);
    }

    public void OnAdd()
    {
        LocalizerHelper.Add(ErrorHelper.WRONG_ARGUMENT_COUNT_ERROR, "{0} takes {1} arguments.\n\nExpected:\n{2}\n\nReceived:\n{3}");
        LocalizerHelper.Add(ErrorHelper.WRONG_ARGUMENTS_ERROR, "{0} expected '{1}' as the #{2} argument.\n\nInstead, it got {3}.");
        
        ModHelperFunctions.LoadAll();
        PluginMenu.Create<PluginListMenu>();
        PluginMenu.Create<TitleMenu>().Open();
    }
}