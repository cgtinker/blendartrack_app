using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Flexible UI Data")]
public class FlexibleUIData : ScriptableObject
{
    [Header("Button Background")]
    public Sprite buttonBackground;
    public Color buttonBackgroundColor;


    [Header("Default Buttons")]
    public Sprite backIcon;
    public Color iconColor;
    public float backButtonSize;

    [Header("Special Buttons")]
    public Sprite swapIcon;
    public float swapIconSize;

    public Sprite filebrowserIcon;
    public float filebrowserIconSize;

    public Sprite menuIcon;
    public float menuIconSize;

    public Sprite recordIcon;
    public float recordIconSize;

    public Sprite settingsIcon;
    public float settingsIconSize;
}
