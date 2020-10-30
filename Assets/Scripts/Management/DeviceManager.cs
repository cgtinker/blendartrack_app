using UnityEngine;

/// <summary>
/// The device manager helps to differentiate between the running device and their augemented reality capabilities
/// </summary>
public class DeviceManager : Singleton<DeviceManager>
{
    public enum Device
    {
        Remote,
        iOS,
        Android
    };

    public Device device
    {
        get;
        set;
    }

    public enum TrackingType
    {
        ArCore_CameraPose,
        ArKit_CameraPose,

        ArCore_FaceMesh,
        ArKit_BlendShapes,

        //implemented for futher tests? Might be cutted.
        Remote_CameraPose,
        Remote_FaceMesh,
        Remote_FaceKeys,

        MainMenu
    };

    public TrackingType Ability
    {
        get;
        set;
    }


    public void SetDataType(TrackingType ability)
    {
        Ability = ability;
        Debug.Log($"Assigned Data Type: {ability}");
    }


    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        device = Device.Remote;
#endif
#if UNITY_IPHONE
            device = Device.iOS;
#endif
#if UNITY_ANDROID
        device = Device.Android;
#endif
        Debug.Log($"Device: {device}");
    }
}