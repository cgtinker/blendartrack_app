using UnityEngine;

namespace ArRetarget
{
    public class TrackingDataManager : MonoBehaviour
    {
        CameraIntrinsicsHandler cameraIntrinsicsHandler;

        private string persistentPath;
        private bool _recording = false;
        public bool captureIntrinsics = true;
        private int frame = 0;

        private IGet<int> getter;
        private IJson json;
        private IInit init;
        private IStop stop;
        private IPrefix prefix;

        #region initialize tracking session
        void Start()
        {
            //set persistent path
            persistentPath = Application.persistentDataPath;
            _recording = false;

            //assign references
            cameraIntrinsicsHandler = this.gameObject.GetComponent<CameraIntrinsicsHandler>();

            Debug.Log("Session started");
        }

        //resetting as not all tracking models include all tracker interfaces
        public void ResetTrackerInterfaces()
        {
            getter = null;
            json = null;
            init = null;
            stop = null;
            prefix = null;
        }

        //the tracking references always contains some of the following interfaces
        public void TrackingReference(GameObject obj)
        {
            ResetTrackerInterfaces();

            //iinit -> setup || ijson -> serialze
            if (obj.GetComponent<IInit>() != null && obj.GetComponent<IJson>() != null)
            {
                init = obj.GetComponent<IInit>();
                json = obj.GetComponent<IJson>();
                prefix = obj.GetComponent<IPrefix>();
            }

            //iget -> pulling data
            if (obj.GetComponent<IGet<int>>() != null)
            {
                getter = obj.GetComponent<IGet<int>>();
            }

            //istop -> stop pushing data
            if (obj.GetComponent<IStop>() != null)
            {
                stop = obj.GetComponent<IStop>();
            }

            Debug.Log("Receiving Tracker Type Reference");
        }
        #endregion

        #region capturing
        //called by the recording button in the capture interface
        public void ToggleRecording()
        {
            if (!_recording)
            {
                OnInitRetargeting();
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
            Debug.Log("Init Retargeting");
            init.Init();

            if (captureIntrinsics)
            {
                cameraIntrinsicsHandler.Init();
            }
        }

        //called by toggle button event
        private void OnStopRetargeting()
        {
            if (stop != null)
            {
                Debug.Log("Stop Retargeting");
                //stopping interface for ios face tracking as it is pushing and not pulling data
                stop.StopTracking();
            }
        }

        protected virtual void OnEnableTracking()
        {
            Application.onBeforeRender += OnBeforeRenderPreformUpdate;
        }

        protected virtual void OnDisableTracking()
        {
            Application.onBeforeRender -= OnBeforeRenderPreformUpdate;
        }

        //update tracking data before the render event
        protected virtual void OnBeforeRenderPreformUpdate()
        {
            if (_recording && getter != null)
            {
                frame++;
                getter.GetFrameData(frame);

                //capturing intrinics data while capturing video
                if (captureIntrinsics)
                {
                    cameraIntrinsicsHandler.GetFrameData(frame);
                }
            }
        }
        #endregion

        public string SerializeJson()
        {
            //file contents
            string contents = json.GetJsonString();

            //file name
            string prefix = this.prefix.GetJsonPrefix();
            string time = FileManagement.GetDateTime();
            string filename = $"{time}_{prefix}.json";

            FileManagement.WriteDataToDisk(data: contents, persistentPath: persistentPath, filename: filename);

            if (captureIntrinsics)
            {
                //file contents
                string intrinsicsContents = cameraIntrinsicsHandler.GetJsonString();

                //file name
                string intrinsicsPrefix = cameraIntrinsicsHandler.GetJsonPrefix();
                string intrinsicsFilename = $"{time}_{intrinsicsPrefix}.json";

                FileManagement.WriteDataToDisk(data: intrinsicsContents, persistentPath: persistentPath, filename: intrinsicsFilename);

                string message = $"{filename}{FileManagement.GetParagraph()}{intrinsicsFilename}";

                return message;
            }

            else
                return filename;
        }
    }
}