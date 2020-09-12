using UnityEngine;

namespace ArRetarget
{
    /// <summary>
    /// The device manager helps to differentiate between the running device and their augemented reality capabilities
    /// </summary>
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

        public enum Capabilities
        {
            ArCore_CameraPose,
            ArKit_CameraPose,

            ArCore_FaceMesh,
            ArKit_ShapeKeys,

            //implemented for futher tests
            Remote_CameraPose,
            Remote_FaceMesh,
            Remote_FaceKeys,

            MainMenu
        };

        public Capabilities Ability
        {
            get;
            set;
        }


        public void SetDataType(Capabilities ability)
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
        device = Device.iOs;
#endif
#if UNITY_ANDROID
            device = Device.Android;
#endif
            Debug.Log($"Device: {device}");
        }
    }
}