using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;

namespace ArRetarget
{
    public class CameraIntrinsicsHandler : MonoBehaviour, IJson, IInit<string>, IPrefix, IGet<int, bool>
    {
        private Camera arCamera;
        private ARCameraManager arCameraManager;

        private string filePath;
        //int frame = 0;
        public void Init(string path)
        {
            filePath = $"{path}_{j_Prefix()}.json";
            JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "cameraProjection", lastFrame: false);

            //reference to the ar camera
            if (arCameraManager == null || arCamera == null)
            {
                var obj = GameObject.FindGameObjectWithTag("arSessionOrigin");
                arCameraManager = obj.GetComponentInChildren<ARCameraManager>();
                arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            }

            curTick = 0;
            contents = "";
            write = false;
        }

        public void GetFrameData(int frame, bool lastFrame)
        {
            if (arCamera == null)
            {
                arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                return;
            }

            if (!lastFrame)
            {

                Matrix4x4 m_matrix = arCamera.projectionMatrix;
                //Matrix4x4 m_matrix = (Matrix4x4)args.projectionMatrix;
                CameraProjectionMatrix tmp = new CameraProjectionMatrix();
                tmp.frame = frame;
                tmp.cameraProjectionMatrix = m_matrix;

                string json = JsonUtility.ToJson(tmp);
                //JsonFileWriter.WriteDataToFile(path: filePath, text: json, "", lastFrame: lastFrame);
                GetAndWriteData(lastFrame, json);
            }

            else if (lastFrame)
            {
                Matrix4x4 m_matrix = arCamera.projectionMatrix;
                CameraProjectionMatrix tmp = new CameraProjectionMatrix();
                tmp.frame = frame;
                tmp.cameraProjectionMatrix = m_matrix;

                CameraConfig m_config = GetCameraConfiguration();
                ScreenResolution m_res = GetResolution();

                string matrix = JsonUtility.ToJson(tmp);
                string config = JsonUtility.ToJson(m_config);
                string res = JsonUtility.ToJson(m_res);

                string par = "}";
                string quote = "\"";
                string json = $"{matrix}],{quote}cameraConfig{quote}:{config},{quote}resolution{quote}:{res}{par}";
                GetAndWriteData(lastFrame, json);
                //JsonFileWriter.WriteDataToFile(path: filePath, text: json, "", lastFrame: lastFrame);
            }
        }

        int curTick;
        static string contents;
        bool write;
        private void GetAndWriteData(bool lastFrame, string json)
        {
            (contents, curTick, write) = DataHelper.JsonContentTicker(lastFrame: lastFrame, curTick: curTick, reqTick: 23, contents: contents, json: json);

            if (write)
            {
                JsonFileWriter.WriteDataToFile(path: filePath, text: contents, "", lastFrame: lastFrame);
                contents = "";
            }
        }

        //provide json string
        public string j_String()
        {
            CameraIntrinsicsContainer container = GetCameraIntrinsicsContainer();
            var json = JsonUtility.ToJson(container);

            return json;
        }

        //json file prefix
        public string j_Prefix()
        {
            return "intrinsics";
        }

        //TODO: serialize camera projection & config
        //generate container storing intrinsics data, camera config and screen resolution
        public CameraIntrinsicsContainer GetCameraIntrinsicsContainer()
        {
            CameraIntrinsicsContainer container = new CameraIntrinsicsContainer();

            if (arCamera == null)
                return container;

            //container.cameraIntrinsics = this.cameraIntrinsics;
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