using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using System.Collections;

namespace ArRetarget
{
    public class FacePosHandler : MonoBehaviour, IInit, IGet<int>, IJson, IPrefix
    {
        ARFaceManager m_FaceManager;
        GameObject faceObj;
        List<PoseData> facePoseData = new List<PoseData>();

        private void Awake()
        {
            m_FaceManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARFaceManager>();

            if (m_FaceManager != null)
                m_FaceManager.facesChanged += OnFaceUpdate;
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            TrackingDataManager dataManager = GameObject.FindGameObjectWithTag("manager").GetComponent<TrackingDataManager>();
            dataManager.SetRecorderReference(this.gameObject);
        }

        public void Init()
        {
            facePoseData.Clear();
        }

        public void GetFrameData(int frame)
        {
            if (faceObj != null)
            {
                var poseData = DataHelper.GetPoseData(faceObj, frame);
                facePoseData.Add(poseData);
            }
        }

        public string GetJsonString()
        {
            FacePoseContainer container = new FacePoseContainer()
            {
                facePoseList = facePoseData
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

            facePoseData.Clear();
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
