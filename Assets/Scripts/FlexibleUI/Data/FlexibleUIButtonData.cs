﻿using System.Collections;
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


    [Header("Default Button Data")]
    public Color iconColor;
    public int defaultButtonSize;

    [Header("Button Presets")]
    public m_ButtonData backButton;
    public m_ButtonData deleteButton;
    public m_ButtonData shareButton;
    public m_ButtonData swapButton;
    public m_ButtonData dropdownButton;
    public m_ButtonData filebrowserButton;
    public m_ButtonData settingsButton;
    public m_ButtonData selectedButton;
    public m_ButtonData deselectedButton;
    public m_ButtonData recordingButton;
}

[System.Serializable]
public class m_ButtonData
{
    public Sprite icon;
    [Space(20)]
    public float iconSize;
    public int minPxSize;
    public int maxPxSize;
    [Space(20)]
    public AnchorPresets anchor;
    public PivotPresets pivot;
    [Space(20)]
    public AnchorPresets iconAnchor;
    public PivotPresets iconPivot;
}