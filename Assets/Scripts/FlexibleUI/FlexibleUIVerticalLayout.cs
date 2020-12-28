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
        var height = Screen.height;
        var width = Screen.width;
        float factor;
        bool portrait = false;
        RectOffset rectOffset = new RectOffset();

        float firstChildPreferredHeight;
        float secondChildPreferredHeight;
        float thirdChildPreferredHeight;

        int rectOffsetTop;
        int rectOffsetBottom;
        int rectOffsetLeft;
        int rectOffsetRight;

        TextAnchor childAlignment;

        if (height > width)
        {
            factor = (float)height / 100f;
            portrait = true;
        }

        else
            factor = (float)width / 100f;

        switch (layoutType)
        {
            case LayoutType.Struct:
                //layout elements
                firstChildPreferredHeight = layoutSkinData.structPreferedFistChildSize * factor;
                secondChildPreferredHeight = layoutSkinData.structPreferedSecondChildSize * factor;
                thirdChildPreferredHeight = layoutSkinData.structPreferedThirdChildSize * factor;
                SetChildSize(firstChildPreferredHeight, secondChildPreferredHeight, thirdChildPreferredHeight);

                //relative offset
                rectOffsetTop = (int)(layoutSkinData.structPadding.top * factor);
                rectOffsetBottom = (int)(layoutSkinData.structPadding.bottom * factor);
                rectOffsetLeft = (int)(layoutSkinData.structPadding.left * factor);
                rectOffsetRight = (int)(layoutSkinData.structPadding.right * factor);
                rectOffset = GetRectOffset(rectOffset, rectOffsetTop, rectOffsetBottom, rectOffsetLeft, rectOffsetRight);

                childAlignment = layoutSkinData.stuctAlignment;
                SetLayoutProperties(rectOffset, childAlignment);
                break;

            case LayoutType.Header:
                //layout element size
                firstChildPreferredHeight = layoutSkinData.headerPreferedFistChildSize * factor;
                secondChildPreferredHeight = layoutSkinData.headerPreferedSecondChildSize * factor;
                thirdChildPreferredHeight = layoutSkinData.headerPreferedThirdChildSize * factor;
                SetChildSize(firstChildPreferredHeight, secondChildPreferredHeight, thirdChildPreferredHeight);

                //relative offset
                rectOffsetTop = (int)(layoutSkinData.headerPadding.top * factor);
                rectOffsetBottom = (int)(layoutSkinData.headerPadding.bottom * factor);
                rectOffsetLeft = (int)(layoutSkinData.headerPadding.left * factor);
                rectOffsetRight = (int)(layoutSkinData.headerPadding.right * factor);
                rectOffset = GetRectOffset(rectOffset, rectOffsetTop, rectOffsetBottom, rectOffsetLeft, rectOffsetRight);

                //layout group
                childAlignment = layoutSkinData.footerAlignment;
                SetLayoutProperties(rectOffset, childAlignment);
                break;

            case LayoutType.Footer:
                //layout element size
                firstChildPreferredHeight = layoutSkinData.footerPreferedFistChildSize * factor;
                secondChildPreferredHeight = layoutSkinData.footerPreferedSecondChildSize * factor;
                thirdChildPreferredHeight = layoutSkinData.footerPreferedThirdChildSize * factor;
                SetChildSize(firstChildPreferredHeight, secondChildPreferredHeight, thirdChildPreferredHeight);

                //relative offset
                rectOffsetTop = (int)(layoutSkinData.footerPadding.top * factor);
                rectOffsetBottom = (int)(layoutSkinData.footerPadding.bottom * factor);
                rectOffsetLeft = (int)(layoutSkinData.footerPadding.left * factor);
                rectOffsetRight = (int)(layoutSkinData.footerPadding.right * factor);
                rectOffset = GetRectOffset(rectOffset, rectOffsetTop, rectOffsetBottom, rectOffsetLeft, rectOffsetRight);

                //layout group
                childAlignment = layoutSkinData.footerAlignment;
                SetLayoutProperties(rectOffset, childAlignment);
                break;

            case LayoutType.Button:
                //layout element size
                firstChildPreferredHeight = (layoutSkinData.buttonPreferedFistChildSize * factor);
                secondChildPreferredHeight = (layoutSkinData.buttonPreferedSecondChildSize * factor);
                thirdChildPreferredHeight = (layoutSkinData.buttonPreferedThirdChildSize * factor);
                SetChildSize(firstChildPreferredHeight, secondChildPreferredHeight, thirdChildPreferredHeight);

                //relative offset
                rectOffsetTop = (int)(layoutSkinData.buttonPadding.top * factor);
                rectOffsetBottom = (int)(layoutSkinData.buttonPadding.bottom * factor);
                rectOffsetLeft = (int)(layoutSkinData.buttonPadding.left * factor);
                rectOffsetRight = (int)(layoutSkinData.buttonPadding.right * factor);
                rectOffset = GetRectOffset(rectOffset, rectOffsetTop, rectOffsetBottom, rectOffsetLeft, rectOffsetRight);

                //layout group
                childAlignment = layoutSkinData.buttonAlignment;
                SetLayoutProperties(rectOffset, childAlignment);
                break;

            case LayoutType.HeaderButton:
                //layout element size
                firstChildPreferredHeight = (layoutSkinData.headerButtonPreferedFistChildSize * factor);
                secondChildPreferredHeight = (layoutSkinData.headerButtonPreferedSecondChildSize * factor);
                thirdChildPreferredHeight = (layoutSkinData.headerButtonPreferedThirdChildSize * factor);
                SetChildSize(firstChildPreferredHeight, secondChildPreferredHeight, thirdChildPreferredHeight);

                //relative offset
                rectOffsetTop = (int)(layoutSkinData.headerButtonPadding.top * factor);
                rectOffsetBottom = (int)(layoutSkinData.headerButtonPadding.bottom * factor);
                rectOffsetLeft = (int)(layoutSkinData.headerButtonPadding.left * factor);
                rectOffsetRight = (int)(layoutSkinData.headerButtonPadding.right * factor);
                rectOffset = GetRectOffset(rectOffset, rectOffsetTop, rectOffsetBottom, rectOffsetLeft, rectOffsetRight);

                //layout group
                childAlignment = layoutSkinData.headerButtonAlignment;
                SetLayoutProperties(rectOffset, childAlignment);
                break;
            case LayoutType.FPS:
                //layout element size
                firstChildPreferredHeight = (layoutSkinData.fpsButtonPreferedFistChildSize) * factor;
                secondChildPreferredHeight = (layoutSkinData.fpsButtonPreferedSecondChildSize) * factor;
                thirdChildPreferredHeight = layoutSkinData.fpsButtonPreferedThirdChildSize * factor;
                SetChildSize(firstChildPreferredHeight, secondChildPreferredHeight, thirdChildPreferredHeight);

                //relative offset
                rectOffsetTop = (int)(layoutSkinData.fpsButtonPadding.top * factor);
                rectOffsetBottom = (int)(layoutSkinData.fpsButtonPadding.bottom * factor);
                rectOffsetLeft = (int)(layoutSkinData.fpsButtonPadding.left * factor);
                rectOffsetRight = (int)(layoutSkinData.fpsButtonPadding.right * factor);
                rectOffset = GetRectOffset(rectOffset, rectOffsetTop, rectOffsetBottom, rectOffsetLeft, rectOffsetRight);

                //layout group
                layoutGroup.padding = rectOffset;
                childAlignment = layoutSkinData.fpsButtonAlignment;
                SetLayoutProperties(rectOffset, childAlignment);
                break;
        }
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
