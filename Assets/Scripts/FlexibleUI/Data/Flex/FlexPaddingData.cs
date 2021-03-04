using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flex Padding Data")]
public class FlexPaddingData : ScriptableObject
{
	public float paddingTop;
	public float paddingLeft;
	public float paddingRight;
	public float paddingBottom;
	public TextAnchor alignment;
}
