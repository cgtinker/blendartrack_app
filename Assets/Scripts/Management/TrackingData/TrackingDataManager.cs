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
            Debug.Log("Session started");
        }

        //resetting as not all tracking models include all tracker interfaces
        public void ResetTrackerInterfaces()
        {
            getters.Clear();
            jsons.Clear();
            inits.Clear();
            stops.Clear();
            prefixs.Clear();
        }

        //the tracking references always contains some of the following interfaces
        public void SetRecorderReference(GameObject obj)
        {
            var arSession = GameObject.FindGameObjectWithTag("arSession").GetComponent<ARSession>();
            if (!arSession.matchFrameRate)
                arSession.matchFrameRate = true;

            if (obj.GetComponent<IInit>() != null)
                inits.Add(obj.GetComponent<IInit>());

            if (obj.GetComponent<IJson>() != null)
            {
                jsons.Add(obj.GetComponent<IJson>());
                prefixs.Add(obj.GetComponent<IPrefix>());
            }

            if (obj.GetComponent<IGet<int>>() != null)
                getters.Add(obj.GetComponent<IGet<int>>());

            if (obj.GetComponent<IStop>() != null)
                stops.Add(obj.GetComponent<IStop>());

            Debug.Log("Receiving Tracker Type Reference: " + obj.name);
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
            string msg = "";

            for (int i = 0; i < jsons.Count; i++)
            {
                string contents = jsons[i].GetJsonString();
                string prefix = prefixs[i].GetJsonPrefix();
                string time = FileManagement.GetDateTime();
                string filename = $"{time}_{prefix}.json";

                if (i < jsons.Count - 1)
                {
                    var tmp = $"{filename}{FileManagement.GetParagraph()}";
                    msg += tmp;
                }

                else
                {
                    var tmp = $"{filename}";
                    msg += tmp;
                }

                FileManagement.WriteDataToDisk(data: contents, persistentPath: persistentPath, filename: filename);
            }

            return msg;
        }
    }
}