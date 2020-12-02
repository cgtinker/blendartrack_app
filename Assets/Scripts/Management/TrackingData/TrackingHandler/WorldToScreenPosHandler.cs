using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class WorldToScreenPosHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        private Camera arCamera;
        private Vector3 anchorPos;

        private float camera_width;
        private float camera_height;

        public List<ScreenPosData> screenPosList = new List<ScreenPosData>();

        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            var dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
        }

        #region interfaces
        public void Init()
        {
            //screen space camera
            arCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            var anchor = GameObject.FindGameObjectWithTag("anchor");
            //camera resolution to normalize screen pos data
            camera_width = arCamera.pixelWidth;
            camera_height = arCamera.pixelHeight;
            //tracking an anchor
            if (anchor != null)
                anchorPos = anchor.transform.position;

            else
                anchorPos = new Vector3(0, 0, 0);
        }

        public void GetFrameData(int frame)
        {
            ScreenPosData data = VPToScreenPoint(cam: arCamera, cam_w: camera_width, cam_h: camera_height, pos: anchorPos, f: frame);
            screenPosList.Add(data);
        }

        public string GetJsonPrefix()
        {
            return "WP";
        }

        public string GetJsonString()
        {
            ScreenPosContainer container = new ScreenPosContainer()
            {
                anchor = new Vector()
                {
                    x = anchorPos.x,
                    y = anchorPos.y,
                    z = anchorPos.z
                },
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
        public static ScreenPosData VPToScreenPoint(Camera cam, float cam_w, float cam_h, Vector3 pos, int f)
        {
            ScreenPosData data = new ScreenPosData();
            var point = cam.WorldToScreenPoint(pos);

            Vector tmp = new Vector
            {
                //normalizing values
                x = point.x / cam_w,
                y = point.y / cam_h,
                z = point.z
            };

            data.frame = f;
            data.screenPos = tmp;

            return data;
        }
    }
}