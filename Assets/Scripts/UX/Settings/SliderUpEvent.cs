using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ArRetarget
{
	public class SliderUpEvent : MonoBehaviour, IPointerUpHandler
	{
		[SerializeField]
		private UserSettingsSlider settingsSlider;

		public void OnPointerUp(PointerEventData eventData)
		{
			settingsSlider.OnPointerUp();
		}
	}
}
