using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArRetarget
{
    public class InputHandler : MonoBehaviour
    {
        DataManager dataManager;

        public string ArCore_CameraPose = "CameraTracker";
        public string ArCore_FaceMesh = "FaceTracker";
        public string ArKit_CameraPose = "CameraTracker";
        public string ArKit_ShapeKeys = "Sample Scene";


        private void Awake()
        {
            dataManager = this.gameObject.GetComponent<DataManager>();
        }

        public void TrackData()
        {
            dataManager.ToggleRecording();
        }

        public void SerializeAndShare()
        {
            dataManager.ToggleRecording();
            dataManager.SerializeJson();
            RestartSession();
        }

        public void RestartSession()
        {
            Debug.Log("Restarting Session");

            string scene = null;
            switch (DeviceManager.Instance.device)
            {
                case DeviceManager.Device.Android:
                    switch (DeviceManager.Instance.DataType)
                    {
                        case DeviceManager.RecData.ArCore_CameraPose:
                            scene = ArCore_CameraPose;
                            break;

                        case DeviceManager.RecData.ArCore_FaceMesh:
                            scene = ArCore_FaceMesh;
                            break;
                    }
                    break;

                case DeviceManager.Device.iOs:
                    switch (DeviceManager.Instance.DataType)
                    {
                        case DeviceManager.RecData.ArKit_CameraPose:
                            scene = ArKit_CameraPose;
                            break;

                        case DeviceManager.RecData.ArKit_ShapeKeys:
                            scene = ArKit_ShapeKeys;
                            break;
                    }
                    break;

                case DeviceManager.Device.Remote:
                    switch (DeviceManager.Instance.DataType)
                    {
                        case DeviceManager.RecData.Remote_CameraPose:
                            scene = ArKit_CameraPose;
                            break;

                        case DeviceManager.RecData.Remote_FaceMesh:
                            scene = ArKit_ShapeKeys;
                            break;

                        case DeviceManager.RecData.Remote_FaceKeys:
                            scene = ArKit_ShapeKeys;
                            break;
                    }
                    break;
            }


            if (scene != null)
                SceneManager.LoadScene(scene);
            else
                Debug.Log("Load main menu");
        }
    }
}
