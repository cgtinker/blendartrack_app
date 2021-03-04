using UnityEngine;

[ExecuteInEditMode()]
public class FlexButtonOverride : MonoBehaviour
{
	public FlexButtonData data;
	public FlexDefaultButtonData defaultData;

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
