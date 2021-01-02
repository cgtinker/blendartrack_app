using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class FlexibleUIHorizontalLayout : FlexibleUILayoutOverride
{
    public enum LayoutType
    {
        Struct,
        Header,
        Footer,
        Button,
        HeaderButton,
        FPS,
        Tutorial
    }

    public LayoutGroup layoutGroup;
    public LayoutType layoutType;
    //childs
    public LayoutElement firstChild;
    public LayoutElement secondChild;
    public LayoutElement thirdChild;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();
        layoutGroup = GetComponent<LayoutGroup>();

        //assign childs
        firstChild = this.gameObject.transform.GetChild(0).gameObject.GetComponent<LayoutElement>();
        secondChild = this.gameObject.transform.GetChild(1).gameObject.GetComponent<LayoutElement>();
        thirdChild = this.gameObject.transform.GetChild(2).gameObject.GetComponent<LayoutElement>();

        //screen size for relative button size
        float factor, m_width;
        bool portrait;
        (factor, m_width, portrait) = ScreenSizeFactor.GetFactor();

        RectOffset rectOffset = new RectOffset();


        switch (layoutType)
        {
            case LayoutType.Struct:
                SetLayoutProps(layoutSkinData.Struct, factor);
                break;

            case LayoutType.Header:
                SetLayoutProps(layoutSkinData.Header, factor);
                break;

            case LayoutType.Footer:
                SetLayoutProps(layoutSkinData.Footer, factor);
                break;

            case LayoutType.Button:
                SetLayoutProps(layoutSkinData.Button, factor);
                break;
            case LayoutType.HeaderButton:
                SetLayoutProps(layoutSkinData.HeaderButton, factor);
                break;
            case LayoutType.FPS:
                SetLayoutProps(layoutSkinData.fpsPlacement, factor);
                break;

            case LayoutType.Tutorial:
                SetLayoutProps(layoutSkinData.TutorialContent, factor);
                break;
        }
    }

    void SetLayoutProps(m_LayoutData data, float factor)
    {
        //layout element size
        firstChild.preferredWidth = data.preferedFistChildSize * factor;
        secondChild.preferredWidth = data.preferedSecondChildSize * factor;
        thirdChild.preferredWidth = data.preferedThirdChildSize * factor;

        //relative offset
        RectOffset rectOffset = new RectOffset();
        rectOffset.top = (int)(data.padding.top * factor);
        rectOffset.bottom = (int)(data.padding.bottom * factor);
        rectOffset.left = (int)(data.padding.left * factor);
        rectOffset.right = (int)(data.padding.right * factor);

        //layout group
        layoutGroup.padding = rectOffset;
        layoutGroup.childAlignment = data.alignment;
    }
}
