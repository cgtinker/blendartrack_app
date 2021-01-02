using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible Horizontal Layout Data")]
public class FlexibleUILayoutData : ScriptableObject
{
    public m_LayoutData Struct;
    public m_LayoutData Header;
    public m_LayoutData Footer;
    public m_LayoutData Button;
    public m_LayoutData HeaderButton;
    public m_LayoutData fpsPlacement;
    public m_LayoutData ScrollContent;
    public m_LayoutData TutorialContent;
}

[System.Serializable]
public class m_LayoutData
{
    public RectOffset padding;
    public TextAnchor alignment;
    public float preferedFistChildSize;
    public float preferedSecondChildSize;
    public float preferedThirdChildSize;
}