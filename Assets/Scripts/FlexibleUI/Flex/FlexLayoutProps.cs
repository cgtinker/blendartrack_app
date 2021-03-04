using UnityEngine;
using UnityEngine.UI;

public class FlexLayoutProps : FlexPropsOverride
{
	public RectTransform rectTransform;

	protected override void OnSkinUI()
	{
		base.OnSkinUI();
		rectTransform = GetComponent<RectTransform>();

		(float factor, float m_width, bool portrait) = ScreenSizeFactor.GetFactor();

		SetLayoutProps(data, factor, portrait);
	}

	public void SetLayoutProps(FlexLayoutPropsData data, float factor, bool portrait)
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
