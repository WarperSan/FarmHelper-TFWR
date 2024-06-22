using JetBrains.Annotations;
using UnityEngine;

namespace ModHelper.Extensions;

public static class GameObjectExtensions
{
    public static GameObject Clone(this GameObject original)
    {
        if (original == null)
            return null;
        
        return Object.Instantiate(original, original.transform.parent);
    }

    public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
    {
        if (gameObject.TryGetComponent(out T component))
            Object.Destroy(component);
    }
}