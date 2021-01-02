using UnityEngine;
using TMPro;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible Text Data")]
public class FlexibleUITextData : ScriptableObject
{
    [Header("Text")]
    public TMP_FontAsset fontAsset;

    [Header("Styles")]
    public m_TextData Header;
    public m_TextData Default;
    public m_TextData Button;
    public m_TextData Hint;
    public m_TextData FileBrowserSelectionHelper;
    public m_TextData StartUp;
    public m_TextData SupportFooter;
    public m_TextData TutorialHead;
    public m_TextData TutorialMessage;
    public m_TextData TutorialSlideCounter;
}

[System.Serializable]
public class m_TextData
{
    public float textSize;
    public FontStyles fontStyle;
    public TextAlignmentOptions alignment;
    public TextOverflowModes overflow;
    public Vector2 rectSize;
    public AnchorPresets rectAnchor;
    public PivotPresets rectPivot;
}