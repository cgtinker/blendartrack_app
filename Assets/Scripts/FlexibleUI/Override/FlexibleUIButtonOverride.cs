using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//doesnt aquire to load the scene
[ExecuteInEditMode()]
public class FlexibleUIButtonOverride : MonoBehaviour
{
    public FlexibleUIButtonData buttonSkinData;

    protected virtual void OnSkinUI()
    {

    }

    //adjusting UI data on awake
    public virtual void Awake()
    {
        OnSkinUI();
    }
}
