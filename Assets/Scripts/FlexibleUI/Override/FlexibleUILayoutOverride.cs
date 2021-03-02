using UnityEngine;
using UnityEditor;

[ExecuteInEditMode()]
public class FlexibleUILayoutOverride : MonoBehaviour
{
	public FlexibleUILayoutData layoutSkinData;

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
