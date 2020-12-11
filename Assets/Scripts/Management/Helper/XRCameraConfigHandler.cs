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
        [SerializeField] ARCameraManager cameraManager = null;
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
                        availableConfigs.Add(configs[i].ToString());
                    }
                }
            }

            return availableConfigs;
        }

        //changing config based on player prefs
        public void ChangeConfig()
        {
            if (cameraManager.descriptor.supportsCameraConfigurations)
            {
                using (var configs = cameraManager.GetConfigurations(Allocator.Temp))
                {
                    for (int i = 0; i < configs.Length; i++)
                    {
                        if (isPreferredConfig(configs[i]))
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
        static bool isPreferredConfig(XRCameraConfiguration config)
        {
            bool m_bool = false;

            for (int i = 0; i < UserPreferences.Instance.CameraConfigList.Count; i++)
            {
                if (PlayerPrefs.GetInt(UserPreferences.Instance.CameraConfigList[i], -1) == 1)
                {
                    if (UserPreferences.Instance.CameraConfigList[i] == config.ToString())
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
                    Debug.Log($"Current Config: {cameraConfiguration}");
                    //assigning the current config
                    activeXRCameraConfig = (XRCameraConfiguration)cameraConfiguration;

                    //referencing available configs when frame received in user prefs dict
                    var availableConfigs = GetAvailableConfiguartionStrings();
                    UserPreferences.Instance.ReferenceAvailableXRCameraConfigs(availableConfigs);

                    //keep current config if it's the previously set one
                    if (PlayerPrefs.GetInt(cameraConfiguration.ToString(), -1) == 1)
                    {
                        Debug.Log("Current config has been the previously stored one");
                        UserPreferences.Instance.SetDefaultXRCameraConfig(cameraConfiguration.ToString());
                        return;
                    }

                    //if the setting is not the stored one
                    bool storedValue = false;
                    //check if there is a stored setting and if change the xr camera settings 
                    for (int i = 0; i < availableConfigs.Count; i++) // (string config in availableConfigs)
                    {
                        if (PlayerPrefs.GetInt(availableConfigs[i], -1) == 1)
                        {
                            Debug.Log($"Changing config to {availableConfigs[i]}");
                            storedValue = true;
                            UserPreferences.Instance.SetDefaultXRCameraConfig(availableConfigs[i]);
                            ChangeConfig();
                        }
                    }

                    //storing the default setting
                    if (storedValue == false)
                    {
                        Debug.Log("No previous config has been stored");
                        UserPreferences.Instance.SetDefaultXRCameraConfig(cameraConfiguration.ToString());
                    }
                }
            }
        }
    }

}
