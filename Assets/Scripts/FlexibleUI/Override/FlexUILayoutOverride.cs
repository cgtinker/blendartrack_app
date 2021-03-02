using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexUILayoutOverride : MonoBehaviour
{
	public FlexLayoutData data;

	protected virtual void OnSkinUI()
	{
		Debug.Log("Updateing Layout");
	}

	//adjusting UI data on awake
	public virtual void Awake()
	{
		OnSkinUI();
	}
}
