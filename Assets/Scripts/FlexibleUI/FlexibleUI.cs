using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//doesnt aquire to load the scene
[ExecuteInEditMode()]
public class FlexibleUI : MonoBehaviour
{
    public FlexibleUIData skinData;

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
