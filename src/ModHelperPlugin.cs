using BepInEx;
using HarmonyLib;
using ModHelper.API;
using ModHelper.API.Attributes;
using ModHelper.API.UI;
using ModHelper.Helpers;

// ReSharper disable StringLiteralTypo

namespace ModHelper;

/// <summary>
/// Plugin made to help other plugins 
/// </summary>
[BepInPlugin("org.warpersan.modhelper", "Mod Helper", "1.0.0.0")]
[FarmInfo("WarperSan", null, "https://github.com/WarperSan/ModHelper-TFWR")]
public class ModHelperPlugin : BaseUnityPlugin
{
    private void Awake()
    {
        var harmony = new Harmony(Info.Metadata.GUID);
        harmony.PatchAll();
    }

    private void Start()
    {
        PluginsPage.Create();

        //FuncHelper.AddAll<ModHelperFunctions>();

        // typeof(Localizer).GetStaticField<Dictionary<string, string>>("language")
        //     .Add("code_tooltip_pow", test);
    }
}