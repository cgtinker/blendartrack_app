using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexUIButton : FlexUIButtonOverride
{
	public Image image;
	public Image icon;
	public Button button;
	public RectTransform rectTransform;
	public RectTransform iconTransform;

	protected override void OnSkinUI()
	{
		base.OnSkinUI();

		GetComponents();
		SetTransitionType();
		SetProperties();

		float factor;
		Vector2 rectSize;
		GetDimensions(out factor, out rectSize);

		SetButtonStyle(data, factor, rectSize, data.iconColor);
		SetRectTransform(rectSize, data.anchor, data.pivot, data.iconAnchor, data.iconPivot);
	}

	#region Set Layout Properties and Icon
	void SetButtonStyle(FlexButtonData data, float factor, Vector2 rectSize, Color color)
	{
		Vector2 size = new Vector2(data.iconSize * factor, data.iconSize * factor);

		iconTransform.sizeDelta = size;
		icon.color = color;

		if (data.icon != null)
			icon.sprite = data.icon;

	}

	void SetRectTransform(Vector2 rectSize, AnchorPresets anchor, PivotPresets pivot, AnchorPresets iconAnchor, PivotPresets iconPivot)
	{
		rectTransform.sizeDelta = rectSize;
		rectTransform.SetPivot(pivot);
		rectTransform.SetAnchor(anchor);

		iconTransform.SetAnchor(iconAnchor);
		iconTransform.SetPivot(iconPivot);
	}
	#endregion

	#region get and set base data
	private void GetDimensions(out float factor, out Vector2 rectSize)
	{
		float m_width;
		bool portrait;
		(factor, m_width, portrait) = ScreenSizeFactor.GetFactor();
		rectSize = new Vector2(base.data.defaultButtonSize * factor, base.data.defaultButtonSize * factor);
	}

	private void SetProperties()
	{
		image.type = Image.Type.Simple;
		image.preserveAspect = true;
		image.sprite = base.data.buttonBackground;
		image.color = base.data.buttonBackgroundColor;
	}

	private void SetTransitionType()
	{
		button.transition = Selectable.Transition.None;
		button.targetGraphic = image;
	}

	private void GetComponents()
	{
		image = GetComponent<Image>();
		button = GetComponent<Button>();
		rectTransform = GetComponent<RectTransform>();
		icon = this.transform.GetChild(0).GetComponent<Image>();
		iconTransform = this.transform.GetChild(0).GetComponent<RectTransform>();
	}
	#endregion
}
