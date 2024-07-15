using System;
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
        var parameters = info.GetParameters();
        var types= parameters.Select(p => p.ParameterType).ToArray();

        // Convert the list of types into a list of argument types
        var argumentTypes = new List<Type>();
        var parameterInfos = new List<ParameterInfo>();

        for (int i = 0; i < types.Length; i++)
        {
            var type = types[i];
            
            if (!typeConverts.TryGetValue(type, out var typeToAdd) && typeof(IPyObject).IsAssignableFrom(type))
                typeToAdd = type;
            
            if (typeToAdd == null)
                continue;

            parameterInfos.Add(parameters[i]);
            argumentTypes.Add(typeToAdd);
        }
        
        bool success = Add(codeName, (interpreter, @params) => {
            // Add the missing optional parameters
            ManageOptionals(@params, parameterInfos, argumentTypes);
            
            // Manage params array
            int arrayArgs = ManageParamArray(@params.Count, parameterInfos.Count, argumentTypes);
            
            // Check if the parameters are valid
            interpreter.bf.CorrectParams(
                @params,
                argumentTypes,
                codeName
            );

            // Converts a list of types into a list of objects
            var arguments = ConvertParameters(
                @params,
                types,
                interpreter,
                arrayArgs
            );

            // Add empty params array
            if (arguments.Count != parameters.Length)
            {
                var paramsItem = parameters.Last().ParameterType.GetElementType();
                if (paramsItem != null)
                    arguments.Add(Array.CreateInstance(paramsItem, 0));
            }
            
            // Call the method
            var result = callback.DynamicInvoke(arguments.ToArray());
            
            // Return delay
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

    private static readonly Dictionary<Type, Type> typeConverts = new() {
        [typeof(double)] = typeof(PyNumber),
        [typeof(bool)] = typeof(PyBool),
    };

    /// <summary>
    /// Converts PyObjects to system types
    /// </summary>
    private static bool ConvertParameter(object value, Type wanted, out object result)
    {
        if (wanted == typeof(double) && value is PyNumber number)
        {
            result = number.num;
            return true;
        }

        if (wanted == typeof(bool) && value is PyBool boolean)
        {
            result = boolean.num != 0;
            return true;
        }

        if (wanted == typeof(string) && value is PyString text)
        {
            result = text.str;
            return true;
        }

        if (wanted == typeof(GridDirection) && value is PyGridDirection gridDirection)
        {
            result = gridDirection.dir;
            return true;
        }
        
        if (typeof(IPyObject).IsAssignableFrom(wanted) && value.GetType() == wanted)
        {
            result = value;
            return true;
        }

        result = null;
        return false;
    }

    private static void ManageOptionals(List<IPyObject> @params, List<ParameterInfo> infos, List<Type> types)
    {
        for (int i = @params.Count; i < infos.Count; i++)
        {
            if (!infos[i].IsOptional)
                break;
                
            @params.Add(
                (IPyObject) Activator.CreateInstance(types[i], infos[i].DefaultValue)
            );
        }
    }
    private static int ManageParamArray(int receivedCount, int originalCount, List<Type> types)
    {
        var arrayArgs = receivedCount - originalCount;
        types.RemoveRange(originalCount, types.Count - originalCount);
        for (int i = 0; i < arrayArgs; i++)
            types.Add(types.Last());
        
        return Math.Max(arrayArgs, 0);
    }
    private static List<object> ConvertParameters(
        List<IPyObject> @params, 
        Type[] types, 
        Interpreter interpreter,
        int arrayArgs
    ) {
        var arguments = new List<object>();
        List<IPyObject>.Enumerator it = @params.GetEnumerator();
        bool hasNext = it.MoveNext();
        while (hasNext)
        {
            object result;
            Type type = types[arguments.Count];
            Type itemType = type.GetElementType();

            if (type == typeof(Interpreter))
                result = interpreter;
            // If type is array, put all items in it
            else if (type.IsArray && itemType != null)
            {
                var tempArgs = Array.CreateInstance(itemType, arrayArgs);

                for (int k = 0; k < tempArgs.Length; k++)
                {
                    ConvertParameter(it.Current, itemType, out result);
                    tempArgs.SetValue(result, k);
                    it.MoveNext();
                }

                result = tempArgs;
                hasNext = it.MoveNext();
            }
            // If item is convertable, go to next
            else if (ConvertParameter(it.Current, type, out result))
                hasNext = it.MoveNext();
            
            // If result invalid, skip
            if (result == null)
                continue;
                
            arguments.Add(result);
        }

        return arguments;
    }
    
    #endregion
}