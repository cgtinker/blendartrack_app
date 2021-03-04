using UnityEngine;

[ExecuteInEditMode()]
public class FlexibleUILayoutOverride : MonoBehaviour
{
	public FlexibleUILayoutData layoutSkinData;

	protected virtual void OnSkinUI()
	{

	}

	//adjusting UI data on awake
	public virtual void Awake()
	{
		OnSkinUI();
	}
}
