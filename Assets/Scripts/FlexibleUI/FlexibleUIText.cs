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
        Custom
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

        var height = Screen.height;
        var width = Screen.width;
        float factor;

        if (height > width)
            factor = (float)height / 100f;

        else
            factor = (float)width / 100f;

        Vector2 rectSize;
        int textSize;

        switch (textType)
        {
            case TextType.Headline:
                textSize = (int)GetTextSize(textSkinData.headerTextSize, factor);

                AssignTextStyle(textSize, textSkinData.headerFontStyles, textSkinData.headerAlignment, textSkinData.headerOverflow);

                rectSize = new Vector2(textSkinData.headerRectSize.x * factor, textSkinData.headerRectSize.y * factor);
                AssignRectTransform(rectSize, textSkinData.headerRectAnchor, textSkinData.headerRectPivot);
                break;

            case TextType.Default:
                textSize = (int)GetTextSize(textSkinData.defaultTextSize, factor);

                AssignTextStyle(textSize, textSkinData.defaultFontStyles, textSkinData.defaultAlignment, textSkinData.defaultOverflow);

                rectSize = new Vector2(textSkinData.defaultRectSize.x * factor, textSkinData.defaultRectSize.y * factor);
                rectTransform.sizeDelta = rectSize;
                AssignRectTransform(rectSize, textSkinData.defaultRectAnchor, textSkinData.defaultRectPivot);
                break;

            case TextType.Hint:
                textSize = (int)GetTextSize(textSkinData.hintTextSize, factor);

                AssignTextStyle(textSize, textSkinData.hintFontStyles, textSkinData.hintAlignment, textSkinData.hintOverflow);

                rectSize = new Vector2(textSkinData.hintRectSize.x * factor, textSkinData.hintRectSize.y * factor);
                rectTransform.sizeDelta = rectSize;
                AssignRectTransform(rectSize, textSkinData.hintRectAnchor, textSkinData.hintRectPivot);
                break;

            case TextType.Custom:
                text.fontSize = (int)(textSkinData.customTextSize * factor);
                break;
        }
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
