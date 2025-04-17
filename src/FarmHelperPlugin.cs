using BepInEx;
using FarmHelper.API.Attributes;
using FarmHelper.Extensions;
using FarmHelper.Helpers;
using FarmHelper.Patches;
using HarmonyLib;

namespace FarmHelper;

/// <summary>
/// Plugin made to help other plugins 
/// </summary>
[BepInPlugin(Constants.GUID, "Farm Helper", "1.0.0.0")]
[FarmInfo("WarperSan", "https://github.com/WarperSan/FarmHelper-TFWR")]
public class FarmHelperPlugin : BaseUnityPlugin
{
    /// <summary>
    /// Instance of this plugin
    /// </summary>
    public static FarmHelperPlugin Instance { get; private set; } = null!;

    private void Awake()
    {
        Instance = this;
        
        Patch();
        
        LocalizerHelper.Add(ErrorHelper.WRONG_ARGUMENT_COUNT_ERROR, "{0} takes {1} arguments.\n\nExpected:\n{2}\n\nReceived:\n{3}");
        LocalizerHelper.Add(ErrorHelper.WRONG_ARGUMENTS_ERROR, "{0} expected '{1}' as the #{2} argument.\n\nInstead, it got {3}.");
        
        ModHelperFunctions.LoadAll();
    }

    #region Harmony

    private Harmony? _harmony;

    private void Patch()
    {
        _harmony = new Harmony(Constants.GUID);
        
        _harmony.PatchAll();
        _harmony.TranspileLambda(
            typeof(CodeUtilities),
            nameof(CodeUtilities.SyntaxColor2),
            typeof(CodeUtilitiesSyntaxColor2),
            nameof(CodeUtilitiesSyntaxColor2.ParseGroupToColor)
        );
    }

    private void Unpatch()
    {
        if (_harmony == null)
            return;
        
        _harmony.UnpatchSelf();
        _harmony = null;
    }

    #endregion
}