using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOrientationHandler : MonoBehaviour
{
    public RectTransform rectTransform;
    CanvasScaler canvasScaler;

    void Awake()
    {
        canvasScaler = this.gameObject.GetComponent<CanvasScaler>();
        SetOrientation();
    }

    void SetOrientation()
    {
        if (rectTransform == null)
            return;

        bool verticalOrientation = rectTransform.rect.width < rectTransform.rect.height ? true : false;

        if (verticalOrientation)
        {
            if (canvasScaler.matchWidthOrHeight != 0)
                canvasScaler.matchWidthOrHeight = 0;
        }

        else
        {
            if (canvasScaler.matchWidthOrHeight != 1)
                canvasScaler.matchWidthOrHeight = 1;
        }
    }

    void OnRectTransformDimensionsChange()
    {
        SetOrientation();
    }
}
