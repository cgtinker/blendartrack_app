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
        Record
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
            factor = (float)height / 100f;

        else
            factor = (float)width / 100f;

        Vector2 size;
        Vector2 rectSize;
        rectSize = new Vector2(buttonSkinData.defaultButtonSize * factor, buttonSkinData.defaultButtonSize * factor);

        //icon display and style
        switch (buttonType)
        {
            case ButtonType.Back:
                size = new Vector2(buttonSkinData.backIconSize * factor, buttonSkinData.backIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.backIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);

                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.backButtonAnchor, pivot: buttonSkinData.backButtonPivot);
                break;

            case ButtonType.Filebrowser:
                size = new Vector2(buttonSkinData.filebrowserIconSize * factor, buttonSkinData.filebrowserIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.filebrowserIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.filebrowserButtonAnchor, pivot: buttonSkinData.filebrowserButtonPivot);
                break;
            case ButtonType.Settings:
                size = new Vector2(buttonSkinData.settingsIconSize * factor, buttonSkinData.settingsIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.settingsIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);

                rectTransform.sizeDelta = rectSize;
                rectTransform.SetPivot(buttonSkinData.settingsButtonPivot);
                rectTransform.SetAnchor(buttonSkinData.settingsButtonAnchor);
                iconTransform.SetPivot(buttonSkinData.settingsIconPivot);
                iconTransform.SetAnchor(buttonSkinData.settingsIconAnchor);
                //AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.settingsButtonAnchor, pivot: buttonSkinData.settingsButtonPivot);
                break;
            case ButtonType.Swap:
                size = new Vector2(buttonSkinData.swapIconSize * factor, buttonSkinData.swapIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.swapIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.swapButtonAnchor, pivot: buttonSkinData.swapButtonPivot);
                break;
            case ButtonType.Assign:
                size = new Vector2(buttonSkinData.assignIconSize * factor, buttonSkinData.assignIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.assignIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.assignButtonAnchor, pivot: buttonSkinData.assignButtonPivot);
                break;
            case ButtonType.Decline:
                size = new Vector2(buttonSkinData.declineIconSize * factor, buttonSkinData.declineIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.declineIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.declineButtonAnchor, pivot: buttonSkinData.declineButtonPivot);
                break;
            case ButtonType.Share:
                size = new Vector2(buttonSkinData.shareIconSize * factor, buttonSkinData.shareIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.shareIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.shareButtonAnchor, pivot: buttonSkinData.shareButtonPivot);
                break;
            case ButtonType.Delete:
                size = new Vector2(buttonSkinData.deleteIconSize * factor, buttonSkinData.deleteIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.deleteIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.deleteButtonAnchor, pivot: buttonSkinData.deleteButtonPivot);
                break;
            case ButtonType.Dropdown:
                size = new Vector2(buttonSkinData.dropdownIconSize * factor, buttonSkinData.dropdownIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.dropdownIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.dropdownButtonAnchor, pivot: buttonSkinData.dropdownButtonPivot);
                break;
            case ButtonType.Selected:
                size = new Vector2(buttonSkinData.selectedIconSize * factor, buttonSkinData.selectedIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.selectedIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.selectedButtonAnchor, pivot: buttonSkinData.selectedButtonPivot);
                break;

            case ButtonType.Deselected:
                size = new Vector2(buttonSkinData.deselectedIconSize * factor, buttonSkinData.deselectedIconSize * factor);
                SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.deselectedIcon, iconColor: buttonSkinData.iconColor, rectSize: rectSize);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.deselectedButtonAnchor, pivot: buttonSkinData.deselectedButtonPivot);
                break;

            case ButtonType.Record:
                size = new Vector2(buttonSkinData.recordIconSize * factor, buttonSkinData.recordIconSize * factor);
                iconTransform.sizeDelta = size;

                //SetButtonStyle(iconSize: size, iconSprite: buttonSkinData.recordIcon, iconColor: buttonSkinData.iconColor, rectSize: size * buttonSkinData.buttonScaleFactor);
                AssignRectTransform(rectSize: rectSize, anchor: buttonSkinData.recordButtonAnchor, pivot: buttonSkinData.recordButtonPivot);
                break;
        }
    }

    void SetButtonStyle(Vector2 iconSize, Sprite iconSprite, Color iconColor, Vector2 rectSize)
    {
        icon.sprite = iconSprite;
        icon.color = iconColor;
        iconTransform.sizeDelta = iconSize;
    }

    void AssignRectTransform(Vector2 rectSize, AnchorPresets anchor, PivotPresets pivot)
    {
        rectTransform.sizeDelta = rectSize;
        rectTransform.SetPivot(pivot);
        rectTransform.SetAnchor(anchor);

        iconTransform.SetAnchor(anchor);
        iconTransform.SetPivot(pivot);
    }
}
