using System;
using System.Reflection;

namespace ModHelper.Helpers;

public static class ReflectionHelper
{
    #region Fields

    /// <summary>
    /// Fetches the value of the given private field for this object. 
    /// </summary>
    /// <param name="instance">Object to fetch the field from</param>
    /// <param name="name">Name of the field</param>
    /// <typeparam name="U">Type of the field</typeparam>
    /// <returns>Value of the field</returns>
    public static U GetField<U>(this object instance, string name)
        => GetField<U>(instance.GetType(), instance, name, BindingFlags.NonPublic | BindingFlags.Instance);

    /// <summary>
    /// Fetches the value of the given private static field for this class
    /// </summary>
    /// <param name="type">Class to fetch the field from</param>
    /// <param name="name">Name of the field</param>
    /// <typeparam name="U">Type of the field</typeparam>
    /// <returns>Value of the field</returns>
    public static U GetStaticField<U>(this Type type, string name) 
        => GetField<U>(type, null, name, BindingFlags.NonPublic | BindingFlags.Static);
    
    private static U GetField<U>(Type type, object instance, string name, BindingFlags flags) 
        => (U) type.GetField(name, flags)?.GetValue(instance);

    #endregion

    #region Methods

    /// <summary>
    /// Calls the method of this object
    /// </summary>
    /// <param name="instance">Object to call the method on</param>
    /// <param name="name">Name of the method</param>
    /// <param name="parameters">Parameters of the method</param>
    /// <typeparam name="U">Type of the return value</typeparam>
    /// <returns>Return value</returns>
    public static U CallMethod<U>(this object instance, string name, params object[] parameters)
        where U : class
        => CallMethod<U>(instance.GetType(), instance, name, BindingFlags.NonPublic | BindingFlags.Instance,
            parameters);
    
    /// <inheritdoc cref="CallMethod{U}"/>
    public static void CallMethod(this object instance, string name, params object[] parameters) 
        => instance.CallMethod<object>(name, parameters);
    
    private static U CallMethod<U>(Type type, object instance, string name, BindingFlags flags,
        params object[] parameters) where U : class
    {
        try
        {
            return type.GetMethod(name, flags)?.Invoke(instance, parameters) as U;
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException ?? e;
        }
    }
    
    #endregion
}