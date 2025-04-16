using System;
using System.Reflection;
using FarmHelper.Helpers;
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
        {
            Log.Warning($"Could not find the lambda for '{originalMethodName}'.");
            return;
        }

        harmony.Patch(
            lambda,
            transpiler: new HarmonyMethod(transpileType.GetMethod(transpileMethodName))
        );
    }
}