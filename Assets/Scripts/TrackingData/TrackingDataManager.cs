using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;

namespace ArRetarget
{
    public class TrackingDataManager : MonoBehaviour
    {
        private string persistentPath;
        public bool _recording = false;
        public bool captureIntrinsics = true;
        private int frame = 0;
        private ARSession arSession;

        private List<IGet<int>> getters = new List<IGet<int>>();
        private List<IJson> jsons = new List<IJson>();
        private List<IInit> inits = new List<IInit>();
        private List<IStop> stops = new List<IStop>();
        private List<IPrefix> prefixs = new List<IPrefix>();

        #region initialize tracking session
        void Start()
        {
            //set persistent path
            persistentPath = Application.persistentDataPath;
            _recording = false;

            //match the frame rate of ar and unity updates
            if (arSession == null)
            {
                arSession = GameObject.FindGameObjectWithTag("arSession").GetComponent<ARSession>();
            }

            if (!arSession.matchFrameRate)
                arSession.matchFrameRate = true;

            Debug.Log("Session started");
        }

        //resetting as not all tracking models include all tracker interfaces
        public void ResetTrackerInterfaces()
        {
            var referencer = GameObject.FindGameObjectWithTag("referencer");
            if (referencer != null)
            {
                var script = referencer.GetComponent<TrackerReferencer>();
                script.assigned = false;
            }

            getters.Clear();
            jsons.Clear();
            inits.Clear();
            stops.Clear();
            prefixs.Clear();
        }

        //the tracking references always contains some of the following interfaces
        public void SetRecorderReference(GameObject obj)
        {
            if (obj.GetComponent<IInit>() != null)
            {
                inits.Add(obj.GetComponent<IInit>());
            }

            if (obj.GetComponent<IJson>() != null)
            {
                jsons.Add(obj.GetComponent<IJson>());
                prefixs.Add(obj.GetComponent<IPrefix>());
            }

            if (obj.GetComponent<IGet<int>>() != null)
            {
                getters.Add(obj.GetComponent<IGet<int>>());
            }

            if (obj.GetComponent<IStop>() != null)
            {
                stops.Add(obj.GetComponent<IStop>());
            }

            Debug.Log("Received: " + obj.name);
        }
        #endregion

        #region capturing
        //called by the recording button in the capture interface
        public void ToggleRecording()
        {
            if (!_recording)
            {
                OnInitRetargeting();
                frame = 0;
                OnEnableTracking();
            }

            else
            {
                OnDisableTracking();
                OnStopRetargeting();
            }

            _recording = !_recording;

            Debug.Log($"Recording: {_recording}");
        }

        //called by toggle button event
        private void OnInitRetargeting()
        {
            foreach (var init in inits)
            {
                init.Init();
            }
        }

        //called by toggle button event
        private void OnStopRetargeting()
        {
            if (stops.Count > 0)
            {
                Debug.Log("stops: " + stops.Count);
                foreach (var stop in stops)
                {
                    stop.StopTracking();
                }
            }
        }

        protected virtual void OnEnableTracking()
        {
            Debug.Log("Enabled Tracking");
            Application.onBeforeRender += OnBeforeRenderPreformUpdate;
        }

        protected virtual void OnDisableTracking()
        {
            Debug.Log("Disabled Tracking");
            Application.onBeforeRender -= OnBeforeRenderPreformUpdate;
        }

        //update tracking data before the render event
        protected virtual void OnBeforeRenderPreformUpdate()
        {

            if (_recording && getters.Count > 0)
            {
                frame++;

                foreach (var getter in getters)
                {
                    getter.GetFrameData(frame);
                }
            }
        }
        #endregion

        public string SerializeJson()
        {


            string time = FileManagement.GetDateTime();
            string dir_path = $"{persistentPath}/{time}_{prefixs[0].GetJsonPrefix()}";
            string msg = $"{FileManagement.GetParagraph()}{time}_{prefixs[0].GetJsonPrefix()}";

            FileManagement.CreateDirectory(dir_path);

            for (int i = 0; i < jsons.Count; i++)
            {
                string contents = jsons[i].GetJsonString();
                string prefix = prefixs[i].GetJsonPrefix();
                string filename = $"{time}_{prefix}.json";

                FileManagement.WriteDataToDisk(data: contents, persistentPath: dir_path, filename: filename);
            }

            if (PlayerPrefs.GetInt("recordCam", -1) == -1)
            {
                return msg;
            }

            else
            {
                string tmp = $"{msg}{FileManagement.GetParagraph()}{FileManagement.GetParagraph()}recording saved to gallery";
                return tmp;
            }
        }
    }
}