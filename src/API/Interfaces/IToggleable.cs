namespace FarmHelper.API.Interfaces;

/// <summary>
/// Used to define all the plugins that can be toggled
/// </summary>
public interface IToggleable
{
    /// <summary>
    /// Called when this plugin is being added
    /// </summary>
    public void OnAdd();
    
    /// <summary>
    /// Called when this plugin is being removed
    /// </summary>
    //public void OnRemove();
}