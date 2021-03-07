using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FlexText : FlexTextOverride
{
	public TextMeshProUGUI text;
	public RectTransform rectTransform;

	protected override void OnSkinUI()
	{
		base.OnSkinUI();

		text = GetComponent<TextMeshProUGUI>();
		rectTransform = GetComponent<RectTransform>();
		text.font = font.fontAsset;

		(float factor, float m_width, bool portrait) = ScreenSizeFactor.GetFactor();
		AssignTextStyle(data, factor);
	}

	void AssignTextStyle(FlexTextData data, float factor)
	{
		int textSize = (int)GetTextSize(data.textSize, factor);
		if (data.onlyAdjustTextSize)
			return;

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
