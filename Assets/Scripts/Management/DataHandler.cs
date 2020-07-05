using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArRetarget
{
    public class DataHandler : MonoBehaviour
    {
        private string attachmentPath;
        private List<CameraData> cameraData = new List<CameraData>();
        private bool recording;
        GameObject MainCamera;

        //camera is always enabled during the ar session so we get a ref at start
        private void Start()
        {
            attachmentPath = Application.persistentDataPath + "/temp.json";
            Debug.Log("Session started");
            MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            recording = false;
            ClearList();
        }

        int f = 0;
        private void Update()
        {
            if (recording)
            {
                f++;
                SetCameraData(f);
            }
        }

        public void ToggleRecording()
        {
            recording = !recording;
            Debug.Log($"recording: {recording}");
        }

        public void SetCameraData(int f)
        {
            CameraData newData = new CameraData()
            {
                px = MainCamera.transform.position.x,
                py = MainCamera.transform.position.y,
                pz = MainCamera.transform.position.z,

                rx = MainCamera.transform.eulerAngles.x,
                ry = MainCamera.transform.eulerAngles.y,
                rz = MainCamera.transform.eulerAngles.z,

                f = f
            };

            cameraData.Add(newData);
        }

        public void JsonSerialization()
        {
            _CameraData data = new _CameraData()
            {
                _cameraData = cameraData
            };

            //json in app data
            var json = JsonUtility.ToJson(data);
            File.WriteAllText(attachmentPath, json);

            DateTime localDate = DateTime.Now;
            string mailSubject = "Ar Retarget " + localDate.ToString();
            string s = Environment.NewLine;
            string mailText = "Ar Retarget " + localDate.ToString();

            StartCoroutine(NativeShare(filePath: attachmentPath, subject: mailSubject, text: mailText));
        }

        private IEnumerator NativeShare(string filePath, string subject, string text)
        {
            yield return new WaitForEndOfFrame();
            new NativeShare().AddFile(filePath).SetSubject(subject).SetText(text).Share();
        }

        //clearing the list after a session
        public void ClearList()
        {
            //Debug.Log("Cleared Cache");
            //DeleteFile.FileAtMediaPath(attachmentPath);
            cameraData.Clear();
        }
    }

    [System.Serializable]
    public class _CameraData
    {
        public List<CameraData> _cameraData;
    }
}
