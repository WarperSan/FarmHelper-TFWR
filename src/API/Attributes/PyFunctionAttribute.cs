using System;

namespace ModHelper.API.Attributes;

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

    /// <summary>
    /// Color of the function
    /// </summary>
    public string Color { get; private set; }

    /// <inheritdoc cref="PyFunctionAttribute"/>
    /// <param name="name">Name of the function to call in-game</param>
    public PyFunctionAttribute(string name)
    {
        Name = name;
    }
    
    /// <inheritdoc cref="PyFunctionAttribute(string)"/>
    public PyFunctionAttribute(string name, string color) : this(name)
    {
        Color = color;
    }
}