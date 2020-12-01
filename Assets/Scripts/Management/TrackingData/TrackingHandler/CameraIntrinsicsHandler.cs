using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using System.Collections;

namespace ArRetarget
{
    public class CameraIntrinsicsHandler : MonoBehaviour, IJson, IInit, IPrefix, IGet<int>
    {
        private Camera arCamera;
        private ARCameraManager arCameraManager;
        private List<CameraProjectionMatrix> cameraProjection = new List<CameraProjectionMatrix>();

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
        }

        //int frame = 0;
        public void Init()
        {
            //reference to the ar camera
            if (arCameraManager == null || arCamera == null)
            {
                var obj = GameObject.FindGameObjectWithTag("arSessionOrigin");
                arCameraManager = obj.GetComponentInChildren<ARCameraManager>();
                arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

                //restting values
                //EnableFrameUpdate();
            }

            else
            {
                //frame = 0;
                cameraProjection.Clear();
                //EnableFrameUpdate();
            }
        }
        public void GetFrameData(int frame)
        {
            OnFrameReceived(frame);
        }
        /*
        public void EnableFrameUpdate()
        {
            arCameraManager.frameReceived += OnFrameReceived;
        }

        public void StopTracking()
        {
            arCameraManager.frameReceived += OnFrameReceived;
        }
        */
        public void OnFrameReceived(int frame)  //(ARCameraFrameEventArgs args)
        {
            if (arCamera == null)
            {
                arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                return;
            }

            Matrix4x4 m_matrix = arCamera.projectionMatrix;
            //Matrix4x4 m_matrix = (Matrix4x4)args.projectionMatrix;
            CameraProjectionMatrix tmp = new CameraProjectionMatrix();
            tmp.frame = frame;
            tmp.cameraProjectionMatrix = m_matrix;

            cameraProjection.Add(tmp);
        }

        //provide json string
        public string GetJsonString()
        {
            CameraIntrinsicsContainer container = GetCameraIntrinsicsContainer();
            var json = JsonUtility.ToJson(container);
            cameraProjection.Clear();

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

            if (arCamera == null)
                return container;

            //container.cameraIntrinsics = this.cameraIntrinsics;
            container.cameraProjection = cameraProjection;
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