using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// player prefs handler to set defaults and reset values
/// </summary>
namespace ArRetarget
{

	public class PlayerPrefsHandler : Singleton<PlayerPrefsHandler>
	{
		#region Reference dictionarys for default values
		//public Dictionary<string, int> CameraConfigDict = new Dictionary<string, int>();
		public List<string> CameraConfigList = new List<string>();
		#endregion

		#region XRCamera Prefs
		//for referencing settings in face mode
		private void Awake()
		{
			if (CameraConfigList.Count == 0)
			{
				LoadPlayerPrefCameraConfigs();
			}
		}

		void LoadPlayerPrefCameraConfigs()
		{
			for (int i = 0; i < 6; i++)
			{
				var m_config = PlayerPrefs.GetString($"cameraConfig_{i}", "");

				if (m_config != "")
				{
					CameraConfigList.Add(m_config);
					print(m_config);
				}
			}
		}

		//referencing available camera settings
		public void ReferenceAvailableXRCameraConfigs(List<string> availableConfigs)
		{
			//settings can differ depending on front / back camera
			//only setting front camera settings as recording on android wasnt possible
			if (availableConfigs.Count != CameraConfigList.Count)
			{
				print("initializing available user camera config preferences");
				CameraConfigList.Clear();
				for (int i = 0; i < availableConfigs.Count; i++)
				{
					if (!CameraConfigList.Contains(availableConfigs[i]))
					{
						//reference in player prefs
						string configTitle = $"cameraConfig_{i}";
						PlayerPrefs.SetString(configTitle, availableConfigs[i]);
						//add to list
						CameraConfigList.Add(availableConfigs[i]);
					}
				}
			}
		}

		public void SetDefaultXRCameraConfig(string config)
		{
			print("setting default camera config value");
			for (int i = 0; i < CameraConfigList.Count; i++)
			{
				if (CameraConfigList[i] != config)
				{
					PlayerPrefs.SetInt(CameraConfigList[i], -1);
				}
				else
				{
					PlayerPrefs.SetInt(CameraConfigList[i], 1);
				}
			}
		}
		#endregion

		#region Values
		public void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}

		public void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}

		public void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}

		public float GetFloat(string key, float defaultValue)
		{
			float m_value = PlayerPrefs.GetFloat(key: key, defaultValue: defaultValue);
			return m_value;
		}

		public int GetInt(string key, int defaultValue)
		{
			int m_int = PlayerPrefs.GetInt(key: key, defaultValue: defaultValue);
			return m_int;
		}

		public string GetString(string key, string defaultValue)
		{
			string m_string = PlayerPrefs.GetString(key, defaultValue: defaultValue);
			return m_string;
		}
		#endregion

		#region first time user prefs
		public void SetFirstTimeUserPrefs(int firstTimeUserKey)
		{
			print("Set first time user player preferences");
			PlayerPrefs.DeleteAll();

			PlayerPrefs.SetInt("firstTimeUser", firstTimeUserKey);
			PlayerPrefs.SetInt("default", 1);

			PlayerPrefs.SetString("scene", "Camera Tracker");
			PlayerPrefs.SetInt("tutorial", 1);

			PlayerPrefs.SetInt("hints", 1);
			PlayerPrefs.SetInt("recordCam", 1);
			PlayerPrefs.SetInt("autofocus", 1);

			PlayerPrefs.SetFloat("pointDensity", 0.05f);
			PlayerPrefs.SetInt("bitrate", 8);
			PlayerPrefs.SetInt("vidzip", 1);
		}
		#endregion
		private void SavePreferences()
		{
			PlayerPrefs.Save();
		}
	}

}