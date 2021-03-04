using UnityEngine;

[ExecuteInEditMode()]
public class FlexTextOverride : MonoBehaviour
{
	public FlexTextData data;
	public FlexFontAssetData font;

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
