using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flex Button")]
public class FlexButtonData : ScriptableObject
{
	[Header("Button Data")]
	public Sprite icon;
	[Space(20)]
	public float iconSize = 5;
	public int minPxSize = 125;
	public int maxPxSize = 250;
	[Space(20)]
	public AnchorPresets anchor = AnchorPresets.MiddleCenter;
	public PivotPresets pivot = PivotPresets.MiddleCenter;
	[Space(20)]
	public AnchorPresets iconAnchor = AnchorPresets.MiddleCenter;
	public PivotPresets iconPivot = PivotPresets.MiddleCenter;
}
