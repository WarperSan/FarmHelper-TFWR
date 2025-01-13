namespace FarmHelper.API.Interfaces;

/// <summary>
/// Defines a plugin that can be toggled
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