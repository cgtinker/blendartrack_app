using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrenOrientationHandling : MonoBehaviour
{
    void OnEnable()
    {
        SetPortraitMode(false);
    }

    void OnDisable()
    {
        SetPortraitMode(true);
    }

    public void SetPortraitMode(bool portrait)
    {
        Debug.Log("Set Portrait Mode: " + portrait);
        if (portrait)
        {
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;

            Screen.orientation = ScreenOrientation.Portrait;
        }

        else
        {
            Screen.autorotateToLandscapeLeft = true;
            Screen.autorotateToLandscapeRight = true;

            Screen.orientation = ScreenOrientation.AutoRotation;
        }
    }
}
