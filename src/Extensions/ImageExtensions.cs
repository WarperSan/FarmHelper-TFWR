using System;
using JetBrains.Annotations;
using ModHelper.API;
using UnityEngine;
using UnityEngine.UI;

namespace ModHelper.Extensions;

public static class ImageExtensions
{
    public static void LoadSprite<T>(this Image image, string name, float size, float pixelsPerUnit = 100) where T : FarmPlugin
        => image.LoadSprite<T>(name, new Rect(Vector2.zero,Vector2.one * size), Vector2.zero, pixelsPerUnit);

    /// <summary>
    /// Tries to set the given asset to the image
    /// </summary>
    /// <param name="image"></param>
    /// <param name="name">Path of the resources from the root</param>
    /// <param name="rect"></param>
    /// <param name="pivot">Pivot of the sprite</param>
    /// <param name="pixelsPerUnit"></param>
    private static void LoadSprite<T>(
        this Image image, 
        string name, 
        Rect rect,
        Vector2 pivot,
        float pixelsPerUnit
    ) where T: FarmPlugin {
        
        var t = GetTexture<T>($"{typeof(T).Namespace}.{name}");

        // If no texture found
        if (t == null)
        {
            image.sprite = null;
            return;
        }
        
        // If rect too big for texture
        if (t.height < rect.height || t.width < rect.width)
        {
            rect.height = Math.Min(t.height, rect.height);
            rect.width = Math.Min(t.width, rect.width);
            ModHelperPlugin.LogWarning($"Resized rect to fit: {rect.height}x{rect.width}");
        }
       
        // Create sprite
        image.sprite = Sprite.Create(
            t, 
            rect, 
            pivot, 
            pixelsPerUnit
        );
    }

    [CanBeNull]
    private static Texture2D GetTexture<T>(string name) where T : FarmPlugin
    {
        // Read bytes
        var stream = System.Reflection.Assembly.GetAssembly(typeof(T)).GetManifestResourceStream(name);

        using var memoryStream = new System.IO.MemoryStream();
        stream?.CopyTo(memoryStream);
        
        var bytes = memoryStream.ToArray();
        
        // If no content found, skip
        if (bytes.Length == 0)
            return null;
        
        // Create texture
        var t = new Texture2D(1, 1);
        t.LoadImage(bytes);

        return t;
    }
}