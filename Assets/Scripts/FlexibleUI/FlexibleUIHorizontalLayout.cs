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
        FPS
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
        var height = Screen.height;
        var width = Screen.width;
        float factor;
        bool portrait = false;
        RectOffset rectOffset = new RectOffset();

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
                //layout element size
                firstChild.preferredWidth = layoutSkinData.structPreferedFistChildSize * factor;
                secondChild.preferredWidth = layoutSkinData.structPreferedSecondChildSize * factor;
                thirdChild.preferredWidth = layoutSkinData.structPreferedThirdChildSize * factor;

                //relative offset
                rectOffset.top = (int)(layoutSkinData.structPadding.top * factor);
                rectOffset.bottom = (int)(layoutSkinData.structPadding.bottom * factor);
                rectOffset.left = (int)(layoutSkinData.structPadding.left * factor);
                rectOffset.right = (int)(layoutSkinData.structPadding.right * factor);

                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutSkinData.stuctAlignment;
                break;

            case LayoutType.Header:
                //layout element size
                firstChild.preferredWidth = layoutSkinData.headerPreferedFistChildSize * factor;
                secondChild.preferredWidth = layoutSkinData.headerPreferedSecondChildSize * factor;
                thirdChild.preferredWidth = layoutSkinData.headerPreferedThirdChildSize * factor;

                //relative offset
                rectOffset.top = (int)(layoutSkinData.headerPadding.top * factor);
                rectOffset.bottom = (int)(layoutSkinData.headerPadding.bottom * factor);
                rectOffset.left = (int)(layoutSkinData.headerPadding.left * factor);
                rectOffset.right = (int)(layoutSkinData.headerPadding.right * factor);

                //layout group
                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutSkinData.footerAlignment;
                break;

            case LayoutType.Footer:
                //layout element size
                firstChild.preferredWidth = layoutSkinData.footerPreferedFistChildSize * factor;
                secondChild.preferredWidth = layoutSkinData.footerPreferedSecondChildSize * factor;
                thirdChild.preferredWidth = layoutSkinData.footerPreferedThirdChildSize * factor;

                //relative offset
                rectOffset.top = (int)(layoutSkinData.footerPadding.top * factor);
                rectOffset.bottom = (int)(layoutSkinData.footerPadding.bottom * factor);
                rectOffset.left = (int)(layoutSkinData.footerPadding.left * factor);
                rectOffset.right = (int)(layoutSkinData.footerPadding.right * factor);

                //layout group
                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutSkinData.footerAlignment;
                break;

            case LayoutType.Button:
                //layout element size
                firstChild.preferredWidth = (layoutSkinData.buttonPreferedFistChildSize) * factor;
                secondChild.preferredWidth = (layoutSkinData.buttonPreferedSecondChildSize) * factor;
                thirdChild.preferredWidth = layoutSkinData.buttonPreferedThirdChildSize * factor;

                //relative offset
                rectOffset.top = (int)(layoutSkinData.buttonPadding.top * factor);
                rectOffset.bottom = (int)(layoutSkinData.buttonPadding.bottom * factor);
                rectOffset.left = (int)(layoutSkinData.buttonPadding.left * factor);
                rectOffset.right = (int)(layoutSkinData.buttonPadding.right * factor);

                //layout group
                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutSkinData.buttonAlignment;
                break;
            case LayoutType.HeaderButton:
                //layout element size
                firstChild.preferredWidth = (layoutSkinData.headerButtonPreferedFistChildSize) * factor;
                secondChild.preferredWidth = (layoutSkinData.headerButtonPreferedSecondChildSize) * factor;
                thirdChild.preferredWidth = layoutSkinData.headerButtonPreferedThirdChildSize * factor;

                //relative offset
                rectOffset.top = (int)(layoutSkinData.headerButtonPadding.top * factor);
                rectOffset.bottom = (int)(layoutSkinData.headerButtonPadding.bottom * factor);
                rectOffset.left = (int)(layoutSkinData.headerButtonPadding.left * factor);
                rectOffset.right = (int)(layoutSkinData.headerButtonPadding.right * factor);

                //layout group
                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutSkinData.headerButtonAlignment;
                break;
            case LayoutType.FPS:
                //layout element size
                firstChild.preferredWidth = (layoutSkinData.fpsButtonPreferedFistChildSize) * factor;
                secondChild.preferredWidth = (layoutSkinData.fpsButtonPreferedSecondChildSize) * factor;
                thirdChild.preferredWidth = layoutSkinData.fpsButtonPreferedThirdChildSize * factor;

                //relative offset
                rectOffset.top = (int)(layoutSkinData.fpsButtonPadding.top * factor);
                rectOffset.bottom = (int)(layoutSkinData.fpsButtonPadding.bottom * factor);
                rectOffset.left = (int)(layoutSkinData.fpsButtonPadding.left * factor);
                rectOffset.right = (int)(layoutSkinData.fpsButtonPadding.right * factor);

                //layout group
                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutSkinData.fpsButtonAlignment;
                break;
        }


    }
}
