using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class WorldToScreenPosHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        private Camera arCamera;
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
            //screen space camera
            arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

            //camera resolution to normalize screen pos data
            camera_width = arCamera.pixelWidth;
            camera_height = arCamera.pixelHeight;
        }

        public void GetFrameData(int frame)
        {
            if (motionAnchor == null)
            {
                motionAnchor = GameObject.FindGameObjectWithTag(motionAnchorTag);
                if (motionAnchor == null)
                {
                    screenPosList.Add(new ScreenPosData());
                    return;
                }
            }

            ScreenPosData data = VPToScreenPoint(cam: arCamera, cam_w: camera_width, cam_h: camera_height, pos: motionAnchor.transform.position, offset: offset, f: frame);
            screenPosList.Add(data);
        }

        public string GetJsonPrefix()
        {
            return "MA";
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

            Vector tmpScreenPos = new Vector
            {
                //normalizing values
                x = point.x / cam_w,
                y = point.y / cam_h,
                z = point.z
            };

            Vector tmpObjPos = new Vector
            {
                x = tar.x,
                y = tar.y,
                z = tar.z
            };

            data.frame = f;
            data.screenPos = tmpScreenPos;
            data.objPos = tmpObjPos;

            return data;
        }
    }
}