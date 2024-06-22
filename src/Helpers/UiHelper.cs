using UnityEngine;

namespace ModHelper.Helpers;

public static class UiHelper
{
    private const string TEMPLATE = Constants.TITLE_PATH + "/PlayButton";
    
    public static bool AddTitleButton(int x, int y, out ColoredButton button)
    {
        button = null;

        // Get template
        var template = GameObject.Find(TEMPLATE);

        if (template == null)
            return false;
        
        // Create button
        var newBtn = Object.Instantiate(
            template, 
            template.transform.parent
        );

        // If not found, clean
        if (!newBtn.TryGetComponent(out button))
        {
            Object.Destroy(newBtn);
            return false;
        }
        
        // Configure button
        newBtn.GetComponent<RectTransform>().localPosition += new Vector3(
            (newBtn.GetComponent<RectTransform>().rect.width + Constants.MAIN_TITLE_OFFSET_X) * x, 
            -Constants.MAIN_TITLE_OFFSET_Y * y
        );

        return true;
    }

    public static void SetActiveTitle(bool isActive) => GameObject.Find(Constants.TITLE_PATH)?.SetActive(isActive);
}