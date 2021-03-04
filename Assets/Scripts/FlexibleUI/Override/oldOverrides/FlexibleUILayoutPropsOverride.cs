using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class FlexibleUILayoutPropsOverride : MonoBehaviour
{
    public FlexibleUILayoutPropsData layoutPropsSkinData;

    protected virtual void OnSkinUI()
    {

    }

    //adjusting UI data on awake
    public virtual void OnEnable()
    {
        OnSkinUI();
    }
}
