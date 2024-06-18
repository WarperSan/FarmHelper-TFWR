using System;
using UnityEngine;
using UnityEngine.UI;

namespace ModHelper.Extensions;

public static class ImageExtensions
{
    public static void LoadSprite(this Image image, string name, float size, float pixelsPerUnit = 100) 
        => image.LoadSprite(name, new Rect(Vector2.zero,Vector2.one * size), Vector2.zero, pixelsPerUnit);

    /// <summary>
    /// Tries to set to the given image
    /// </summary>
    /// <param name="image"></param>
    /// <param name="name">The case-sensitive name of the manifest resource being requested</param>
    /// <param name="rect"></param>
    /// <param name="pivot"></param>
    /// <param name="pixelsPerUnit"></param>
    private static void LoadSprite(
        this Image image, 
        string name, 
        Rect rect,
        Vector2 pivot,
        float pixelsPerUnit) 
    {
        // Get stream
        var stream = System.Reflection.Assembly.GetExecutingAssembly()
            .GetManifestResourceStream(name);

        // Copy to memory
        using var memoryStream = new System.IO.MemoryStream();
        stream?.CopyTo(memoryStream);
        
        // Create texture
        var t = new Texture2D(1, 1);
        t.LoadImage(memoryStream.ToArray());

        rect.height = Math.Min(t.height, rect.height);
        rect.width = Math.Min(t.width, rect.width);
        
        // Create sprite
        image.sprite = Sprite.Create(
            t, 
            rect, 
            pivot, 
            pixelsPerUnit
        );
    }
}