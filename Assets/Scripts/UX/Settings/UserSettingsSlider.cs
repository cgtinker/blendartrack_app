using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace ArRetarget
{
	public class UserSettingsSlider : MonoBehaviour
	{
		[Header("User Settings")]
		[SerializeField]
		private string settingsName;
		[SerializeField]
		private float defaultValue;
		[SerializeField]
		private float factor;

		[Header("Ref")]
		[SerializeField]
		private Slider slider;

		[Header("Display")]
		[SerializeField]
		private TextMeshProUGUI displayText;
		[SerializeField]
		private string prefix;
		[SerializeField]
		private string suffix;

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();

			float current = GetUserPreference(settingsName);
			ChangeDisplayValue(current);
			slider.value = Mathf.FloorToInt(current);
		}

		public void OnValueChanged()
		{
			ChangeDisplayValue(slider.value);
		}

		public void OnPointerUp()
		{
			Debug.Log("done changi");
			SetUserPreference(settingsName, slider.value);
		}

		private void ChangeDisplayValue(float value)
		{
			int m_value = Mathf.FloorToInt(value);
			string m_text = $"{prefix}{m_value}{suffix}";
			displayText.text = m_text;
		}

		private float GetUserPreference(string key)
		{
			float m_val = PlayerPrefsHandler.Instance.GetFloat(key, defaultValue);
			return m_val * factor;
		}

		private void SetUserPreference(string key, float value)
		{
			PlayerPrefsHandler.Instance.SetFloat(key, value / factor);
		}
	}
}
