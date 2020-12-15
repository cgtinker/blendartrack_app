using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public class HeadPositionHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        ARFaceManager m_FaceManager;
        GameObject faceObj;
        List<PoseData> headPosData = new List<PoseData>();

        private void Start()
        {
            m_FaceManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARFaceManager>();

            if (m_FaceManager != null)
                m_FaceManager.facesChanged += OnFaceUpdate;
        }

        public void Init()
        {
            headPosData.Clear();
        }

        public void GetFrameData(int frame)
        {
            if (faceObj != null)
            {
                var poseData = DataHelper.GetPoseData(faceObj, frame);
                headPosData.Add(poseData);
            }
        }

        public string GetJsonString()
        {
            CameraPoseContainer container = new CameraPoseContainer()
            {
                cameraPoseList = headPosData
            };

            //create json string
            var json = JsonUtility.ToJson(container);
            return json;
        }

        public string GetJsonPrefix()
        {
            return "HP";
        }

        private void OnDisable()
        {
            if (m_FaceManager != null)
                m_FaceManager.facesChanged -= OnFaceUpdate;

            headPosData.Clear();
        }

        private void OnFaceUpdate(ARFacesChangedEventArgs args)
        {
            if (args.added.Count > 0)
            {
                faceObj = args.added[0].gameObject;
            }

            if (args.removed.Count > 0)
            {
                faceObj = null;
            }
        }
    }
}
