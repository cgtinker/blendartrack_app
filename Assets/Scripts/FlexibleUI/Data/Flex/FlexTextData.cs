using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "FlexibleUI/Flex Text")]
public class FlexTextData : ScriptableObject
{
	public float textSize;
	public FontStyles fontStyle;
	public TextAlignmentOptions alignment;
	public TextOverflowModes overflow;
	public Vector2 rectSize;
	public AnchorPresets rectAnchor;
	public PivotPresets rectPivot;

	public bool onlyAdjustTextSize;
}
