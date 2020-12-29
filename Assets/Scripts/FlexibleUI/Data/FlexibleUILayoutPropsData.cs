using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible Layout Props Data")]
public class FlexibleUILayoutPropsData : ScriptableObject
{
    public m_LayoutProps Header;
    public m_LayoutProps InlineButton;
    public m_LayoutProps InlineHeader;
    public m_LayoutProps InlineEmpty;
    public m_LayoutProps SupportFooter;
    public m_CustomPadding ScrollRectContent;
    public m_CustomPadding SessionHintContent;
    public m_LayoutProps SessionHintAnchor;
    public m_LayoutProps SelectionHelper;
    public m_LayoutProps IntoAnimationSize;
    public m_LayoutProps timerFpsDisplay;
}

[System.Serializable]
public class m_LayoutProps
{
    public AnchorPresets rectAnchor;
    public PivotPresets rectPivot;
    public int rectHeight;
    public bool ignoreAnchors;
}

[System.Serializable]
public class m_CustomPadding
{
    public float paddingTop;
    public float paddingLeft;
    public float paddingRight;
    public float paddingBottom;
    public TextAnchor alignment;
}