using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public class WorldToScreenPosHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
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

        #region interfaces
        public void Init()
        {
            screenPosList.Clear();

            //screen space camera
            if (arCamera == null)
            {
                var cam = GameObject.FindGameObjectWithTag("MainCamera");
                arCamera = cam.GetComponent<Camera>();
                cameraTransform = cam.transform;
            }

            /*
            if (referenceCreator == null)
                referenceCreator = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ReferenceCreator>();
            */
            if (arRaycastManager == null)
                arRaycastManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARRaycastManager>();


            //camera resolution to normalize screen pos data
            camera_width = arCamera.pixelWidth;
            camera_height = arCamera.pixelHeight;
        }

        public void GetFrameData(int frame)
        {
            //Vector3 position = GetReferencePoint();
            Vector3 position = DeviationRay();

            ScreenPosData data = VPToScreenPoint(cam: arCamera, cam_w: camera_width, cam_h: camera_height, pos: position, offset: 0, f: frame);

            screenPosList.Add(data);
        }

        float minDeviation = 0.70f;
        float maxDeviation = 1.70f;
        float iterations = 7;
        public Vector3 DeviationRay()
        {
            Vector3 tar = Vector3.zero;

            for (int i = 0; i < iterations; i++)
            {
                //screen center * deviation
                float u = (camera_width / 2) * Random.Range(minDeviation, maxDeviation);
                float v = (camera_height / 2) * Random.Range(minDeviation, maxDeviation);
                Vector2 pos = new Vector2(u, v);


                if (arRaycastManager.Raycast(pos, arRaycastHits))
                {
                    Debug.Log(i);
                    var pose = arRaycastHits[0].pose;
                    tar = pose.position;
                    break;
                }
            }

            return tar;
        }

        //depricated anchor method
        /*
        public Vector3 GetReferencePoint()
        {
            Vector3 m_vector = Vector3.zero;

            var anchors = referenceCreator.anchors;

            if (anchors.Count > 0)
            {
                foreach (GameObject anchor in anchors)
                {
                    if (anchor.gameObject.GetComponentInChildren<MeshRenderer>().isVisible)
                    {
                        float dist = (Vector3.Distance(cameraTransform.position, anchor.transform.position));

                        if (dist > 0f && dist < 10f)
                        {
                            m_vector = anchor.transform.position;
                            break;
                        }
                    }
                }
            }

            return m_vector;
        }
        */

        public string GetJsonPrefix()
        {
            return "screen";
        }

        public string GetJsonString()
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
            var tmpScreenPos = DataHelper.GetVector(new Vector3(point.x / cam_w, point.y / cam_h, point.z));
            //obj pos
            Vector tmpObjPos = DataHelper.GetVector(tar);

            data.frame = f;
            data.screenPos = tmpScreenPos;
            data.objPos = tmpObjPos;

            return data;
        }
    }
}