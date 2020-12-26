using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIButtonData : ScriptableObject
{
    [Header("Button Background")]
    public Sprite buttonBackground;
    public Color buttonBackgroundColor;
    public float buttonScaleFactor;


    [Header("Default Buttons")]
    public Color iconColor;

    [Header("Back Button")]
    public Sprite backIcon;
    public float backButtonSize;
    public int backMinPxSize;
    public int backMaxPxSize;

    [Header("Delete Button")]
    public Sprite deleteIcon;
    public float deleteButtonSize;
    public int deleteMinPxSize;
    public int deleteMaxPxSize;

    [Header("Share Button")]
    public Sprite shareIcon;
    public float shareButtonSize;
    public int shareMinPixPxSize;
    public int shareMaxPxSize;

    [Header("Swap Button")]
    public Sprite swapIcon;
    public float swapIconSize;
    public int swapMinPxSize;
    public int swapMaxPxSize;

    [Header("Record Button")]
    public Sprite dropdownIcon;
    public float dropdownButtonSize;
    public int dropdownMixPxSize;
    public int dropdownMaxPxSize;

    [Header("Filebrowser Button")]
    public Sprite filebrowserIcon;
    public float filebrowserIconSize;
    public int filebrowserMinPxSize;
    public int filebrowserMaxPxSize;

    [Header("Settings Button")]
    public Sprite settingsIcon;
    public float settingsIconSize;
    public int settingsMinPxSize;
    public int settingsMaxPxSize;

    [Header("Assign Button")]
    public Sprite assignIcon;
    public float assignIconSize;
    public int assignMinPxSize;
    public int assignMaxPxSize;

    [Header("Decline Button")]
    public Sprite declineIcon;
    public float declineIconSize;
    public int declineMinPxSize;
    public int declineMaxPxSize;

    [Header("Selected Button")]
    public Sprite selectedIcon;
    public float selectedButtonSize;
    public int selectedMinPxSize;
    public int selectedMaxPxSize;

    [Header("Deselected Button")]
    public Sprite deselectedIcon;
    public float deselectedButtonSize;
    public int deselectedMinPxSize;
    public int deselectedMaxPxSize;

    [Header("Record Button")]
    public Sprite recordIcon;
    public Sprite breakIcon;
    public float recordButtonSize;
    public int recordMinPxSize;
    public int recordMaxPxSize;
}
