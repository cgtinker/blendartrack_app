using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class FlexibleUIButton : FlexibleUI
{
    public enum ButtonType
    {
        Back,
        Filebrowser,
        Settings,
        Swap,
        Record,
    }

    public Image image;
    public Image icon;
    public Button button;
    public ButtonType buttonType;
    public RectTransform rectTransform;
    public RectTransform iconTransform;

    protected override void OnSkinUI()
    {
        base.OnSkinUI();

        //getting components to manipulate
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();

        icon = this.transform.GetChild(0).GetComponent<Image>();
        iconTransform = this.transform.GetChild(0).GetComponent<RectTransform>();

        //button transition
        button.transition = Selectable.Transition.None;
        button.targetGraphic = image;

        //button style
        image.type = Image.Type.Simple;
        image.preserveAspect = true;
        image.sprite = skinData.buttonBackground;
        image.color = skinData.buttonBackgroundColor;

        //screen size for relative button size
        var height = Screen.height;
        var width = Screen.width;
        float factor;

        if (height > width)
        {
            factor = height / 100;
        }

        else
        {
            factor = width / 100;
        }

        Vector2 size;
        //icon display and style
        switch (buttonType)
        {
            case ButtonType.Back:
                icon.sprite = skinData.backIcon;
                icon.color = skinData.iconColor;

                size = new Vector2(skinData.backButtonSize * factor, skinData.backButtonSize * factor);
                rectTransform.sizeDelta = size;
                iconTransform.sizeDelta = size;
                break;

            case ButtonType.Filebrowser:
                icon.sprite = skinData.filebrowserIcon;
                icon.color = skinData.iconColor;

                size = new Vector2(skinData.filebrowserIconSize * factor, skinData.filebrowserIconSize * factor);
                rectTransform.sizeDelta = size;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Settings:
                icon.sprite = skinData.settingsIcon;
                icon.color = skinData.iconColor;

                size = new Vector2(skinData.settingsIconSize * factor, skinData.settingsIconSize * factor);
                rectTransform.sizeDelta = size;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Swap:
                icon.sprite = skinData.swapIcon;
                icon.color = skinData.iconColor;

                size = new Vector2(skinData.swapIconSize * factor, skinData.swapIconSize * factor);
                rectTransform.sizeDelta = size;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Record:
                icon.sprite = skinData.recordIcon;
                icon.color = skinData.iconColor;

                size = new Vector2(skinData.recordIconSize * factor, skinData.recordIconSize * factor);
                rectTransform.sizeDelta = size;
                iconTransform.sizeDelta = size;
                break;
        }
    }
}
