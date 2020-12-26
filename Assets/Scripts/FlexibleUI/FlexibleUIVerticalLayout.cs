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
        Button
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
        RectOffset rectOffset = new RectOffset();

        if (height > width)
            factor = height / 100;

        else
            factor = width / 100;

        switch (layoutType)
        {
            case LayoutType.Struct:
                //layout elements
                firstChild.preferredHeight = layoutSkinData.structPreferedFistChildSize * factor;
                secondChild.preferredHeight = layoutSkinData.structPreferedSecondChildSize * factor;
                thirdChild.preferredHeight = layoutSkinData.structPreferedThirdChildSize * factor;

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
                firstChild.preferredHeight = layoutSkinData.headerPreferedFistChildSize * factor;
                secondChild.preferredHeight = layoutSkinData.headerPreferedSecondChildSize * factor;
                thirdChild.preferredHeight = layoutSkinData.headerPreferedThirdChildSize * factor;

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
                firstChild.preferredHeight = layoutSkinData.footerPreferedFistChildSize * factor;
                secondChild.preferredHeight = layoutSkinData.footerPreferedSecondChildSize * factor;
                thirdChild.preferredHeight = layoutSkinData.footerPreferedThirdChildSize * factor;

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
                firstChild.preferredHeight = layoutSkinData.buttonPreferedFistChildSize * factor;
                secondChild.preferredHeight = layoutSkinData.buttonPreferedSecondChildSize * factor;
                thirdChild.preferredHeight = layoutSkinData.buttonPreferedThirdChildSize * factor;

                //relative offset
                rectOffset.top = (int)(layoutSkinData.buttonPadding.top * factor);
                rectOffset.bottom = (int)(layoutSkinData.buttonPadding.bottom * factor);
                rectOffset.left = (int)(layoutSkinData.buttonPadding.left * factor);
                rectOffset.right = (int)(layoutSkinData.buttonPadding.right * factor);

                //layout group
                layoutGroup.padding = rectOffset;
                layoutGroup.childAlignment = layoutSkinData.buttonAlignment;
                break;
        }
    }
}
