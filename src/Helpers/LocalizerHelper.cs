using System.Collections.Generic;
using FarmHelper.API;

namespace FarmHelper.Helpers;

/// <summary>
/// Class helping to add texts to the game
/// </summary>
public static class LocalizerHelper
{
    /// <summary>
    /// Key => lang (EN, FR, ...)
    /// Value:
    ///      Key => key (code_tooltip_max)
    ///      Value => value (actual text)
    /// </summary>
    private static readonly Dictionary<string, Dictionary<string, string>> Languages = new();

    /// <summary>
    /// Tries to find the text of the given key in the given language or in any language.
    /// </summary>
    /// <returns>Value for the key or null if not found</returns>
    public static string GetText(string lang, string key)
    {
        if (lang == null || key == null)
            return null;
        
        while (true)
        {
            // If lang is defined
            if (Languages.TryGetValue(lang, out var lines))
            {
                // If key is defined in lang
                if (lines.TryGetValue(key, out var value)) 
                    return value;
            }

            // If key not found in ANY, 
            if (lang == Constants.Any) 
                return null;

            lang = Constants.Any;
        }
    }
    
    /// <summary>
    /// Adds the given value at the given key to the given language
    /// </summary>
    public static void Add(string key, string value, string lang = Constants.Any)
    {
        if (!Languages.ContainsKey(lang)) 
            Languages.Add(lang, new Dictionary<string, string>());

        Languages[lang][key] = value;
    }
}