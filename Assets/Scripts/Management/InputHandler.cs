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

        private void RestartSession()
        {
            Debug.Log("Restarting Session");

            string scene = null;
            switch (DeviceManager.Instance.device)
            {
                case DeviceManager.Device.Android:
                    switch (DeviceManager.Instance.Ability)
                    {
                        case DeviceManager.TrackingType.ArCore_CameraPose:
                            scene = ArCore_CameraPose;
                            break;

                        case DeviceManager.TrackingType.ArCore_FaceMesh:
                            scene = ArCore_FaceMesh;
                            break;
                    }
                    break;

                case DeviceManager.Device.iOS:
                    switch (DeviceManager.Instance.Ability)
                    {
                        case DeviceManager.TrackingType.ArKit_CameraPose:
                            scene = ArKit_CameraPose;
                            break;

                        case DeviceManager.TrackingType.ArKit_BlendShapes:
                            scene = ArKit_ShapeKeys;
                            break;
                    }
                    break;

                case DeviceManager.Device.Remote:
                    switch (DeviceManager.Instance.Ability)
                    {
                        case DeviceManager.TrackingType.Remote_CameraPose:
                            scene = ArCore_CameraPose;
                            break;

                        case DeviceManager.TrackingType.Remote_FaceMesh:
                            scene = ArCore_FaceMesh;
                            break;

                        case DeviceManager.TrackingType.Remote_FaceKeys:
                            scene = ArKit_ShapeKeys;
                            break;
                    }
                    break;
            }


            if (scene != null)
                SceneManager.LoadSceneAsync(scene);
            else
                Debug.Log("Load main menu");
        }
    }
}
