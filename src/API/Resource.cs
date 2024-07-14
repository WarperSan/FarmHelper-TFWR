namespace FarmHelper.API;

/// <summary>
/// Shortcuts for resource paths
/// </summary>
internal static class Resource
{
    public static string GetImage(string name) => $"Resources.Images.{name}";
    public static string GetDLL(string name) => $"FarmHelper.Resources.DLL.{name}";
    public static string GetBundle(string name) => $"FarmHelper.Resources.Bundles.{name}";
}