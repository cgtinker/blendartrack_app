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
        Preview,

        Tutorial,
        CloseTutorial,
        Forward,

        Resume,
        Reset,
        PreviewPop
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
        float factor, m_width;
        bool portrait;
        (factor, m_width, portrait) = ScreenSizeFactor.GetFactor();

        Vector2 rectSize;
        rectSize = new Vector2(buttonSkinData.defaultButtonSize * factor, buttonSkinData.defaultButtonSize * factor);

        //icon display and style
        switch (buttonType)
        {
            case ButtonType.Back:
                SetButtonStyle(buttonSkinData.backButton, factor, rectSize, buttonSkinData.iconColor);
                break;
            case ButtonType.Filebrowser:
                SetButtonStyle(buttonSkinData.filebrowserButton, factor, rectSize, buttonSkinData.iconColor);
                break;
            case ButtonType.Settings:
                SetButtonStyle(buttonSkinData.settingsButton, factor, rectSize, buttonSkinData.iconColor);
                break;
            case ButtonType.Swap:
                SetButtonStyle(buttonSkinData.swapButton, factor, rectSize, buttonSkinData.iconColor);
                break;
            case ButtonType.Share:
                SetButtonStyle(buttonSkinData.shareButton, factor, rectSize, buttonSkinData.iconColor);
                break;
            case ButtonType.Delete:
                SetButtonStyle(buttonSkinData.deleteButton, factor, rectSize, buttonSkinData.iconColor);
                break;
            case ButtonType.Dropdown:
                SetButtonStyle(buttonSkinData.dropdownButton, factor, rectSize, buttonSkinData.iconColor);
                break;
            case ButtonType.Selected:
                SetButtonStyle(buttonSkinData.selectedButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.Deselected:
                SetButtonStyle(buttonSkinData.deselectedButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.Record:
                SetButtonStyle(buttonSkinData.recordingButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.Tutorial:
                SetButtonStyle(buttonSkinData.tutorialIcon, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.CloseTutorial:
                SetButtonStyle(buttonSkinData.closeButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.Forward:
                SetButtonStyle(buttonSkinData.forwardButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.Resume:
                SetButtonStyle(buttonSkinData.resumePopButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.PreviewPop:
                SetButtonStyle(buttonSkinData.previewPopButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.Reset:
                SetButtonStyle(buttonSkinData.resetPopButton, factor, rectSize, buttonSkinData.iconColor);
                break;

            case ButtonType.Preview:
                SetButtonStyle(buttonSkinData.previewButton, factor, rectSize, buttonSkinData.iconColor);
                break;
        }
    }

    void SetButtonStyle(m_ButtonData data, float factor, Vector2 rectSize, Color color)
    {
        Vector2 size = new Vector2(data.iconSize * factor, data.iconSize * factor);

        iconTransform.sizeDelta = size;
        icon.color = color;

        if (data.icon != null)
            icon.sprite = data.icon;

        SetRectTransform(rectSize, data.anchor, data.pivot, data.iconAnchor, data.iconPivot);
    }

    void SetRectTransform(Vector2 rectSize, AnchorPresets anchor, PivotPresets pivot, AnchorPresets iconAnchor, PivotPresets iconPivot)
    {
        rectTransform.sizeDelta = rectSize;
        rectTransform.SetPivot(pivot);
        rectTransform.SetAnchor(anchor);

        iconTransform.SetAnchor(iconAnchor);
        iconTransform.SetPivot(iconPivot);
    }
}
