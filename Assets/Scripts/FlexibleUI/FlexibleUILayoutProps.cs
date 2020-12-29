using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleUILayoutProps : FlexibleUILayoutPropsOverride
{
    public enum CustomLayoutProperty
    {
        HeaderTop,
        InlineButton,
        SupportFooter,
        InlineHeader,
        InlineEmpty,
        ScrollRectContent,
        SelectionHelper
    }

    public RectTransform rectTransform;
    public CustomLayoutProperty buttonType;

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
            case CustomLayoutProperty.HeaderTop:
                SetLayoutProps(layoutPropsSkinData.Header, factor, portrait);
                break;
            case CustomLayoutProperty.InlineButton:
                SetLayoutProps(layoutPropsSkinData.InlineButton, factor, portrait);
                break;
            case CustomLayoutProperty.SupportFooter:
                SetLayoutProps(layoutPropsSkinData.SupportFooter, factor, portrait);
                break;
            case CustomLayoutProperty.InlineHeader:
                SetLayoutProps(layoutPropsSkinData.InlineHeader, factor, portrait);
                break;
            case CustomLayoutProperty.InlineEmpty:
                SetLayoutProps(layoutPropsSkinData.InlineEmpty, factor, portrait);
                break;
            case CustomLayoutProperty.SelectionHelper:
                SetLayoutProps(layoutPropsSkinData.SelectionHelper, factor, portrait);
                break;
            case CustomLayoutProperty.ScrollRectContent:
                RectOffset rectOffset = new RectOffset();
                LayoutGroup layoutGroup = this.gameObject.GetComponent<LayoutGroup>();

                //relative offset
                rectOffset.top = (int)(layoutPropsSkinData.ScrollRectContent.paddingTop * factor);
                rectOffset.bottom = (int)(layoutPropsSkinData.ScrollRectContent.paddingBottom * factor);
                rectOffset.left = (int)(layoutPropsSkinData.ScrollRectContent.paddingLeft * factor);
                rectOffset.right = (int)(layoutPropsSkinData.ScrollRectContent.paddingRight * factor);

                //layout group
                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutPropsSkinData.ScrollRectContent.alignment;
                break;
        }
    }

    public void SetLayoutProps(m_LayoutProps data, float factor, bool portrait)
    {
        if (!data.ignoreAnchors)
            SetRectTransform(data.rectAnchor, data.rectPivot);
        SetRectHeight(data.rectHeight, factor, portrait);
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
