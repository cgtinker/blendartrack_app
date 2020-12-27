using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class FlexibleUITextButtonOverride : MonoBehaviour
{
    public FlexibleUITextButtonData textButtonSkinData;

    protected virtual void OnSkinUI()
    {

    }

    //adjusting UI data on awake
    public virtual void Awake()
    {
        OnSkinUI();
    }

    //should be an editor script, TODO: Remove on realease!
    public virtual void Update()
    {
        if (Application.isEditor)
        {
            OnSkinUI();
        }
    }
}
