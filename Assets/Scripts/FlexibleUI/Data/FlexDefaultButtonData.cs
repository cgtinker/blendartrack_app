using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flex Default Button Data")]
public class FlexDefaultButtonData : ScriptableObject
{
	[Header("Button Background")]
	public Sprite buttonBackground;
	public Color buttonBackgroundColor = Color.black;
	public float buttonScaleFactor = 1.3f;


	[Header("Default Button Data")]
	public Color iconColor = Color.white;
	public int defaultButtonSize = 5;
}
