using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace FarmHelper.API.UI;

public abstract class PluginMenu : MonoBehaviour
{
    #region Static

    private static readonly List<(Type, PluginMenu)> menus = [];
    private static readonly Stack<PluginMenu> history = new();

    public static T GetInstance<T>() where T : PluginMenu 
        => menus.FirstOrDefault(menu => menu.Item2 is T).Item2 as T;

    public static PluginMenu GetPrevious() 
        => history.TryPeek(out PluginMenu menu) ? menu : null;

    #endregion
    #region Create

    /// <summary>
    /// Creates an instance of the given menu
    /// </summary>
    /// <returns>Created menu or null if an error occurred</returns>
    public static T Create<T>() where T : PluginMenu
    {
        GameObject gameObject = new GameObject(typeof(T).FullName);
        DontDestroyOnLoad(gameObject);
        
        T menu = gameObject.AddComponent<T>();
        
        menu.root = menu.Create();

        if (menu.root == null)
        {
            Destroy(gameObject);
            return null;
        }
        
        menu.SetActive(false);
        menus.Add((typeof(T), menu));

        return menu;
    }
    
    protected virtual GameObject Create() => null;

    #endregion
    #region Set Active

    private GameObject root;

    private void SetActive(bool isActive)
    {
        root.SetActive(isActive);
        gameObject.SetActive(isActive);
    }

    #endregion
    #region Open

    public static void Open<T>() where T : PluginMenu => GetInstance<T>()?.Open();
    
    public void Open()
    {
        SetActive(true);

        GetPrevious()?.SetActive(false);
        
        history.Push(this);
    }

    #endregion
    #region Close

    public static void Close<T>() where T : PluginMenu => GetInstance<T>()?.Close();

    public static void CloseAll()
    {
        while (history.Count > 0)
            GetPrevious()?.Close();
    }
    
    public void Close()
    {
        SetActive(false);

        var prev = GetPrevious();

        if (prev == this)
            history.Pop();
        
        GetPrevious()?.SetActive(true);
    }
    
    #endregion
    #region Info

    public abstract string DisplayName { get; }

    #endregion
}