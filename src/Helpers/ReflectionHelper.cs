using System;
using System.Reflection;

namespace ModHelper.Helpers;

/// <summary>
/// Class helping with Reflection
/// </summary>
public static class ReflectionHelper
{
    private const BindingFlags INSTANCE_FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
    private const BindingFlags STATIC_FLAGS = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static;
    
    private const BindingFlags ALL_FLAGS = INSTANCE_FLAGS | STATIC_FLAGS;
    
    #region Fields

    /// <summary>
    /// Fetches the value of the given field for this object. 
    /// </summary>
    /// <param name="instance">Object to fetch the field from</param>
    /// <param name="name">Name of the field</param>
    /// <typeparam name="U">Type of the field</typeparam>
    /// <returns>Value of the field</returns>
    public static U GetField<U>(this object instance, string name)
        => GetField<U>(instance.GetType(), instance, name, INSTANCE_FLAGS);

    /// <summary>
    /// Fetches the value of the given static field for this class
    /// </summary>
    /// <param name="type">Class to fetch the field from</param>
    /// <param name="name">Name of the field</param>
    /// <typeparam name="U">Type of the field</typeparam>
    /// <returns>Value of the field</returns>
    public static U GetStaticField<U>(this Type type, string name) 
        => GetField<U>(type, null, name, STATIC_FLAGS);
    
    private static U GetField<U>(Type type, object instance, string name, BindingFlags flags) 
        => (U) type.GetField(name, flags)?.GetValue(instance);

    #endregion

    #region Properties

    /// <summary>
    /// Fetches the value of the given property for this object. 
    /// </summary>
    /// <param name="instance">Object to fetch the property from</param>
    /// <param name="name">Name of the property</param>
    /// <typeparam name="U">Type of the property</typeparam>
    /// <returns>Value of the property</returns>
    public static U GetProperty<U>(this object instance, string name)
        => GetProperty<U>(instance.GetType(), instance, name, INSTANCE_FLAGS);

    /// <summary>
    /// Fetches the value of the given static property for this class
    /// </summary>
    /// <param name="type">Class to fetch the property from</param>
    /// <param name="name">Name of the property</param>
    /// <typeparam name="U">Type of the property</typeparam>
    /// <returns>Value of the property</returns>
    public static U GetProperty<U>(this Type type, string name) 
        => GetProperty<U>(type, null, name, STATIC_FLAGS);
    
    private static U GetProperty<U>(Type type, object instance, string name, BindingFlags flags) 
        => (U) type.GetProperty(name, flags)?.GetValue(instance);

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
    public static U CallMethod<U>(this object instance, string name, params object[] parameters) where U : class
        => CallMethod<U>(instance.GetType(), instance, name, ALL_FLAGS, parameters);
    
    /// <inheritdoc cref="CallMethod{U}(Type, object, string, BindingFlags, object[])"/>
    public static void CallMethod(this object instance, string name, params object[] parameters) 
        => instance.CallMethod<object>(name, parameters);
    
    private static U CallMethod<U>(Type type, object instance, string name, BindingFlags flags, params object[] parameters) 
        where U : class
    {
        try
        {
            var method = type.GetMethod(name, flags);

            if (method == null)
                throw new NullReferenceException($"No method is named '{name}' for the type '{type.FullName}'.");
            
            return method.Invoke(instance, parameters) as U;
        }
        catch (TargetInvocationException e)
        {
            throw e.InnerException ?? e;
        }
    }
    
    #endregion
}