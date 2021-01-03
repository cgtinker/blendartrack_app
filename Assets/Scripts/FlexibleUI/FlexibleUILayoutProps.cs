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
        SelectionHelper,
        IntroAnim,
        SessionHintContent,
        TimerFPSDisplay,
        RecordingPopup
    }

    public RectTransform rectTransform;
    public CustomLayoutProperty buttonType;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
        rectTransform = GetComponent<RectTransform>();

        float factor, m_width;
        bool portrait;
        (factor, m_width, portrait) = ScreenSizeFactor.GetFactor();


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
            case CustomLayoutProperty.IntroAnim:
                SetRectTransform(layoutPropsSkinData.IntoAnimationSize.rectAnchor, layoutPropsSkinData.IntoAnimationSize.rectPivot);

                float m_animSize = layoutPropsSkinData.IntoAnimationSize.rectHeight * m_width;
                rectTransform.sizeDelta = new Vector2((int)m_animSize, (int)m_animSize);
                break;
            case CustomLayoutProperty.ScrollRectContent:
                SetPaddingProps(layoutPropsSkinData.ScrollRectContent, factor);
                break;
            case CustomLayoutProperty.SessionHintContent:
                SetPaddingProps(layoutPropsSkinData.SessionHintContent, factor);
                SetRectTransform(layoutPropsSkinData.SessionHintAnchor.rectAnchor, layoutPropsSkinData.SessionHintAnchor.rectPivot);
                break;
            case CustomLayoutProperty.TimerFPSDisplay:
                SetLayoutProps(layoutPropsSkinData.timerFpsDisplay, factor, portrait);
                break;

            case CustomLayoutProperty.RecordingPopup:
                SetLayoutProps(layoutPropsSkinData.RecordingPopup, factor, portrait);
                break;
        }
    }

    void SetPaddingProps(m_CustomPadding data, float factor)
    {
        RectOffset rectOffset = new RectOffset();
        LayoutGroup layoutGroup = this.gameObject.GetComponent<LayoutGroup>();

        //relative offset
        rectOffset.top = (int)(data.paddingTop * factor);
        rectOffset.bottom = (int)(data.paddingBottom * factor);
        rectOffset.left = (int)(data.paddingLeft * factor);
        rectOffset.right = (int)(data.paddingRight * factor);

        //layout group
        layoutGroup.padding = rectOffset;
        layoutGroup.childAlignment = data.alignment;
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
        rectTransform.sizeDelta = new Vector2(1, (int)(height * factor));
    }
}
