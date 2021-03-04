using UnityEngine;

[ExecuteInEditMode()]
public class FlexPaddingOverride : MonoBehaviour
{
	public FlexPaddingData data;

	protected virtual void OnSkinUI()
	{

	}

	//adjusting UI data on awake
	public virtual void OnEnable()
	{
		OnSkinUI();
	}

	public void UpdateUI()
	{
		OnSkinUI();
	}
}
