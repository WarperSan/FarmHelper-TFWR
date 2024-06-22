using ModHelper.API;
using UnityEngine;

namespace ModHelper.Extensions;

public static class DebugExtensions
{
    public static void PrintComponents(this Transform origin)
    {
        var components = origin.GetComponents<Component>();
        FarmPlugin.Msg<ModHelperPlugin>($"Components in '{origin.name}' ({components.Length}):");
        foreach (var component in components)
            FarmPlugin.Msg<ModHelperPlugin>($"- {component.GetType().FullName}");
    }

    public static void PrintDeeper(this Transform parent, int level = 0)
    {
        FarmPlugin.Msg<ModHelperPlugin>(new string(' ', level) + $"- {parent.name}");
        
        foreach (Transform variable in parent)
            variable.PrintDeeper(level + 1);
    }
}