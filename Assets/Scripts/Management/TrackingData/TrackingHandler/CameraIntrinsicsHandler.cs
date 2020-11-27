using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

namespace ArRetarget
{
    public class CameraIntrinsicsHandler : MonoBehaviour, IJson, IInit, IGet<int>, IPrefix
    {
        private ARCameraManager arCameraManager;
        private List<CameraIntrinsics> cameraIntrinsics = new List<CameraIntrinsics>();

        public void Init()
        {
            //reference to the ar camera
            if (arCameraManager == null)
            {
                arCameraManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponentInChildren<ARCameraManager>();
                cameraIntrinsics.Clear();
                return;
            }
        }

        //getting camera intrinsics data
        public void GetFrameData(int frame)
        {
            CameraIntrinsics intrinsics = GetIntrinsics(frame);
            cameraIntrinsics.Add(intrinsics);
        }

        //provide json string
        public string GetJsonString()
        {
            CameraIntrinsicsContainer container = GetCameraIntrinsicsContainer();
            var json = JsonUtility.ToJson(container);
            cameraIntrinsics.Clear();

            return json;
        }

        //json file prefix
        public string GetJsonPrefix()
        {
            return "CI";
        }

        //generate container storing intrinsics data, camera config and screen resolution
        public CameraIntrinsicsContainer GetCameraIntrinsicsContainer()
        {
            CameraIntrinsicsContainer container = new CameraIntrinsicsContainer();
            container.cameraIntrinsics = this.cameraIntrinsics;
            container.cameraConfig = GetCameraConfiguration();
            container.resolution = GetResolution();

            return container;
        }

        #region prepare subsystem data
        //formatting subsystem camera config for json conversion
        private CameraConfig GetCameraConfiguration()
        {
            CameraConfig config = new CameraConfig();
            NativeArray<XRCameraConfiguration> xrCameraConfig = GetXRCameraConfigurations();

            config.fps = (int)xrCameraConfig[0].framerate;
            config.height = xrCameraConfig[0].height;
            config.width = xrCameraConfig[0].width;

            return config;
        }

        //formatting subsystem camera intrinsics for json conversion
        private CameraIntrinsics GetIntrinsics(int frame)
        {
            CameraIntrinsics intrinsics = new CameraIntrinsics();

            var tmp_intrinsics = GetCameraIntrinsics();
            intrinsics.flX = tmp_intrinsics.focalLength.x;
            intrinsics.flY = tmp_intrinsics.focalLength.y;
            intrinsics.ppX = tmp_intrinsics.principalPoint.x;
            intrinsics.ppY = tmp_intrinsics.principalPoint.y;
            intrinsics.frame = frame;
            return intrinsics;
        }

        //formatting screen resolution for json conversion
        private ScreenResolution GetResolution()
        {
            ScreenResolution res = new ScreenResolution();

            Vector2 m_res = GetScreenResolution();
            res.screenWidth = m_res.x;
            res.screenHeight = m_res.y;

            return res;
        }

        //accessing the screen resolution
        private Vector2 GetScreenResolution()
        {
            Vector2 screenResolution = new Vector2(Screen.width, Screen.height);
            return screenResolution;
        }
        #endregion

        #region receive subsystem data
        //accessing subsystem camera intrinsics data
        private XRCameraIntrinsics GetCameraIntrinsics()
        {
            XRCameraIntrinsics intrinsics;
            arCameraManager.TryGetIntrinsics(out intrinsics);

            return intrinsics;
        }

        //accessing subsystem camera config data
        private NativeArray<XRCameraConfiguration> GetXRCameraConfigurations()
        {
            NativeArray<XRCameraConfiguration> xrCameraConfigurations;
            xrCameraConfigurations = arCameraManager.GetConfigurations(allocator: Unity.Collections.Allocator.Temp);

            return xrCameraConfigurations;
        }
        #endregion
    }
}