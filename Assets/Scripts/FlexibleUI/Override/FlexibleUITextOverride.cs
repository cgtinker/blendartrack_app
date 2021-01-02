using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class FlexibleUITextOverride : MonoBehaviour
{
    public FlexibleUITextData textSkinData;

    protected virtual void OnSkinUI()
    {

    }

    //adjusting UI data on awake
    public virtual void Awake()
    {
        OnSkinUI();
    }
    public virtual void Update()
    {
        OnSkinUI();
    }
}
