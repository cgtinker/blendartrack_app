using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DeviceManager : Singleton<DeviceManager>
{
	//depending on the device, different scenes will be available
	#region Device Management
	public enum Device
	{
		iOS,
		Android,
		iOSX,
		UnityEditor
	};

	public Device device
	{
		get;
		private set;
	}

	//setting device
	private void Awake()
	{
#if UNITY_IOS
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			bool lowerThanIphoneX =
				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6 ||
				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6Plus ||
				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6S ||
				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone6SPlus ||

				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone7 ||
				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone7Plus ||

				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone8 ||
				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhone8Plus ||

				UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneSE1Gen
				;

			if (!lowerThanIphoneX)
			{
				device = Device.iOSX;
				Debug.Log("Runtime Platform: " + device);
				return;
			}

			else
			{
				device = Device.iOS;
				Debug.Log("Runtime Platform: " + device);
				return;
			}
		}
#endif
#if UNITY_ANDROID
		if (Application.platform == RuntimePlatform.Android)
		{
			device = Device.Android;
			Debug.Log("Runtime Platform: " + device);
			return;
		}
#endif
		else
		{
			device = Device.UnityEditor;
			Debug.Log("Runtime Platform Unity Editor: " + device);
			return;
		}
	}
	#endregion
}
