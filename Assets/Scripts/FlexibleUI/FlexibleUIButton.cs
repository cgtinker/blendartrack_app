using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class FlexibleUIButton : FlexibleUIButtonOverride
{
    public enum ButtonType
    {
        Back,
        Filebrowser,
        Settings,
        Swap,
        Assign,
        Decline,
        Share,
        Delete,
        Dropdown,
        Selected,
        Deselected,
        Record,
        Break
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
        image.sprite = buttonSkinData.buttonBackground;
        image.color = buttonSkinData.buttonBackgroundColor;

        //screen size for relative button size
        var height = Screen.height;
        var width = Screen.width;
        float factor;

        if (height > width)
            factor = height / 100;

        else
            factor = width / 100;

        Vector2 size;
        //icon display and style
        switch (buttonType)
        {
            case ButtonType.Back:
                icon.sprite = buttonSkinData.backIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.backButtonSize * factor, buttonSkinData.backButtonSize * factor);
                rectTransform.sizeDelta = size * 1.25f;
                //rectTransform.anchoredPosition = buttonSkinData.backRectAnchor;
                iconTransform.sizeDelta = size;
                break;

            case ButtonType.Filebrowser:
                icon.sprite = buttonSkinData.filebrowserIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.filebrowserIconSize * factor, buttonSkinData.filebrowserIconSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Settings:
                icon.sprite = buttonSkinData.settingsIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.settingsIconSize * factor, buttonSkinData.settingsIconSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Swap:
                icon.sprite = buttonSkinData.swapIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.swapIconSize * factor, buttonSkinData.swapIconSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Assign:
                icon.sprite = buttonSkinData.assignIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.assignIconSize * factor, buttonSkinData.assignIconSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Decline:
                icon.sprite = buttonSkinData.declineIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.declineIconSize * factor, buttonSkinData.declineIconSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Share:
                icon.sprite = buttonSkinData.shareIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.shareButtonSize * factor, buttonSkinData.shareButtonSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Delete:
                icon.sprite = buttonSkinData.deleteIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.deleteButtonSize * factor, buttonSkinData.deleteButtonSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Dropdown:
                icon.sprite = buttonSkinData.dropdownIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.dropdownButtonSize * factor, buttonSkinData.dropdownButtonSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Selected:
                icon.sprite = buttonSkinData.selectedIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.selectedButtonSize * factor, buttonSkinData.selectedButtonSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Deselected:
                icon.sprite = buttonSkinData.deselectedIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.deselectedButtonSize * factor, buttonSkinData.deselectedButtonSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Record:
                icon.sprite = buttonSkinData.recordIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.recordButtonSize * factor, buttonSkinData.recordButtonSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
            case ButtonType.Break:
                icon.sprite = buttonSkinData.breakIcon;
                icon.color = buttonSkinData.iconColor;

                size = new Vector2(buttonSkinData.recordButtonSize * factor, buttonSkinData.recordButtonSize * factor);
                rectTransform.sizeDelta = size * buttonSkinData.buttonScaleFactor;
                iconTransform.sizeDelta = size;
                break;
        }
    }
}
