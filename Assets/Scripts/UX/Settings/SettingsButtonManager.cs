using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace ArRetarget
{
	public class SettingsButtonManager : MonoBehaviour
	{
		#region refs
		public GameObject SettingsButtonPrefab;
		public GameObject videoSettingsHeader;

		public Transform SettingsObjParent;
		private List<SettingButtonData> recordingSettings = new List<SettingButtonData>();
		#endregion

		private void Start()
		{
			GenerateSettingsButtons();
		}

		public void GenerateSettingsButtons()
		{
			//video settings generated at runtime (device dependant)
			if (PlayerPrefsHandler.Instance.CameraConfigList.Count > 0)
			{
				//videoSettigsTitle = GenerateSettingsTitel("Video Settings");
				GenerateRecordingSettingsButtons();
			}
		}


		#region ui generation
		//custom toggle group
		public void OnToggleXRCameraSetting(string name)
		{
			foreach (SettingButtonData button in recordingSettings)
			{
				if (button.userPrefsName != name)
				{
					var script = button.obj.GetComponent<UserSettingsButton>();
					script.ChangeSelectionToggleStatus(false);
					script.SetUserPreference(button.userPrefsName, false);
					script.btnIsOn = false;
				}
			}
		}

		//can be different for face / cam (face recording currently not possible)
		public void GenerateRecordingSettingsButtons()
		{
			//same settings available
			if (PlayerPrefsHandler.Instance.CameraConfigList.Count == recordingSettings.Count)
			{
				return;
			}

			else
			{
				//removing current
				foreach (SettingButtonData data in recordingSettings)
				{
					Destroy(data.obj);
				}
				recordingSettings.Clear();

				//referencing the available configs
				for (int i = 0; i < PlayerPrefsHandler.Instance.CameraConfigList.Count; i++)
				{
					SettingButtonData tmp = new SettingButtonData()
					{
						displayName = PlayerPrefsHandler.Instance.CameraConfigList[i],
						userPrefsName = PlayerPrefsHandler.Instance.CameraConfigList[i]
					};

					recordingSettings.Add(tmp);
				}

				//generating the buttons
				GenerateButtons(recordingSettings, true);
			}
		}

		#region ui gen
		private void GenerateButtons(List<SettingButtonData> buttons, bool isToggleGroup)
		{
			foreach (SettingButtonData button in buttons)
			{
				//if button is already instantiate
				if (button.obj)
					return;

				//generate button
				var tmp = Instantiate(SettingsButtonPrefab, Vector3.zero, Quaternion.identity);
				button.obj = tmp;
				var script = tmp.GetComponent<UserSettingsButton>();
				script.Init(button.displayName, button.userPrefsName, isToggleGroup, this.gameObject.GetComponent<SettingsButtonManager>());
				tmp.transform.SetParent(SettingsObjParent);

				//sappling index for video ui generation
				int parentIndex = videoSettingsHeader.transform.GetSiblingIndex();
				tmp.transform.SetSiblingIndex(parentIndex + 1);

				tmp.transform.localScale = Vector3.one;
			}
		}
		#endregion
		#endregion
	}

	[System.Serializable]
	public class SettingButtonData
	{
		public string displayName;
		public string userPrefsName;
		public GameObject obj;
		public bool status;
	}
}