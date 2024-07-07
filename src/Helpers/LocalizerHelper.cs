using System.Collections.Generic;

namespace ModHelper.Helpers;

/// <summary>
/// Class helping to add texts to the game
/// </summary>
public static class LocalizerHelper
{
    /// <summary>
    /// Adds a string to the current language
    /// </summary>
    /// <param name="key">Key of the string</param>
    /// <param name="value">String to use</param>
    public static void Add(string key, string value) 
        => typeof(Localizer).GetStaticField<Dictionary<string, string>>("language")[key] = value;
}