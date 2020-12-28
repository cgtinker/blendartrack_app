using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible Button Text Data")]
public class FlexibleUITextButtonData : ScriptableObject
{
    [Header("Header Button")]
    public AnchorPresets headerRectAnchor;
    public PivotPresets headerRectPivot;
    public int headerRectHeight;

    [Header("Inline Button")]
    public AnchorPresets inlineRectAnchor;
    public PivotPresets inlineRectPivot;
    public int inlineRectHeight;

    [Header("SupportFooter")]
    public AnchorPresets supportRectAnchor;
    public PivotPresets supportRectPivot;
    public int supportRectHeight;

    [Header("CustomHeight")]
    public int customRectHeight;

    [Header("EmptyHeight")]
    public int emptyRectHeight;
}
