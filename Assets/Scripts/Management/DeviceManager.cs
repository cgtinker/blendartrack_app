using UnityEngine;
using System.Collections;

public class DeviceManager : Singleton<DeviceManager>
{
    //depending on the device, different scenes will be available
    #region Device Management
    public enum Device
    {
        iOS,
        Android,
        iOSX
    };

    public Device device
    {
        get;
        set;
    }

    //setting device
    private void Awake()
    {
#if Unity_IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            bool deviceIsIphoneX = UnityEngine.iOS.Device.generation == UnityEngine.iOS.DeviceGeneration.iPhoneX;
            if (deviceIsIphoneX)
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
            device = Device.iOS;
            Debug.Log("Runtime Platform Unity Editor: " + device);
            return;
        }
    }
    #endregion
}
