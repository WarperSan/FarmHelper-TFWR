using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FarmHelper.API;
using FarmHelper.API.Attributes;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping to add built-in functions to the game
/// </summary>
public static class FuncHelper
{
    /// <summary>
    /// Adds a built-in function
    /// </summary>
    /// <param name="name">Name of the function</param>
    /// <param name="callback">Code to run when executing the function</param>
    /// <returns>
    /// Success of the addition
    /// </returns>
    public static bool Add(string name, Func<List<IPyObject>, double> callback)
    {
        var functions = Saver.Inst.mainFarm.workspace.interpreter.bf.functions;

        // If name already used, skip
        if (!functions.TryAdd(name, callback))
        {
            Log.Info<FarmHelperPlugin>($"A function named '{name}' already existed.");
            return false; 
        }

        // Unlocks the function
        Saver.Inst.mainFarm.unlocks.Add(name.ToLower());
        return true;
    }
    
    /// <summary>
    /// Adds a built-in function
    /// </summary>
    /// <param name="name">Name of the function</param>
    /// <param name="callback">Code to run when executing the function</param>
    /// <returns>
    /// Success of the addition
    /// </returns>
    public static bool Add(string name, Func<Interpreter, List<IPyObject>, double> callback)
        => Add(name, prm => callback?.Invoke(Saver.Inst.mainFarm.workspace.interpreter, prm) ?? 0);

    /// <summary>
    /// Adds a built-in function
    /// </summary>
    /// <param name="callback">Function to run when executing the function</param>
    /// <returns>
    /// Success of the addition
    /// </returns>
    /// <remarks>
    /// This method uses <see cref="PyFunctionAttribute"/> to fetch the data.
    /// </remarks>
    public static bool Add(Delegate callback)
    {
        var info = callback.GetMethodInfo();
        var attr = info.GetCustomAttribute<PyFunctionAttribute>();

        // Must have PyFunctionAttribute
        if (attr == null)
        {
            Log.Warning<FarmHelperPlugin>($"'{info.Name}' does not contain the attribute '{nameof(PyFunctionAttribute)}'.");
            return false;
        }
        
        // Fetch data
        var codeName = attr.Name;
        var allTypes = info.GetParameters().Select(p => p.ParameterType).ToArray();
        var requiredTypes = allTypes.Where(p => typeof(IPyObject).IsAssignableFrom(p)).ToList();
        var extraTypes = allTypes.LastOrDefault(t => t.IsArray)?.GetElementType();
        
        bool success = Add(codeName, (interpreter, @params) =>
        {
            // Create type list
            var count = Math.Max(@params.Count, requiredTypes.Count);
            var types = new List<Type>();

            for (int i = 0; i < count; i++)
                types.Add(i < requiredTypes.Count ? requiredTypes[i] : extraTypes);
            
            // Check for params
            interpreter.bf.CorrectParams(
                @params,
                types,
                codeName
            );

            // Convert arguments
            var arguments = ConvertParams(interpreter, allTypes, @params);
            
            var result = callback.DynamicInvoke(arguments);

            return result as double? ?? 0;
        });

        // If failed, skip
        if (!success)
            return false;
        
        return attr.Color != null && ColorHelper.Add(codeName + @"(?=\(.*?)", attr.Color, true);
    }

    /// <summary>
    /// Adds all built-in functions in the given class
    /// </summary>
    /// <typeparam name="T">Class to take the functions from</typeparam>
    /// <returns>Success of all the additions</returns>
    public static bool AddAll<T>() where T : class
    {
        bool success = true;
        
        foreach (var method in typeof(T).GetMethods(Constants.ALL_FLAGS))
        {
            var attr = method.GetCustomAttribute<PyFunctionAttribute>();
            
            if (attr == null)
                continue;

            var paramTypes = method.GetParameters().Select(param => param.ParameterType);
            var paramAndReturnTypes = paramTypes.Append(method.ReturnType).ToArray();
            var delegateType = Expression.GetDelegateType(paramAndReturnTypes);
            
            success &= Add(method.CreateDelegate(delegateType));
        }
        
        return success;
    }
    
    #region Arguments Converting

    private static object[] ConvertParams(Interpreter interpreter, Type[] types, List<IPyObject> parameters)
    {
        object[] args = new object [types.Length];

        var i = 0;
        for (int j = 0; j < args.Length; j++)
        {
            Type t = types[j];
            Type itemType = t.GetElementType();
            bool isExtra = false;
            object obj;

            // If parameter is Interpreter
            if (t == typeof(Interpreter))
            {
                obj = interpreter;
                isExtra = true;
            }
            // If parameter is params
            // Convert the rest of parameters
            else if (t.IsArray && itemType != null)
            {
                var tempArgs = Array.CreateInstance(itemType, parameters.Count - i);

                for (int k = 0; k < tempArgs.Length; k++)
                    tempArgs.SetValue(Convert.ChangeType(parameters[i + k], itemType), k);

                obj = tempArgs;
                j = args.Length - 1;
            }
            // Everything else
            else
                obj = Convert.ChangeType(parameters[i], t);

            args[j] = obj;

            if (!isExtra)
                i++;
        }

        return args;
    }

    #endregion
}