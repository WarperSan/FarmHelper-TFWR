using UnityEngine.Events;
using UnityEngine.UI;

namespace ModHelper.Extensions;

public static class ColoredButtonExtensions
{
    public static void SetListener(this ColoredButton button, UnityAction call)
    {
        // Clear all click events
        button.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        
        // Add click event
        button.OnClick.AddListener(call);
    }
}