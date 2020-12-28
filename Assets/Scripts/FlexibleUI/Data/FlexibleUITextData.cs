using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible Text Data")]
public class FlexibleUITextData : ScriptableObject
{
    [Header("Text")]
    public TMP_FontAsset fontAsset;

    [Header("Header Text")]
    public int headerTextSize;
    public FontStyles headerFontStyles;
    public TextAlignmentOptions headerAlignment;
    public TextOverflowModes headerOverflow;

    public Vector2 headerRectSize;
    public AnchorPresets headerRectAnchor;
    public PivotPresets headerRectPivot;

    [Header("Default Text")]
    public int defaultTextSize;
    public FontStyles defaultFontStyles;
    public TextAlignmentOptions defaultAlignment;
    public TextOverflowModes defaultOverflow;

    public Vector2 defaultRectSize;
    public AnchorPresets defaultRectAnchor;
    public PivotPresets defaultRectPivot;

    [Header("Button Text")]
    public int buttonTextSize;
    public FontStyles buttonFontStyles;
    public TextAlignmentOptions buttonAlignment;
    public TextOverflowModes buttonOverflow;

    public Vector2 buttonRectSize;
    public AnchorPresets buttonRectAnchor;
    public PivotPresets buttonRectPivot;

    [Header("Hint Text")]
    public int hintTextSize;
    public FontStyles hintFontStyles;
    public TextAlignmentOptions hintAlignment;
    public TextOverflowModes hintOverflow;
    public Vector2 hintRectSize;
    public AnchorPresets hintRectAnchor;
    public PivotPresets hintRectPivot;

    [Header("Custom Text")]
    public int customTextSize;

}
