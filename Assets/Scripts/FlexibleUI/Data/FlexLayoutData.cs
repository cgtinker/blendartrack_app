using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flex Layout")]
public class FlexLayoutData : ScriptableObject
{
	public FloatingPadding padding;
	public TextAnchor alignment;
	public float preferedFistChildSize;
	public float preferedSecondChildSize;
	public float preferedThirdChildSize;
}

[System.Serializable]
public class FloatingPadding
{
	public float left;
	public float right;
	public float top;
	public float bottom;
}
