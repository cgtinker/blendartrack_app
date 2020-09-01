using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeviceManager : Singleton<DeviceManager>
{
    public enum Device
    {
        Remote,
        iOs,
        Android
    };

    public Device device
    {
        get;
        set;
    }

    public enum RecData
    {
        ArCore_CameraPose,
        ArKit_CameraPose,
        ArCore_FaceMesh,
        ArKit_ShapeKeys,
        Remote_CameraPose,
        Remote_FaceMesh,
        Remote_FaceKeys
    };

    public RecData DataType
    {
        get;
        set;
    }


    public void SetDataType(RecData data)
    {
        DataType = data;
        Debug.Log("Assigned Data Type");
    }


    // Start is called before the first frame update
    void Awake()
    {
#if UNITY_EDITOR
        device = Device.Remote;
#endif
#if UNITY_IPHONE
        device = Device.iOs;
#endif
#if UNITY_ANDROID
        device = Device.Android;
#endif
    }
}
