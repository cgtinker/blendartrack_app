using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flex Layout Props")]
public class FlexLayoutPropsData : ScriptableObject
{
	public AnchorPresets rectAnchor;
	public PivotPresets rectPivot;
	public int rectHeight;
	public bool ignoreAnchors;
}
