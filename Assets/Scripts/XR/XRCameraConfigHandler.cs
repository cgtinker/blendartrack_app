using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

namespace ArRetarget
{
	[RequireComponent(typeof(ARCameraManager))]
	public class XRCameraConfigHandler : MonoBehaviour
	{
		private ARCameraManager cameraManager = null;
		bool managerReceivedFrame;
		public XRCameraConfiguration activeXRCameraConfig;

		//checks and may changes camera config on frame received
		void OnEnable()
		{
			if (!cameraManager)
				cameraManager = this.gameObject.GetComponent<ARCameraManager>();

			cameraManager.frameReceived += FrameReceived;
		}

		void OnDisable()
		{
			cameraManager.frameReceived -= FrameReceived;
		}

		//getting a list of strings based on the available camera configs
		public List<string> GetAvailableConfiguartionStrings()
		{
			List<string> availableConfigs = new List<string>();

			if (cameraManager.descriptor.supportsCameraConfigurations)
			{
				using (var configs = cameraManager.GetConfigurations(Allocator.Temp))
				{
					for (int i = 0; i < configs.Length; i++)
					{
						availableConfigs.Add(CameraConfigString(configs[i]));
					}
				}
			}

			return availableConfigs;
		}

		public string CameraConfigString(XRCameraConfiguration config)
		{
			Vector2Int resolution = config.resolution;
			int? framerate = config.framerate;

			string config_str = $"{resolution[0]}x{resolution[1]} at {framerate} fps";
			return config_str;
		}

		//changing config based on player prefs (called on enable after frame received)
		public void ChangeConfig()
		{
			if (cameraManager.descriptor.supportsCameraConfigurations)
			{
				using (var configs = cameraManager.GetConfigurations(Allocator.Temp))
				{
					for (int i = 0; i < configs.Length; i++)
					{
						if (isPreferredConfig(CameraConfigString(configs[i])))
						{
							try
							{
								cameraManager.currentConfiguration = configs[i];
								activeXRCameraConfig = configs[i];
								break;
							}
							catch (Exception e)
							{
								Debug.LogError("setting cameraManager.currentConfiguration failed with exception: " + e);
							}
						}
					}
				}
			}
		}

		//preferd config stored in player prefs
		static bool isPreferredConfig(string config)
		{
			bool m_bool = false;

			for (int i = 0; i < PlayerPrefsHandler.Instance.CameraConfigList.Count; i++)
			{
				if (PlayerPrefsHandler.Instance.GetInt(PlayerPrefsHandler.Instance.CameraConfigList[i], -1) == 1)
				{
					if (PlayerPrefsHandler.Instance.CameraConfigList[i] == config)
					{
						m_bool = true;
					}
				}
			}
			return m_bool;
		}

		//checks config and changes it if necessary
		private void FrameReceived(ARCameraFrameEventArgs args)
		{
			if (!managerReceivedFrame)
			{
				managerReceivedFrame = true;
				if (cameraManager.descriptor.supportsCameraConfigurations)
				{
					//gets current config
					var cameraConfiguration = cameraManager.currentConfiguration;
					Assert.IsTrue(cameraConfiguration.HasValue);
					//current config has value
					activeXRCameraConfig = (XRCameraConfiguration)cameraConfiguration;
					Debug.Log($"Current Config: {CameraConfigString(activeXRCameraConfig)}");

					//referencing available configs when frame received in user prefs dict
					var availableConfigs = GetAvailableConfiguartionStrings();
					PlayerPrefsHandler.Instance.ReferenceAvailableXRCameraConfigs(availableConfigs);

					//keep current config if it's the previously set one
					if (PlayerPrefsHandler.Instance.GetInt(CameraConfigString(activeXRCameraConfig), -1) == 1)
					{
						Debug.Log("Current config has been the previously stored one");
						PlayerPrefsHandler.Instance.SetDefaultXRCameraConfig(
							CameraConfigString(activeXRCameraConfig));
						return;
					}

					//if the setting is not the stored one
					bool storedValue = false;
					//check if there is a stored setting and if change the xr camera settings 
					for (int i = 0; i < availableConfigs.Count; i++) // (string config in availableConfigs)
					{
						if (PlayerPrefsHandler.Instance.GetInt(availableConfigs[i], -1) == 1)
						{
							Debug.Log($"Changing config to {availableConfigs[i]}");
							storedValue = true;
							PlayerPrefsHandler.Instance.SetDefaultXRCameraConfig(availableConfigs[i]);
							ChangeConfig();
						}
					}

					//storing the default setting
					if (storedValue == false)
					{
						if (availableConfigs.Count <= 1)
						{
							Debug.Log("Aborted reset: Available Configs == 1");
							return;
						}

						Debug.Log("No previous config has been stored");
						PlayerPrefsHandler.Instance.SetDefaultXRCameraConfig(
							CameraConfigString(activeXRCameraConfig));
					}
				}
			}
		}
	}

}
