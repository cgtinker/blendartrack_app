using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using System.Collections;

namespace ArRetarget
{
    public class FacePosHandler : MonoBehaviour, IInit<string, string>, IGet<int, bool>, IPrefix
    {
        ARFaceManager m_FaceManager;
        GameObject faceObj;

        #region init
        private void Awake()
        {
            m_FaceManager = GameObject.FindGameObjectWithTag("arSessionOrigin").GetComponent<ARFaceManager>();

            if (m_FaceManager != null)
                m_FaceManager.facesChanged += OnFaceUpdate;
        }

        private string filePath;
        public void Init(string path, string title)
        {
            write = false;
            curTick = 0;
            filePath = $"{path}{title}_{j_Prefix()}.json";
            JsonFileWriter.WriteDataToFile(path: filePath, text: "", title: "facePoseList", lastFrame: false);
            Debug.Log("init face pos");
        }
        #endregion

        #region getting and writing data
        public void GetFrameData(int frame, bool lastFrame)
        {
            if (faceObj != null)
            {
                AccessingPoseData(frame, lastFrame);
            }
        }

        private int curTick;
        static string jsonContents;
        private bool write;
        private void AccessingPoseData(int frame, bool lastFrame)
        {
            //getting vertex data
            var poseData = DataHelper.GetPoseData(faceObj, frame);
            string json = JsonUtility.ToJson(poseData);
            (jsonContents, curTick, write) = DataHelper.JsonContentTicker(lastFrame: lastFrame, curTick: curTick, reqTick: 53, contents: jsonContents, json: json);

            if (write)
            {
                JsonFileWriter.WriteDataToFile(path: filePath, text: jsonContents, title: "", lastFrame: lastFrame);
                jsonContents = "";
                if (lastFrame)
                    filePath = null;
            }
        }

        public string j_Prefix()
        {
            return "f-pose";
        }
        #endregion

        #region referencing face
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
        #endregion
    }
}
