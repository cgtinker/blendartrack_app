using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

namespace ArRetarget
{
	public class UserSettingsButton : MonoBehaviour
	{
		//name in user settings
		public string settingsName;
		public TextMeshProUGUI displayText;
		SettingsButtonManager settingsButtonManager;

		//button status
		public bool btnIsOn = false;
		private bool isToggleGroup = false;

		//visual representation
		public GameObject selected;
		public GameObject deselected;

		/// <summary>
		/// function to initialize a user settings button
		/// </summary>
		/// <param name="displayText"></param> text for the settings button
		/// <param name="settingsName"></param> name of the user pref
		public void Init(string displayText, string settingsName, bool toggleGroup, SettingsButtonManager settingsButtonManager)
		{
			this.displayText.text = displayText;
			this.settingsName = settingsName;
			this.isToggleGroup = toggleGroup;
			this.settingsButtonManager = settingsButtonManager;


		}

		private IEnumerator Start()
		{
			yield return new WaitForEndOfFrame();

			btnIsOn = GetUserPreference(settingsName);
			ChangeSelectionToggleStatus(btnIsOn);
		}

		//if button gets pressed change status
		public void OnToggleButton()
		{
			//for toggle groups just for xr settings atm
			if (isToggleGroup)
			{
				if (btnIsOn)
				{
					return;
				}

				settingsButtonManager.OnToggleXRCameraSetting(settingsName);
			}

			btnIsOn = !btnIsOn;
			SetUserPreference(settingsName, btnIsOn);
			ChangeSelectionToggleStatus(btnIsOn);
			Debug.Log(settingsName + btnIsOn);
		}

		//changing visual for selected/ deselected
		public void ChangeSelectionToggleStatus(bool status)
		{
			selected.SetActive(status);
			deselected.SetActive(!status);
		}

		#region set and get user prefs
		public bool GetUserPreference(string key)
		{
			int m_val = PlayerPrefsHandler.Instance.GetInt(key, -1);
			if (m_val == 1)
				return true;

			else
				return false;
		}

		public void SetUserPreference(string prefName, bool status)
		{
			if (status == true)
			{
				PlayerPrefsHandler.Instance.SetInt(prefName, 1);
			}

			else
			{
				PlayerPrefsHandler.Instance.SetInt(prefName, -1);
			}
		}
		#endregion
	}
}
