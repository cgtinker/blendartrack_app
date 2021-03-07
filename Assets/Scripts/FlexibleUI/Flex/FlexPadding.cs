using UnityEngine;
using UnityEngine.UI;

public class FlexPadding : FlexPaddingOverride
{
	public RectTransform rectTransform;

	protected override void OnSkinUI()
	{
		base.OnSkinUI();
		rectTransform = GetComponent<RectTransform>();
		(float factor, float m_width, bool portrait) = ScreenSizeFactor.GetFactor();

		SetPaddingProps(data, factor);
	}

	void SetPaddingProps(FlexPaddingData data, float factor)
	{
		RectOffset rectOffset = new RectOffset();
		LayoutGroup layoutGroup = this.gameObject.GetComponent<LayoutGroup>();

		rectOffset.top = (int)(data.paddingTop * factor);
		rectOffset.bottom = (int)(data.paddingBottom * factor);
		rectOffset.left = (int)(data.paddingLeft * factor);
		rectOffset.right = (int)(data.paddingRight * factor);

		layoutGroup.padding = rectOffset;
		layoutGroup.childAlignment = data.alignment;
	}
}
