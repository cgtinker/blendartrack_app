using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutGroup))]
public class FlexibleUIVerticalLayout : FlexibleUILayoutOverride
{
    public enum LayoutType
    {
        Struct,
        Header,
        Footer,
        Button,
        HeaderButton,
        FPS
    };

    public LayoutType layoutType;

    public LayoutGroup layoutGroup;
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


        switch (layoutType)
        {
            case LayoutType.Struct:
                SetLayout(layoutSkinData.Struct, factor);
                break;

            case LayoutType.Header:
                SetLayout(layoutSkinData.Header, factor);
                break;

            case LayoutType.Footer:
                SetLayout(layoutSkinData.Footer, factor);
                break;

            case LayoutType.Button:
                SetLayout(layoutSkinData.Button, factor);
                break;

            case LayoutType.HeaderButton:
                SetLayout(layoutSkinData.HeaderButton, factor);
                break;
            case LayoutType.FPS:
                SetLayout(layoutSkinData.fpsPlacement, factor);
                break;
        }
    }

    void SetLayout(m_LayoutData data, float factor)
    {
        SetChildSize(data.preferedFistChildSize * factor, data.preferedSecondChildSize * factor, data.preferedThirdChildSize * factor);
        RectOffset rectOffset = new RectOffset();
        rectOffset = GetRectOffset(rectOffset, (int)(data.padding.top * factor), (int)(data.padding.bottom * factor), (int)(data.padding.left * factor), (int)(data.padding.right * factor));
        layoutGroup.padding = rectOffset;
        SetLayoutProperties(rectOffset, data.alignment);
    }

    void SetChildSize(float firstChildSize, float secondChildSize, float thirdChildSize)
    {
        //layout element size
        firstChild.preferredHeight = (int)firstChildSize;
        secondChild.preferredHeight = (int)secondChildSize;
        thirdChild.preferredHeight = (int)thirdChildSize;
    }

    RectOffset GetRectOffset(RectOffset rectOffset, int paddingTop, int paddingBot, int paddingLeft, int paddingRight)
    {
        //relative offset
        rectOffset.top = paddingTop;
        rectOffset.bottom = paddingBot;
        rectOffset.left = paddingLeft;
        rectOffset.right = paddingRight;

        return rectOffset;
    }

    void SetLayoutProperties(RectOffset rectOffset, TextAnchor alignment)
    {
        //layout group
        layoutGroup.padding = rectOffset;
        layoutGroup.childAlignment = alignment;
    }
}
