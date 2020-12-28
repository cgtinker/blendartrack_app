using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleUITextButton : FlexibleUITextButtonOverride
{
    public enum TextButtonType
    {
        Header,
        Inline,
        Support,
        Custom,
        Empty
    }

    public RectTransform rectTransform;
    public TextButtonType buttonType;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
        rectTransform = GetComponent<RectTransform>();

        var height = Screen.height;
        var width = Screen.width;

        bool portrait = false;
        float factor;

        if (height > width)
        {
            factor = (float)height / 100f;
            portrait = true;
        }

        else
        {
            factor = (float)width / 100f;
            portrait = true;
        }

        switch (buttonType)
        {
            case TextButtonType.Header:
                SetRectTransform(textButtonSkinData.headerRectAnchor, textButtonSkinData.headerRectPivot);
                SetRectHeight(textButtonSkinData.headerRectHeight, factor, portrait);
                break;
            case TextButtonType.Inline:
                SetRectTransform(textButtonSkinData.inlineRectAnchor, textButtonSkinData.inlineRectPivot);
                SetRectHeight(textButtonSkinData.inlineRectHeight, factor, portrait);
                break;
            case TextButtonType.Support:
                SetRectTransform(textButtonSkinData.supportRectAnchor, textButtonSkinData.supportRectPivot);
                SetRectHeight(textButtonSkinData.supportRectHeight, factor, portrait);
                break;
            case TextButtonType.Custom:
                SetRectHeight(textButtonSkinData.customRectHeight, factor, portrait);
                break;
            case TextButtonType.Empty:
                SetRectHeight(textButtonSkinData.emptyRectHeight, factor, portrait);
                break;
        }
    }

    public void SetRectTransform(AnchorPresets anchor, PivotPresets pivot)
    {
        rectTransform.SetAnchor(anchor);
        rectTransform.SetPivot(pivot);
    }

    void SetRectHeight(float height, float factor, bool portrait)
    {
        if (portrait)
            rectTransform.sizeDelta = new Vector2(1, (int)(height * factor));


        else
            rectTransform.sizeDelta = new Vector2((int)(height * factor), 1);
    }
}
