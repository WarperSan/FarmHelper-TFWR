using System;

namespace ModHelper.API;

/// <summary>
/// Attribute marking every function that should be added as a built-in function
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class PyFunctionAttribute : Attribute
{
    /// <summary>
    /// Name of the function to call in-game
    /// </summary>
    public string Name { get; private set; }
    
    /// <inheritdoc cref="PyFunctionAttribute"/>
    /// <param name="name">Name of the function to call in-game</param>
    public PyFunctionAttribute(string name)
    {
        Name = name;
    }
}