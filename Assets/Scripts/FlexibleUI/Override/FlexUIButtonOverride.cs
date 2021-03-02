using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexUIButtonOverride : MonoBehaviour
{
	public FlexButtonData data;

	protected virtual void OnSkinUI()
	{

	}

	//adjusting UI data on awake
	public virtual void Awake()
	{
		OnSkinUI();
	}
}
