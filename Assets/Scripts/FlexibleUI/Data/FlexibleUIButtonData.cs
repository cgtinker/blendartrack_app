using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "FlexibleUI/Flexible UI Data")]
public class FlexibleUIButtonData : ScriptableObject
{
    [Header("Button Background")]
    public Sprite buttonBackground;
    public Color buttonBackgroundColor;
    public float buttonScaleFactor;


    [Header("Default Buttons")]
    public Color iconColor;
    public int defaultButtonSize;
    public int minDefaultButtonSize;
    public int maxDefaultButtonSize;

    [Header("Back Button")]
    public Sprite backIcon;
    public float backIconSize;
    public int backMinPxSize;
    public int backMaxPxSize;
    public AnchorPresets backButtonAnchor;
    public PivotPresets backButtonPivot;

    [Header("Delete Button")]
    public Sprite deleteIcon;
    public float deleteIconSize;
    public int deleteMinPxSize;
    public int deleteMaxPxSize;
    public AnchorPresets deleteButtonAnchor;
    public PivotPresets deleteButtonPivot;

    [Header("Share Button")]
    public Sprite shareIcon;
    public float shareIconSize;
    public int shareMinPixPxSize;
    public int shareMaxPxSize;
    public AnchorPresets shareButtonAnchor;
    public PivotPresets shareButtonPivot;

    [Header("Swap Button")]
    public Sprite swapIcon;
    public float swapIconSize;
    public int swapMinPxSize;
    public int swapMaxPxSize;
    public AnchorPresets swapButtonAnchor;
    public PivotPresets swapButtonPivot;

    [Header("Dropdown Button")]
    public Sprite dropdownIcon;
    public float dropdownIconSize;
    public int dropdownMixPxSize;
    public int dropdownMaxPxSize;
    public AnchorPresets dropdownButtonAnchor;
    public PivotPresets dropdownButtonPivot;

    [Header("Filebrowser Button")]
    public Sprite filebrowserIcon;
    public float filebrowserIconSize;
    public int filebrowserMinPxSize;
    public int filebrowserMaxPxSize;
    public AnchorPresets filebrowserButtonAnchor;
    public PivotPresets filebrowserButtonPivot;

    [Header("Settings Button")]
    public Sprite settingsIcon;
    public float settingsIconSize;
    public int settingsMinPxSize;
    public int settingsMaxPxSize;
    public AnchorPresets settingsButtonAnchor;
    public PivotPresets settingsButtonPivot;
    public AnchorPresets settingsIconAnchor;
    public PivotPresets settingsIconPivot;

    [Header("Assign Button")]
    public Sprite assignIcon;
    public float assignIconSize;
    public int assignMinPxSize;
    public int assignMaxPxSize;
    public AnchorPresets assignButtonAnchor;
    public PivotPresets assignButtonPivot;

    [Header("Decline Button")]
    public Sprite declineIcon;
    public float declineIconSize;
    public int declineMinPxSize;
    public int declineMaxPxSize;
    public AnchorPresets declineButtonAnchor;
    public PivotPresets declineButtonPivot;

    [Header("Selected Button")]
    public Sprite selectedIcon;
    public float selectedIconSize;
    public int selectedMinPxSize;
    public int selectedMaxPxSize;
    public AnchorPresets selectedButtonAnchor;
    public PivotPresets selectedButtonPivot;

    [Header("Deselected Button")]
    public Sprite deselectedIcon;
    public float deselectedIconSize;
    public int deselectedMinPxSize;
    public int deselectedMaxPxSize;
    public AnchorPresets deselectedButtonAnchor;
    public PivotPresets deselectedButtonPivot;

    [Header("Record Button")]
    public Sprite recordIcon;
    public Sprite breakIcon;
    public float recordIconSize;
    public int recordMinPxSize;
    public int recordMaxPxSize;
    public AnchorPresets recordButtonAnchor;
    public PivotPresets recordButtonPivot;
}
