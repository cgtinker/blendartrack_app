using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FlexibleUIText : FlexibleUITextOverride
{
    public enum TextType
    {
        Headline,
        Default,
        Hint,
        ButtonText,
        StartUp,
        SelectionHelper,
        SupportFooter,
        TutorialHead,
        TutorialMessage,
        TutorialSlideCounter
    }

    public TextMeshProUGUI text;
    public TextType textType;
    public RectTransform rectTransform;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        text = GetComponent<TextMeshProUGUI>();
        rectTransform = GetComponent<RectTransform>();
        text.font = textSkinData.fontAsset;

        float factor, m_width;
        bool portrait;
        (factor, m_width, portrait) = ScreenSizeFactor.GetFactor();

        switch (textType)
        {
            case TextType.Headline:
                AssignTextStyle(textSkinData.Header, factor);
                break;

            case TextType.Default:
                AssignTextStyle(textSkinData.Default, factor);
                break;

            case TextType.Hint:
                AssignTextStyle(textSkinData.Hint, factor);
                break;

            case TextType.SelectionHelper:
                text.fontSize = (int)GetTextSize(textSkinData.FileBrowserSelectionHelper.textSize, factor);
                break;

            case TextType.ButtonText:
                AssignTextStyle(textSkinData.Button, factor);
                break;

            case TextType.StartUp:
                AssignTextStyle(textSkinData.StartUp, factor);
                break;

            case TextType.SupportFooter:
                AssignTextStyle(textSkinData.SupportFooter, factor);
                break;

            case TextType.TutorialHead:
                AssignTextStyle(textSkinData.TutorialHead, factor);
                break;

            case TextType.TutorialMessage:
                AssignTextStyle(textSkinData.TutorialMessage, factor);
                break;

            case TextType.TutorialSlideCounter:
                AssignTextStyle(textSkinData.TutorialSlideCounter, factor);
                break;
        }
    }

    void AssignTextStyle(m_TextData data, float factor)
    {
        int textSize = (int)GetTextSize(data.textSize, factor);
        AssignTextStyle(textSize, data.fontStyle, data.alignment, data.overflow);
        Vector2 rectSize = new Vector2(data.rectSize.x * factor, data.rectSize.y * factor);
        rectTransform.sizeDelta = rectSize;
        AssignRectTransform(rectSize, data.rectAnchor, data.rectPivot);
    }

    float GetTextSize(float m_textSize, float factor)
    {
        return m_textSize * factor / 10;
    }

    void AssignTextStyle(int fontSize, FontStyles fontStyles, TextAlignmentOptions alignment, TextOverflowModes overflow)
    {
        text.fontSize = fontSize;
        text.fontStyle = fontStyles;
        text.alignment = alignment;
        text.overflowMode = overflow;
    }

    void AssignRectTransform(Vector2 rectSize, AnchorPresets anchor, PivotPresets pivot)
    {
        rectTransform.sizeDelta = rectSize;
        rectTransform.SetPivot(pivot);
        rectTransform.SetAnchor(anchor);
    }
}
