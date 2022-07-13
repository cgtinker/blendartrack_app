using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

namespace ArRetarget
{
	public class XRCameraConfigHandler : MonoBehaviour
	{
		[SerializeField]
		private ARCameraManager cameraManager;
		private List<string> availableConfigs = new List<string>();
		int counter = 0;

		public void SetCameraConfig()
		{
			counter = 0;
			Debug.Log("Attempt to set camera configuration");
			cameraManager.frameReceived += FrameReceived;
		}

		void FrameReceived(ARCameraFrameEventArgs args)
		{
			if (counter > 3)
				cameraManager.frameReceived -= FrameReceived;
			else
				counter++;

			// Using the ARCameraManager to obtain the camera configurations.
			using (NativeArray<XRCameraConfiguration> configurations = cameraManager.GetConfigurations(Allocator.Temp))
			{
				if (!configurations.IsCreated || (configurations.Length <= 0))
				{
					Debug.LogWarning($"No configurations found { configurations.Length}");
					return;
				}

				// Iterate through the list of returned configs to locate the desired configuration.
				// Using 640x480 VGA configuration as default.
				var desiredConfig = configurations[0];
				if (availableConfigs.Count != configurations.Length)
					availableConfigs.Clear();
				// TODO: stop iterating if i > len(configs)
				for (int i = 0; i < configurations.Length; ++i)
				{
					if (availableConfigs.Count != configurations.Length)
						availableConfigs.Add(CameraConfigToString(configurations[i]));
					if (IsDesiredConfig(configurations[i]))
						desiredConfig = configurations[i];
				}

				// Add available configrations to PlayerPrefs
				PlayerPrefsHandler.Instance.ReferenceAvailableXRCameraConfigs(availableConfigs);

				// Unsubribe from the ARCameraManager frameReceived event if the
				// desired confiration has been applied to the ARCameraManager
				XRCameraConfiguration? currentConfig = cameraManager.currentConfiguration;
				Assert.IsTrue(currentConfig.HasValue);

				// Check if active configuration ma tches the use rs desired configuration
				if (IsDesiredConfig((XRCameraConfiguration)currentConfig))
				{
					Debug.Log($"Desired matches current {currentConfig}");
					cameraManager.frameReceived -= FrameReceived;
				}
				// Setting the desired configuration. If it succeds, the session automatically
				// pauses and resumes to apply the new configuration.
				else
				{
					Debug.Log($"Try setting current to desired {desiredConfig}");
					cameraManager.currentConfiguration = desiredConfig;
				}
			}
		}

		private bool IsDesiredConfig(XRCameraConfiguration _config)
		{
			var config = CameraConfigToString(_config);
			if (PlayerPrefsHandler.Instance.GetInt(config, -1) == 1)
				return true;
			return false;
		}

		private string CameraConfigToString(XRCameraConfiguration config)
		{
			Vector2Int resolution = config.resolution;
			int? framerate = config.framerate;

			string config_str = $"{resolution[0]}x{resolution[1]} at {framerate} fps";
			return config_str;
		}
	}
}
