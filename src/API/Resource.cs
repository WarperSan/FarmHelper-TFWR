namespace ModHelper.API;

/// <summary>
/// Shortcuts for resource paths
/// </summary>
internal static class Resource
{
    public static string GetImage(string name) => $"Resources.Images.{name}";
    public static string GetDLL(string name) => $"ModHelper.Resources.DLL.{name}";
    public static string GetBundle(string name) => $"ModHelper.Resources.Bundles.{name}";
}