using System;
using System.Reflection;
using HarmonyLib;

namespace FarmHelper.Extensions;

/// <summary>
/// Extensions methods for <see cref="Harmony"/>
/// </summary>
public static class HarmonyExtension
{
    /// <summary>
    /// Transpiles the given lambda with the given method
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public static void TranspileLambda(
        this Harmony harmony,
        Type originalType,
        string originalMethodName,
        Type transpileType,
        string transpileMethodName
    ) {
        MethodInfo? lambda = null;

        foreach (var types in originalType.GetNestedTypes(BindingFlags.NonPublic))
        {
            foreach (var method in types.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (!method.Name.Contains("b__"))
                    continue;
                
                if (!method.Name.Contains(originalMethodName))
                    continue;
                
                lambda = method;
                break;
            }
            
            if (lambda != null)
                break;
        }

        if (lambda == null)
            throw new NullReferenceException($"Could not find the lambda for '{originalMethodName}'.");

        harmony.Patch(
            lambda,
            transpiler: new HarmonyMethod(transpileType.GetMethod(transpileMethodName))
        );
    }

    /// <summary>
    /// Swaps the labels of the two given <see cref="CodeInstruction"/>
    /// </summary>
    public static void SwapLabels(this CodeInstruction origin, CodeInstruction destination)
    {
        (origin.labels, destination.labels) = (destination.labels, origin.labels);
    }

    /// <summary>
    /// Finds the local field with the given name in the given type
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    public static FieldInfo GetLocalField(this Type type, string fieldName)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        
        var displayClass = AccessTools.FirstInner(type, t => t.GetField(fieldName, flags) != null);
        var field = displayClass?.GetField(fieldName, flags);

        if (field == null)
            throw new NullReferenceException($"Could not find the local field '{fieldName}' in the class '{type.Name}'.");

        return field;
    }
}