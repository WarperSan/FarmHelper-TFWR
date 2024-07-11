using BepInEx;
using FarmHelper.API.Attributes;
using FarmHelper.API.UI;
using FarmHelper.Helpers;
using FarmHelper.UI;

// ReSharper disable StringLiteralTypo

namespace FarmHelper;

/// <summary>
/// Plugin made to help other plugins 
/// </summary>
[BepInPlugin("org.warpersan.farmhelper", "Farm Helper", "1.0.0.0")]
[FarmInfo("WarperSan", "https://github.com/WarperSan/FarmHelper-TFWR")]
public class FarmHelperPlugin : BaseUnityPlugin
{
    private void Start()
    {
        FuncHelper.AddAll<ModHelperFunctions>();
        PluginMenu.Create<PluginListMenu>();
        PluginMenu.Create<TitleMenu>().Open();
    }
}