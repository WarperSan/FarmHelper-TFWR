using System.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace ModHelper;

/// <summary>
/// Contains useful constants to ease the code.
/// </summary>
public static class Constants
{
    public const float WINDOW_ICON_OFFSET_X = 50f;
    public const float MAIN_TITLE_OFFSET_X = 15f;
    public const float MAIN_TITLE_OFFSET_Y = 62.68f;
    
    // --- PATHS ---
    public const string TITLE_PATH = "OverlayUI/MenuCanvas/Menu/TitlePage";
    public const string SAVE_CHOOSER_PATH = "OverlayUI/MenuCanvas/Menu/SaveChooser";
    // ---
    
    // --- FLAGS ---
    public const BindingFlags INSTANCE_FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    public const BindingFlags STATIC_FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
    
    public const BindingFlags ALL_FLAGS = INSTANCE_FLAGS | STATIC_FLAGS;
    // ---
}