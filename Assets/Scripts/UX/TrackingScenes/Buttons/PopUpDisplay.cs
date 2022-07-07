using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PopUpDisplay : MonoBehaviour
{
	public Button button;
	public TextMeshProUGUI tmp_text;
	public RectTransform rectTransform;
	public GameObject desitionation;

	public string text;
	public float staticDuration;
	public float travelDuration;

	public enum PopupType
	{
		Static,
		Notification,
		ButtonEvent
	}

	public PopupType type;

	public void DisplayPopup(Transform parent)
	{
		tmp_text.text = text;
		SetTransform(parent);

		switch (type)
		{
			case PopupType.Static:
			Debug.Log("Static Popup");
			break;

			case PopupType.Notification:
			StartCoroutine(DisablePopupTimer(staticDuration));
			break;

			case PopupType.ButtonEvent:
			break;
		}
	}

	private IEnumerator DisablePopupTimer(float duration)
	{
		yield return new WaitForSeconds(duration);
		Destroy(this.gameObject);
	}

	private void SetTransform(Transform parent)
	{
		this.transform.SetParent(parent);
		this.transform.localScale = Vector3.one;
	}

	public float currentTravelTime = 0; // actual floating time 
}
