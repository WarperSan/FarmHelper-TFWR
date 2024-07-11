using FarmHelper.API.Attributes;
using UnityEngine;

namespace FarmHelper.API.Interfaces;

/// <summary>
/// Used to define all the plugins that uses their custom page
/// </summary>
public interface IPageCallback
{
    /// <summary>
    /// Called when creating this plugin's page
    /// </summary>
    /// <param name="farmInfo">Information of this plugin</param>
    /// <param name="page">Root of the page</param>
    public void CreatePage(FarmInfoAttribute farmInfo, GameObject page);
}