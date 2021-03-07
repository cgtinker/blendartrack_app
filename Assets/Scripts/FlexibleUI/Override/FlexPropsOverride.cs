using UnityEngine;

[ExecuteInEditMode()]
public class FlexPropsOverride : MonoBehaviour
{
	public FlexLayoutPropsData data;

	protected virtual void OnSkinUI()
	{

	}

	//adjusting UI data on awake
	public virtual void Awake()
	{
		OnSkinUI();
	}

	public virtual void UpdateUI()
	{
		OnSkinUI();
	}
}
