using FarmHelper.Helpers;
using UnityEngine;

namespace FarmHelper.Extensions;

/// <summary>
/// Methods used for debugging
/// </summary>
public static class DebugExtensions
{
    /// <summary>
    /// Prints all the components along this component
    /// </summary>
    /// <param name="origin"></param>
    public static void PrintComponents(this Component origin)
    {
        var components = origin.GetComponents<Component>();
        Log.Info<FarmHelperPlugin>($"Components in '{origin.name}' ({components.Length}):");
        foreach (var component in components)
            Log.Info<FarmHelperPlugin>($"- {component.GetType().FullName}");
    }

    /// <summary>
    /// Prints the hierarchy of GameObject, starting from this Transform
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="level"></param>
    public static void PrintDeeper(this Transform parent, int level = 0)
    {
        Log.Info<FarmHelperPlugin>(new string(' ', level) + $"- {parent.name}");
        
        foreach (Transform variable in parent)
            variable.PrintDeeper(level + 1);
    }
}