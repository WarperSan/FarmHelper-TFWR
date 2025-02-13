using UnityEngine;

namespace FarmHelper.API;

/// <summary>
/// Class that allows any class to be accessed from anywhere
/// </summary>
/// <typeparam name="T">Type of the class</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : new()
{
    private static T _instance;

    /// <summary>
    /// Unique instance of <typeparamref name="T"/>
    /// </summary>
    public static T Instance => _instance ??= new T();

    #region MonoBehaviour

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogWarning($"Another instance of {GetType().Name} has been found.");
            Destroy(gameObject);
            return;
        }

        _instance = gameObject.GetComponent<T>();

        if (!DestroyOnLoad)
            DontDestroyOnLoad(gameObject);

        OnAwake();
    }

    #endregion MonoBehaviour

    #region Virtual

    /// <summary>
    /// Defines if the singleton should be destroyed when loading a new scene
    /// </summary>
    protected virtual bool DestroyOnLoad => false;

    /// <summary>
    /// Called when <see cref="Awake"/> is called
    /// </summary>
    protected virtual void OnAwake()
    { }

    #endregion Virtual
}
