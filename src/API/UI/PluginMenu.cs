using System;
using System.Collections.Generic;
using FarmHelper.Helpers;
using UnityEngine;

namespace FarmHelper.API.UI;

/// <summary>
/// Abstract class for creating custom menus
/// </summary>
public abstract class PluginMenu : MonoBehaviour
{
    #region Static

    private static readonly Dictionary<Type, PluginMenu> Menus = [];
    private static readonly List<PluginMenu> History = [];

    /// <summary>
    /// Obtains the first instance of the given menu
    /// </summary>
    /// <returns>First instance found or null</returns>
    public static T GetInstance<T>() where T : PluginMenu => Menus.TryGetValue(typeof(T), out var menu) ? (T)menu : null;

    /// <summary>
    /// Obtains the previous menu in the stack
    /// </summary>
    /// <returns>Previous menu or null</returns>
    public static PluginMenu GetPrevious() => History.Count > 0 ? History[History.Count - 1] : null;

    /// <summary>
    /// Clears every menu created
    /// </summary>
    public static void ClearAll()
    {
        History.Clear();
        foreach (var menu in Menus)
        {
            if (menu.Value.gameObject)
                Destroy(menu.Value.gameObject);
        }
        Menus.Clear();
    }

    #endregion
    #region Create

    /// <summary>
    /// Creates an instance of the given menu
    /// </summary>
    /// <returns>Created menu or null if an error occurred</returns>
    public static T Create<T>() where T : PluginMenu
    {
        var gameObject = new GameObject(typeof(T).FullName);
        DontDestroyOnLoad(gameObject);
        
        var menu = gameObject.AddComponent<T>();

        var parent = GameObject.Find(Constants.MENU_PATH)?.GetComponent<Menu>();

        if (parent == null)
        {
            Log.Warning<FarmHelperPlugin>($"The object 'Menu' was not found at '{Constants.MENU_PATH}'.");
            parent = FindObjectOfType<Menu>();
        }
        
        menu._root = menu.Create(parent);

        if (menu._root == null)
        {
            Destroy(gameObject);
            return null;
        }
        
        menu.SetActive(false);
        Menus.Add(typeof(T), menu);

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

    private GameObject _root;

    private void SetActive(bool isActive)
    {
        _root?.SetActive(isActive);
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
        
        History.Add(this);
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
        while (History.Count > 0)
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
            History.RemoveAt(History.Count - 1);
        
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