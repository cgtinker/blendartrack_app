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
            factor = height / 100;

        else
            factor = width / 100;

        Vector2 size;

        switch (textType)
        {
            case TextType.Headline:
                text.fontSize = textSkinData.headerTextSize;
                text.fontStyle = textSkinData.headerFontStyles;
                text.alignment = textSkinData.headerAlignment;
                text.overflowMode = textSkinData.headerOverflow;
                size = new Vector2(textSkinData.headerRectSize.x * factor, textSkinData.headerRectSize.y * factor);
                rectTransform.sizeDelta = size;
                rectTransform.pivot = textSkinData.headerPivot;
                break;

            case TextType.Default:
                text.fontSize = textSkinData.defaultTextSize;
                text.fontStyle = textSkinData.defaultFontStyles;
                text.alignment = textSkinData.defaultAlignment;
                text.overflowMode = textSkinData.defaultOverflow;
                size = new Vector2(textSkinData.defaultRectSize.x * factor, textSkinData.defaultRectSize.y * factor);
                rectTransform.sizeDelta = size;
                rectTransform.pivot = textSkinData.defaultPivot;
                break;

            case TextType.Hint:
                text.fontSize = textSkinData.hintTextSize;
                text.fontStyle = textSkinData.hintFontStyles;
                text.alignment = textSkinData.hintAlignment;
                text.overflowMode = textSkinData.hintOverflow;
                size = new Vector2(textSkinData.hintRectSize.x * factor, textSkinData.hintRectSize.y * factor);
                rectTransform.sizeDelta = size;
                rectTransform.pivot = textSkinData.hintPivot;
                break;

            case TextType.Custom:
                text.fontSize = textSkinData.customTextSize;
                break;
        }
    }
}
