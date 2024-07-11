using System.Text.RegularExpressions;
using UnityEngine;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping to add syntax colors to the game
/// </summary>
public static class ColorHelper
{
    /// <summary>
    /// Adds a syntax color to the game
    /// </summary>
    /// <param name="pattern">Pattern to respect to use this color</param>
    /// <param name="color">Color to use in HEX</param>
    /// <param name="fromStart">Determines if the pattern is added at the start or at the end</param>
    /// <returns>Success of the addition</returns>
    /// <remarks>
    /// If <paramref name="fromStart"/> is true, this pattern can overwrite existing patterns
    /// </remarks>
    public static bool Add(string pattern, string color, bool fromStart = false)
    {
        // If invalid color, skip
        if (!IsValidColor(color))
        {
            Log.Warning<FarmHelperPlugin>($"'{color}' is not a valid HEX code and will not be put on the pattern '{pattern}'.");
            return false;
        }
        
        // Add to colors
        // Never add before comments
        CodeUtilities.colors.Insert(
            fromStart ? System.Math.Min(CodeUtilities.colors.Count, 1) : CodeUtilities.colors.Count,
            (new Regex(pattern), color)
        );
        
        return true;
    }

    /// <summary>
    /// Adds a syntax color to the game
    /// </summary>
    /// <param name="pattern">Pattern to respect to use this color</param>
    /// <param name="color">Color to use</param>
    /// <param name="fromStart">Determines if the pattern is added at the start or at the end</param>
    /// <returns>Success of the addition</returns>
    /// <remarks>
    /// If <paramref name="fromStart"/> is true, this pattern can overwrite existing patterns
    /// </remarks>
    public static bool Add(string pattern, Color color, bool fromStart = false)
        => Add(pattern, "#" + ColorUtility.ToHtmlStringRGB(color), fromStart);
    
    private static bool IsValidColor(string color)
    {
        // Should start with #
        if (!color.StartsWith('#'))
            return false;

        // Wrong length
        if (color.Length != 7)
            return false;

        // Check if string is valid int
        return int.TryParse(color[1..], System.Globalization.NumberStyles.HexNumber, null, out _);
    }
}