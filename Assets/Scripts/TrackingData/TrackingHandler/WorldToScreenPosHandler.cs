using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public class WorldToScreenPosHandler : MonoBehaviour, IInit<string, string>, IGet<int, bool>, IPrefix //IJson
    {
        //ReferenceCreator referenceCreator;
        ARRaycastManager arRaycastManager;
        private List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
        ARPlaneManager arPlaneManager;
        ReferenceCreator referenceCreator;

        private Camera arCamera;

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
        public void Init(string path, string title)
        {
            filePath = $"{path}{title}_{j_Prefix()}.json";
            JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "screenPosData", lastFrame: false);


            //screen space camera
            if (arCamera == null)
            {
                var cam = GameObject.FindGameObjectWithTag("MainCamera");
                arCamera = cam.GetComponent<Camera>();
            }

            if (arRaycastManager == null || arPlaneManager == null)
            {
                var sessionOrigin = GameObject.FindGameObjectWithTag("arSessionOrigin");
                arRaycastManager = sessionOrigin.GetComponent<ARRaycastManager>();
                arPlaneManager = sessionOrigin.GetComponent<ARPlaneManager>();
                referenceCreator = sessionOrigin.GetComponent<ReferenceCreator>();
            }

            curTick = 0;
            write = false;
            jsonContents = "";
            //camera resolution to normalize screen pos data
            camera_width = arCamera.pixelWidth;
            camera_height = arCamera.pixelHeight;
        }

        public void GetFrameData(int frame, bool lastFrame)
        {

            //Vector3 position = DeviationRay();
            //Vector3 position = motionAnchor.transform.position;
            Vector3 position = PosByMarker();

            ScreenPosData data = VPToScreenPoint(cam: arCamera, cam_w: camera_width, cam_h: camera_height, target: position, f: frame);
            string json = JsonUtility.ToJson(data);

            WriteData(json, lastFrame);
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

        public Vector3 PosByMarker()
        {
            rayHit = false;

            var markers = referenceCreator.anchors;
            for (int i = 0; i < markers.Count; i++)
            {
                var tmp_pos = markers[i].transform.position;
                var tmp_point = arCamera.WorldToScreenPoint(tmp_pos);
                var vec = new Vector3(tmp_point.x / camera_width, tmp_point.y / camera_height, tmp_point.z);

                if (vec.x > 0.005f && vec.x < 0.495f && vec.z > 0 || vec.x > 0.505f && vec.x < 0.995f && vec.z > 0)
                {
                    if (vec.y > 0.005f && vec.y < 0.495f || vec.y > 0.505f && vec.y < 0.995f)
                    {
                        tar = tmp_pos;
                        rayHit = true;
                        break;
                    }
                }
            }

            if (!rayHit)
                tar = DeviationRay();

            return tar;
        }
        /*
        int iter = 0;
        public Vector3 RayByPlanePosition()
        {
            iter = 0;
            rayHit = false;
            var trackables = arPlaneManager.trackables;

            foreach (ARPlane plane in trackables)
            {
                iter++;
                if (iter > iterations)
                {
                    rayHit = false;
                    break;
                }

                var tmp_pos = plane.transform.position;
                var tmp_point = arCamera.WorldToScreenPoint(tmp_pos);
                var tmp_vec = new Vector3(tmp_point.x / camera_width, tmp_point.y / camera_height, tmp_point.z);

                if (tmp_vec.z > 0 && tmp_vec.x > 0.1f && tmp_vec.y > 0.1f && tmp_vec.x < 0.9f && tmp_vec.y < 0.9f)
                {
                    tar = tmp_pos;
                    rayHit = true;
                    break;
                }
            }

            if (!rayHit)
            {
                Debug.LogWarning("no planes?");
                tar = DeviationRay();
            }

            return tar;
        }
        */
        float iterations = 5;
        bool rayHit;
        Vector3 tar;
        public Vector3 DeviationRay()
        {
            rayHit = false;

            for (int i = 0; i < iterations; i++)
            {
                //screen center * deviation
                float u = (camera_width / 2) * Random.Range(0.2f, 1.8f);
                float v = (camera_height / 2) * Random.Range(0.1f, 1.2f);
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
                tar = new Vector3(arCamera.transform.position.x + 1.5f, arCamera.transform.position.y + 1.5f, arCamera.transform.position.z + 3f);
            }

            return tar;
        }

        public string j_Prefix()
        {
            return "screen";
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
        public static ScreenPosData VPToScreenPoint(Camera cam, float cam_w, float cam_h, Vector3 target, int f)
        {
            ScreenPosData data = new ScreenPosData();
            var point = cam.WorldToScreenPoint(target);

            //normalized vector
            var tmpScreenPos = new Vector3(point.x / cam_w, point.y / cam_h, point.z);

            data.frame = f;
            data.screenPos = tmpScreenPos;
            data.objPos = target;

            return data;
        }
    }
}