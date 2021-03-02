using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flex Layout")]
public class FlexLayoutData : ScriptableObject
{
	public RectOffset padding;
	public TextAnchor alignment;
	public float preferedFistChildSize;
	public float preferedSecondChildSize;
	public float preferedThirdChildSize;
}
