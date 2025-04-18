using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AgriCore.Helpers;

/// <summary>
/// Class helping to add syntax colors to the game
/// </summary>
public static class ColorHelper
{
    private readonly static Dictionary<string, string> ColorPerGroup = [];
    private readonly static Dictionary<string, List<string>> PatternsPerGroup = [];
    
    /// <summary>
    /// Adds a pattern for a given color
    /// </summary>
    /// <param name="pattern">REGEX pattern to respect</param>
    /// <param name="color">Color to use in HEX</param>
    /// <param name="group">Name of the REGEX group</param>
    /// <returns>Success of the addition</returns>
    public static bool Add(string pattern, string color, string group)
    {
        if (!IsValidColor(color))
        {
            Log.Warning($"'{color}' is not a valid HEX code and will not be put on the pattern '{pattern}'.");
            return false;
        }
        
        if (string.IsNullOrEmpty(group))
        {
            Log.Warning($"Cannot add the pattern '{pattern}' to an empty group.");
            return false;
        }

        ColorPerGroup[group] = color;

        if (!PatternsPerGroup.TryGetValue(group, out var patterns))
        {
            PatternsPerGroup[group] = [pattern];
            return true;
        }
        
        patterns.Add(pattern);
        return true;
    }

    /// <summary>
    /// Compiles the registered patterns into a regex string
    /// </summary>
    public static string GetRegexString()
    {
        var patternGroups = new List<string>();

        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var group in PatternsPerGroup)
        {
            var groupName = group.Key;
            var patterns = string.Join("|", group.Value);
            
            patternGroups.Add($"(?<{groupName}>(?:{patterns}))");
        }
        
        return string.Join("|", patternGroups);
    }

    /// <summary>
    /// Adds the color to the given text depending on the match
    /// </summary>
    public static string? GetColoredText(Match match)
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var group in ColorPerGroup)
        {
            var groupName = group.Key;
     
            if (!match.Groups[groupName].Success)
                continue;

            return $"<color={group.Value}>{match.Value}</color>";
        }

        return null;
    }

    private static bool IsValidColor(string color)
    {
        // Should start with #
        if (!color.StartsWith("#"))
            return false;

        // Wrong length
        if (color.Length != 7)
            return false;

        // Check if string is valid int
        return int.TryParse(color.Substring(1), System.Globalization.NumberStyles.HexNumber, null, out _);
    }
}