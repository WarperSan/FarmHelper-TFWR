using UnityEngine;

namespace ModHelper.Extensions;

public static class DebugExtensions
{
    public static void PrintComponents(this Transform origin)
    {
        var components = origin.GetComponents<Component>();
        ModHelperPlugin.LogInfo($"Components in '{origin.name}' ({components.Length}):");
        foreach (var component in components)
            ModHelperPlugin.LogInfo($"- {component.GetType().FullName}");
    }

    public static void PrintDeeper(this Transform parent, int level = 0)
    {
        ModHelperPlugin.LogInfo(new string(' ', level) + $"- {parent.name}");
        
        foreach (Transform variable in parent)
            variable.PrintDeeper(level + 1);
    }
}