using UnityEngine;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible Horizontal Layout Data")]
public class FlexibleUILayoutData : ScriptableObject
{
    [Header("Struct")]
    public RectOffset structPadding;
    public TextAnchor stuctAlignment;
    public float structPreferedFistChildSize;
    public float structPreferedSecondChildSize;
    public float structPreferedThirdChildSize;

    [Header("Header")]
    public RectOffset headerPadding;
    public TextAnchor headerAlignment;
    public float headerPreferedFistChildSize;
    public float headerPreferedSecondChildSize;
    public float headerPreferedThirdChildSize;

    [Header("Footer")]
    public RectOffset footerPadding;
    public TextAnchor footerAlignment;
    public float footerPreferedFistChildSize;
    public float footerPreferedSecondChildSize;
    public float footerPreferedThirdChildSize;

    [Header("Button")]
    public RectOffset buttonPadding;
    public TextAnchor buttonAlignment;
    public float buttonPreferedFistChildSize;
    public float buttonPreferedSecondChildSize;
    public float buttonPreferedThirdChildSize;
}
