using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "Flexible Text Data")]
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
    public Vector2 headerPivot;

    [Header("Default Text")]
    public int defaultTextSize;
    public FontStyles defaultFontStyles;
    public TextAlignmentOptions defaultAlignment;
    public TextOverflowModes defaultOverflow;
    public Vector2 defaultRectSize;
    public Vector2 defaultPivot;

    [Header("Hint Text")]
    public int hintTextSize;
    public FontStyles hintFontStyles;
    public TextAlignmentOptions hintAlignment;
    public TextOverflowModes hintOverflow;
    public Vector2 hintRectSize;
    public Vector2 hintPivot;

    [Header("Custom Text")]
    public int customTextSize;

}
