using UnityEngine.Events;
using UnityEngine.UI;

namespace ModHelper.Extensions;

/// <summary>
/// Extensions methods for <see cref="ColoredButton"/>
/// </summary>
public static class ColoredButtonExtensions
{
    /// <summary>
    /// Sets the listener of this button to the given action
    /// </summary>
    /// <param name="button"></param>
    /// <param name="callback">Code to run upon a click</param>
    public static void SetListener(this ColoredButton button, UnityAction callback)
    {
        // Clear all click events
        button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        
        // Add click event
        button.OnClick.AddListener(callback);
    }
}