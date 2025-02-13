using System.Reflection;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace FarmHelper.API;

/// <summary>
/// Contains useful constants to ease the code.
/// </summary>
public static class Constants
{
    public const float MainTitleOffsetX = 15f;
    public const float MainTitleOffsetY = 62.68f;
    
    // --- PATHS ---
    public const string MenuPath = "OverlayUI/MenuCanvas/Menu";
    public const string TitlePath = MenuPath + "/TitlePage";
    // ---
    
    // --- FLAGS ---
    public const BindingFlags InstanceFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    public const BindingFlags StaticFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
    
    public const BindingFlags AllFlags = InstanceFlags | StaticFlags;
    // ---
    
    // --- LANG ---
    public const string Any = "ANY";
    public const string English = "EN";
    // ---
}