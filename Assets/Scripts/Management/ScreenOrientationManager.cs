using UnityEngine;
using System.Collections;

public static class ScreenOrientationManager
{
    public static void SetOrientation(bool portait)
    {
        try
        {
            SetAutoRotation(portait);

            if (!portait)
                Screen.orientation = ScreenOrientation.AutoRotation;

            else
                Screen.orientation = ScreenOrientation.Portrait;
        }

        catch
        {
            Debug.LogError("Cannot set correct screen orientation");
        }
    }

    private static void SetAutoRotation(bool cur)
    {
        Screen.autorotateToLandscapeLeft = cur;
        Screen.autorotateToLandscapeRight = cur;
    }
}
