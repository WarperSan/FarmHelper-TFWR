using System;
using System.Collections.Generic;
using System.Linq;
using FarmHelper.Helpers;
using UnityEngine;

namespace FarmHelper.API.UI;

/// <summary>
/// Abstract class for creating custom menus
/// </summary>
public abstract class PluginMenu : MonoBehaviour
{
    #region Static

    private static readonly List<(Type, PluginMenu)> menus = [];
    private static readonly Stack<PluginMenu> history = new();

    /// <summary>
    /// Obtains the first instance of the given menu
    /// </summary>
    /// <returns>First instance found or null</returns>
    public static T GetInstance<T>() where T : PluginMenu 
        => menus.FirstOrDefault(menu => menu.Item2 is T).Item2 as T;

    /// <summary>
    /// Obtains the previous menu in the stack
    /// </summary>
    /// <returns>Previous menu or null</returns>
    public static PluginMenu GetPrevious() 
        => history.TryPeek(out PluginMenu menu) ? menu : null;

    /// <summary>
    /// Clears every menu created
    /// </summary>
    public static void ClearAll()
    {
        history.Clear();
        foreach (var (_, menu) in menus)
        {
            if (menu.gameObject != null)
                Destroy(menu.gameObject);
        }
        menus.Clear();
    }

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

        Menu parent = GameObject.Find(Constants.MENU_PATH)?.GetComponent<Menu>();

        if (parent == null)
        {
            Log.Warning<FarmHelperPlugin>($"The object 'Menu' was not found at '{Constants.MENU_PATH}'.");
            parent = FindObjectOfType<Menu>();
        }
        
        menu.root = menu.Create(parent);

        if (menu.root == null)
        {
            Destroy(gameObject);
            return null;
        }
        
        menu.SetActive(false);
        menus.Add((typeof(T), menu));

        return menu;
    }
    
    /// <summary>
    /// Called when creating a new instance of this menu
    /// </summary>
    /// <param name="parent">Instance of Menu</param>
    /// <returns>Root of the UI for this menu</returns>
    protected virtual GameObject Create(Menu parent) => null;

    #endregion
    #region Set Active

    private GameObject root;

    private void SetActive(bool isActive)
    {
        root?.SetActive(isActive);
        gameObject.SetActive(isActive);
    }

    #endregion
    #region Open

    /// <summary>
    /// Opens the first instance of the given menu
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void Open<T>() where T : PluginMenu => GetInstance<T>()?.Open();
    
    /// <summary>
    /// Opens this menu
    /// </summary>
    public void Open()
    {
        SetActive(true);

        GetPrevious()?.SetActive(false);
        
        history.Push(this);
    }

    #endregion
    #region Close

    /// <summary>
    /// Closes the first instance of the given menu
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void Close<T>() where T : PluginMenu => GetInstance<T>()?.Close();

    /// <summary>
    /// Closes all the menus currently opened
    /// </summary>
    public static void CloseAll()
    {
        while (history.Count > 0)
            GetPrevious()?.Close();
    }
    
    /// <summary>
    /// Closes this menu
    /// </summary>
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

    /// <summary>
    /// Display name  of this menu
    /// </summary>
    public abstract string DisplayName { get; }

    #endregion
}