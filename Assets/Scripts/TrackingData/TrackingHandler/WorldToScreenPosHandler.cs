using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public class WorldToScreenPosHandler : MonoBehaviour, IInit<string>, IGet<int, bool>, IJson, IPrefix
    {
        //ReferenceCreator referenceCreator;
        ARRaycastManager arRaycastManager;
        private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

        private Camera arCamera;
        private Transform cameraTransform;

        //private Vector3 anchorPos;
        [HideInInspector]
        public GameObject motionAnchor;
        [HideInInspector]
        public float offset = 0f;
        [HideInInspector]
        public string motionAnchorTag;

        private float camera_width;
        private float camera_height;

        [HideInInspector]
        public List<ScreenPosData> screenPosList = new List<ScreenPosData>();
        private string filePath;

        #region interfaces
        public void Init(string path)
        {
            filePath = $"{path}_{j_Prefix()}.json";
            JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "screenPosData", lastFrame: false);


            //screen space camera
            if (arCamera == null)
            {
                var cam = GameObject.FindGameObjectWithTag("MainCamera");
                arCamera = cam.GetComponent<Camera>();
                cameraTransform = cam.transform;
            }

            if (arRaycastManager == null)
                arRaycastManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARRaycastManager>();


            //camera resolution to normalize screen pos data
            camera_width = arCamera.pixelWidth;
            camera_height = arCamera.pixelHeight;

            curTick = 0;
            write = false;
            jsonContents = "";
        }

        public void GetFrameData(int frame, bool lastFrame)
        {
            Vector3 position = DeviationRay();

            ScreenPosData data = VPToScreenPoint(cam: arCamera, cam_w: camera_width, cam_h: camera_height, pos: position, offset: 0, f: frame);
            string json = JsonUtility.ToJson(data);

            if (lastFrame)
            {
                string par = "]}";
                json += par;
            }
            WriteData(json, lastFrame);

            //JsonFileWriter.WriteDataToFile(path: filePath, text: json, title: "", lastFrame: lastFrame);
        }

        int curTick;
        bool write;
        static string jsonContents;
        private void WriteData(string json, bool lastFrame)
        {
            (jsonContents, curTick, write) = DataHelper.JsonContentTicker(lastFrame: lastFrame, curTick: curTick, reqTick: 49, contents: jsonContents, json: json);

            if (write)
            {
                JsonFileWriter.WriteDataToFile(path: filePath, text: jsonContents, title: "", lastFrame: lastFrame);
                jsonContents = "";
            }
        }

        float minDeviation = 0.70f;
        float maxDeviation = 1.70f;
        float iterations = 5;
        bool rayHit;
        Vector3 tar;
        public Vector3 DeviationRay()
        {
            rayHit = false;

            for (int i = 0; i < iterations; i++)
            {
                //screen center * deviation
                float u = (camera_width / 2) * Random.Range(minDeviation, maxDeviation);
                float v = (camera_height / 2) * Random.Range(minDeviation, maxDeviation);
                Vector2 pos = new Vector2(u, v);

                if (arRaycastManager.Raycast(pos, arRaycastHits))
                {
                    var pose = arRaycastHits[0].pose;
                    tar = pose.position;
                    rayHit = true;
                    break;
                }
            }

            if (!rayHit)
            {
                Debug.LogWarning("No plane object found");
                tar = new Vector3(arCamera.transform.position.x + 1.5f, arCamera.transform.position.y + 1.5f, arCamera.transform.position.z + 3f);
            }

            return tar;
        }

        public string j_Prefix()
        {
            return "screen";
        }

        public string j_String()
        {
            ScreenPosContainer container = new ScreenPosContainer()
            {
                screenPosData = screenPosList
            };

            var json = JsonUtility.ToJson(container);

            return json;
        }

        #endregion
        /// <summary>
        /// returns a vector with normalized x, y values at a given frame:
        /// (0, 0) -> bottom left
        /// (1, 1) -> top right
        /// z = distance to camera
        /// </summary>
        /// <param name="cam"></param> camera used for tracking
        /// <param name="cam_w"></param> camera pixel width
        /// <param name="cam_h"></param> camera pixel height
        /// <param name="pos"></param> vector for screen pos calc
        /// <param name="f"></param> frame
        /// <returns></returns>
        public static ScreenPosData VPToScreenPoint(Camera cam, float cam_w, float cam_h, Vector3 pos, float offset, int f)
        {
            ScreenPosData data = new ScreenPosData();
            Vector3 tar = new Vector3(pos.x, pos.y, pos.z + offset);
            var point = cam.WorldToScreenPoint(tar);

            //normalized vector
            var tmpScreenPos = new Vector3(point.x / cam_w, point.y / cam_h, point.z);

            data.frame = f;
            data.screenPos = tmpScreenPos;
            data.objPos = tar;

            return data;
        }
    }
}