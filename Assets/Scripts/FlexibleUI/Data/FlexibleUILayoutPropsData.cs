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
    public m_ScrollRectData ScrollRectContent;
    public m_LayoutProps SelectionHelper;
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
public class m_ScrollRectData
{
    public float paddingTop;
    public float paddingLeft;
    public float paddingRight;
    public float paddingBottom;
    public TextAnchor alignment;
}