using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using ModHelper.API;
using UnityEngine;

namespace ModHelper.Helpers;

/// <summary>
/// Class helping for functionalities regarding in-game code
/// </summary>
public static class CodeHelper
{
    #region Color

    /// <inheritdoc cref="AddCodeColor(string,string,bool)"/>
    public static void AddCodeColor(string pattern, Color color, bool addFromStart = false)
        => AddCodeColor(pattern, "#" + ColorUtility.ToHtmlStringRGB(color), addFromStart);

    /// <summary>
    /// Adds a code tag for the given pattern of the given color.
    /// </summary>
    /// <param name="pattern">Pattern to match</param>
    /// <param name="color">Color of the match</param>
    /// <param name="addFromStart">Determines if the tag is added to the start or to the end of the list</param>
    /// <remarks>
    /// If <paramref name="addFromStart"/> is true, this code tag can overwrite existing code tags.
    /// </remarks>
    public static void AddCodeColor(string pattern, string color, bool addFromStart = false)
    {
        // If invalid color, skip
        if (!IsValidColor(color))
        {
            Log.Error<ModHelperPlugin>($"'{color}' is not a valid HEX code and will not be put on the pattern '{pattern}'.");
            return;
        }
        
        var colors = typeof(CodeUtilities).GetStaticField<List<(Regex, string)>>("colors");
        
        colors.Insert(
            addFromStart ? 0 : color.Length,
            (new Regex(pattern), color)
        );
    }

    private static bool IsValidColor(string color)
    {
        var code = color;

        // Should start with #
        if (!code.StartsWith('#'))
            return false;

        // Wrong length
        if (code.Length != 7)
            return false;

        // Check if string is valid int
        return int.TryParse(code[1..], NumberStyles.HexNumber, null, out _);
    }
    
    #endregion

    #region Function

    /// <summary>
    /// Adds built-in functions to the game.
    /// </summary>
    /// <param name="callbacks">Functions to call when executing each built-in function</param>
    public static void AddFunction(params Delegate[] callbacks)
    {
        foreach (var callback in callbacks)
            AddFunction(callback);
    }

    /// <summary>
    /// <inheritdoc cref="AddFunction(string,System.Func{System.Collections.Generic.List{IPyObject},double})"/>
    /// </summary>
    /// <param name="callback">Function to call when executing</param>
    /// <remarks>
    /// To use this method, the given function must have <see cref="PyFunctionAttribute"/>.
    /// </remarks>
    public static void AddFunction(Delegate callback)
    {
        var info = callback.GetMethodInfo();
        var attr = info.GetCustomAttribute<PyFunctionAttribute>();

        // Must have PyFunctionAttribute
        if (attr == null)
        {
            Log.Info<ModHelperPlugin>($"'{info.Name}' does not contain the attribute '{nameof(PyFunctionAttribute)}'.");
            return;
        }

        var codeName = attr.Name;
        var allTypes = info.GetParameters().Select(p => p.ParameterType).ToArray();
        var paramTypes = allTypes.Where(p => typeof(IPyObject).IsAssignableFrom(p)).ToList();
        
        AddFunction(codeName, (interpreter, @params) =>
        {
            // Check for params
            interpreter.bf.CallMethod("CorrectParams", 
                @params, 
                paramTypes,
                codeName
            );

            // Convert arguments
            var result = callback.DynamicInvoke(args: ConvertParams(allTypes, @params));

            return result as double? ?? 0;
        });
    }
    
    /// <inheritdoc cref="AddFunction(string,System.Func{System.Collections.Generic.List{IPyObject},double})"/>
    public static void AddFunction(string name, Func<Interpreter, List<IPyObject>, double> callback)
    {
        AddFunction(name, prm =>
        {
            if (callback == null)
                return 0;

            return callback.Invoke(Saver.Inst.mainFarm.workspace.interpreter, prm);
        });
    }

    /// <summary>
    /// Adds a built-in function to the game.
    /// </summary>
    /// <param name="name">Name of the function</param>
    /// <param name="callback">Code to run when executing</param>
    public static void AddFunction(string name, Func<List<IPyObject>, double> callback)
    {
        var functions = Saver.Inst.mainFarm.workspace.interpreter.bf.functions;

        if (!functions.TryAdd(name, callback))
        {
            Log.Info<ModHelperPlugin>($"A function named '{name}' already existed.");
            return;            
        }

        Saver.Inst.mainFarm.GetField<HashSet<string>>("unlocks").Add(name.ToLower());
    }

    private static object[] ConvertParams(Type[] types, List<IPyObject> parameters)
    {
        var args = new List<object>();

        var i = 0;
        foreach (var t in types)
        {
            object obj;

            if (t == typeof(Interpreter))
                obj = Saver.Inst.mainFarm.workspace.interpreter;
            else
            {
                obj = Convert.ChangeType(parameters[i], t);
                i++;
            }
                
            args.Add(obj);
        }

        return args.ToArray();
    }

    #endregion
}