using BepInEx;
using HarmonyLib;
using ModHelper.API.Attributes;
using ModHelper.API.UI;
using ModHelper.UI;

// ReSharper disable StringLiteralTypo

namespace ModHelper;

/// <summary>
/// Plugin made to help other plugins 
/// </summary>
[BepInPlugin("org.warpersan.modhelper", "Mod Helper", "1.0.0.0")]
[FarmInfo("WarperSan", "https://github.com/WarperSan/ModHelper-TFWR")]
public class ModHelperPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        var harmony = new Harmony(Info.Metadata.GUID);
        harmony.PatchAll();
        API.AssemblyLoader.LoadEmbeddedDLL();
    }

    private void Start()
    {
        PluginMenu.Create<PluginListMenu>();
        PluginMenu.Create<TitleMenu>().Open();
    }
}