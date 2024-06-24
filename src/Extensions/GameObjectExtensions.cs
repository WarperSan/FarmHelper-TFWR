using JetBrains.Annotations;
using UnityEngine;

namespace ModHelper.Extensions;

/// <summary>
/// Extension methods for GameObject
/// </summary>
public static class GameObjectExtensions
{
    /// <summary>
    /// Clones the giving object and returns it
    /// </summary>
    /// <param name="original"></param>
    /// <returns></returns>
    public static GameObject Clone(this GameObject original) 
        => original == null ? null : Object.Instantiate(original, original.transform.parent);

    /// <summary>
    /// Removes the first component of the given type
    /// </summary>
    /// <param name="gameObject"></param>
    /// <typeparam name="T"></typeparam>
    public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T component))
            Object.Destroy(component);
    }
}