using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class FlexLayout : FlexLayoutOverride
{
	public LayoutGroup layoutGroup;
	public LayoutElement firstChild;
	public LayoutElement secondChild;
	public LayoutElement thirdChild;

	protected override void OnSkinUI()
	{
		base.OnSkinUI();
		layoutGroup = GetComponent<LayoutGroup>();

		GetLayoutComponents();
		(float factor, float m_width, bool portrait) = ScreenSizeFactor.GetFactor();

		SetLayout(data, factor);
	}

	#region set layout
	void SetLayout(FlexLayoutData data, float factor)
	{
		SetLayoutElementSize(data, factor);
		RectOffset rectOffset = GetPadding(data, factor);

		layoutGroup.padding = rectOffset;
		layoutGroup.childAlignment = data.alignment;
	}

	private void SetLayoutElementSize(FlexLayoutData data, float factor)
	{
		firstChild.preferredWidth = data.preferedFistChildSize * factor;
		secondChild.preferredWidth = data.preferedSecondChildSize * factor;
		thirdChild.preferredWidth = data.preferedThirdChildSize * factor;
	}
	#endregion

	#region components and padding
	private static RectOffset GetPadding(FlexLayoutData data, float factor)
	{
		RectOffset rectOffset = new RectOffset();
		rectOffset.top = (int)(data.padding.top * factor);
		rectOffset.bottom = (int)(data.padding.bottom * factor);
		rectOffset.left = (int)(data.padding.left * factor);
		rectOffset.right = (int)(data.padding.right * factor);
		return rectOffset;
	}

	private void GetLayoutComponents()
	{
		firstChild = this.gameObject.transform.GetChild(0).gameObject.GetComponent<LayoutElement>();
		secondChild = this.gameObject.transform.GetChild(1).gameObject.GetComponent<LayoutElement>();
		thirdChild = this.gameObject.transform.GetChild(2).gameObject.GetComponent<LayoutElement>();
	}
	#endregion
}