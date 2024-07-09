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
    /// <param name="color">Color of the function</param>
    public PyFunctionAttribute(string name, string color = null)
    {
        Name = name;
        Color = color;
    }
}