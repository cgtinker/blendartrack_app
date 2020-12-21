using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using System.Collections;

namespace ArRetarget
{
    public class FacePosHandler : MonoBehaviour, IInit<string>, IGet<int, bool>, IPrefix
    {
        ARFaceManager m_FaceManager;
        GameObject faceObj;

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

        private string filePath;
        public void Init(string path)
        {
            filePath = $"{path}_{j_Prefix()}.json";
            JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "facePoseList", lastFrame: false);
            Debug.Log("init face pos");
        }

        public void GetFrameData(int frame, bool lastFrame)
        {
            if (faceObj != null)
            {
                var poseData = DataHelper.GetPoseData(faceObj, frame);
                string json = JsonUtility.ToJson(poseData);

                if (lastFrame)
                {
                    string par = "]}";
                    json += par;
                }

                JsonFileWriter.WriteDataToFile(path: filePath, text: json, title: "", lastFrame: lastFrame);
            }
        }

        public string j_Prefix()
        {
            return "f-pos";
        }

        private void OnDisable()
        {
            if (m_FaceManager != null)
                m_FaceManager.facesChanged -= OnFaceUpdate;
        }

        private void OnFaceUpdate(ARFacesChangedEventArgs args)
        {
            if (args.added.Count > 0 && !faceObj)
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
