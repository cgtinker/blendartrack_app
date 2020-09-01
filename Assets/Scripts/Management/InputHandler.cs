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
            switch (dataManager.DataType)
            {
                case DataManager.RecData.ArCore_CameraPose:
                    scene = ArCore_CameraPose;
                    break;
                case DataManager.RecData.ArCore_FaceMesh:
                    scene = ArCore_FaceMesh;
                    break;
                case DataManager.RecData.ArKit_CameraPose:
                    scene = ArKit_CameraPose;
                    break;
                case DataManager.RecData.ArKit_ShapeKeys:
                    scene = ArKit_ShapeKeys;
                    break;
            }

            if (scene != null)
                SceneManager.LoadScene(scene);
            else
                Debug.Log("Load main menu");
        }
    }
}
