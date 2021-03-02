using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible Size")]
public class FlexSizeData : ScriptableObject
{
	public AnchorPresets rectAnchor;
	public PivotPresets rectPivot;
	public int rectHeight;
	public bool ignoreAnchors;
}
